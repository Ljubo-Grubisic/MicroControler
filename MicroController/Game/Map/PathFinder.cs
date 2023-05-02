using microController.graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using System.Xml.Linq;
using microController.helpers;
using System.Runtime.CompilerServices;

namespace microController.game
{
    public class PathFinder
    {
        public List<int[]> Path = new List<int[]>();

        private Node[,] MapInNodes;

        private List<Node> Opened = new List<Node>();
        private List<Node> Closed = new List<Node>();

        public static Node FirstNode;
        public static Node LastNode;

        private static Rectangle ArrowRectangle;
        private static Rectangle ColorRectangle;
        
        public PathFinder(Map map)
        {
            ArrowRectangle = new Rectangle(new Vector2f(), new Vector2f(map.SquareSize, map.SquareSize), ImageHelper.LoadImgNoBackground("Arrow.jpg")) { FillColor = Color.Yellow };
            ColorRectangle = new Rectangle(new Vector2f(), new Vector2f(map.SquareSize, map.SquareSize));
            LoadMapInNodes(map);
        }

        public void FindPath(Node StartingNode, Node EndingNode, Map map)
        {
            FirstNode = new Node();
            LastNode = new Node();
            Node current;
            bool startup = true;
            Opened.Clear();
            Closed.Clear();
            FirstNode = StartingNode;
            LoadMapInNodes(map);
            while (true)
            {
                if (startup)
                {
                    current = StartingNode;
                    startup = false;
                }
                else
                    current = NodeWithLowestF();
                Opened.Remove(current);
                Closed.Add(current);

                if (current.Id.I == EndingNode.Id.I && current.Id.J == EndingNode.Id.J)
                {
                    LastNode = current;  
                    return;
                }

                foreach (Node.NodeId node in NodeNeighbour(current))
                {
                    if (node.I >= 0 && node.I < map.DataSize.X && node.J >= 0 && node.J < map.DataSize.Y)
                    {
                        Node neighbour = MapInNodes[node.I, node.J];

                        if (!DoesClosedContain(neighbour))
                        {
                            if (neighbour.Traversable)
                            {
                                if (GetDistance(neighbour, EndingNode) < neighbour.H || !Opened.Contains(neighbour))
                                {
                                    neighbour.G = GetDistance(neighbour, StartingNode);
                                    neighbour.H = GetDistance(neighbour, EndingNode);
                                    neighbour.ParentId = current.Id;
                                    if (!DoesOpenedContain(neighbour))
                                        Opened.Add(neighbour);
                                }
                            }
                        }
                    }
                }
            }
        }

        private Node.NodeId LastId;
        public void DisplayPath(RenderWindow window, Map map, Node node)
        {
            if (node.Id.I == LastNode.Id.I && node.Id.J == LastNode.Id.J)
            {
                ColorRectangle.Position = new Vector2f(node.Id.J * map.SquareSize, node.Id.I * map.SquareSize) + (Vector2f)map.Window.Position;
                ColorRectangle.Size = new Vector2f(map.SquareSize, map.SquareSize);
                ColorRectangle.FillColor = Color.Green;
                ColorRectangle.Draw(window);
            }
            else if (node.Id.I == FirstNode.Id.I && node.Id.J == FirstNode.Id.J)
            {
                ColorRectangle.Position = new Vector2f(node.Id.J * map.SquareSize, node.Id.I * map.SquareSize) + (Vector2f)map.Window.Position;
                ColorRectangle.Size = new Vector2f(map.SquareSize, map.SquareSize);
                ColorRectangle.FillColor = Color.Blue;
                ColorRectangle.Draw(window);
            }
            else
            {
                ArrowRectangle.Position = new Vector2f(node.Id.J * map.SquareSize + (map.SquareSize / 2), node.Id.I * map.SquareSize + (map.SquareSize / 2)) + (Vector2f)map.Window.Position;
                ArrowRectangle.Size = new Vector2f(map.SquareSize, map.SquareSize);
                ArrowRectangle.Origin = new Vector2f(map.SquareSize / 2, map.SquareSize / 2);
                if (LastId.I > node.Id.I)
                {
                    ArrowRectangle.Rotation = 180;
                }
                if (LastId.I < node.Id.I)
                {
                    ArrowRectangle.Rotation = 0;
                }
                if (LastId.J > node.Id.J)
                {
                    ArrowRectangle.Rotation = 90;
                }
                if (LastId.J < node.Id.J)
                {
                    ArrowRectangle.Rotation = -90;
                }

                ArrowRectangle.Draw(window);
            }

            LastId = node.Id;

            if (node.ParentId.I != 0 && node.ParentId.J != 0)
            {
                DisplayPath(window, map, Closed[GetNodeFromClosedWithId(node.ParentId)]);
            }
            LastId = new Node.NodeId { };
        }

        private int GetNodeFromClosedWithId(Node.NodeId id)
        {
            for (int i = 0; i < Closed.Count; i++)
            {
                if (Closed[i].Id.I == id.I && Closed[i].Id.J == id.J)
                {
                    return i;
                }
            }
            return 0;
        }

        private void LoadMapInNodes(Map map)
        {
            MapInNodes = new Node[map.DataSize.X, map.DataSize.Y];
            for (int i = 0; i < map.DataSize.X; i++)
            {
                for (int j = 0; j < map.DataSize.Y; j++)
                {
                    if (map.Data[i, j] == 0)
                        MapInNodes[i, j] = new Node() { Id = { I = i, J = j }, Traversable = true };
                    else
                        MapInNodes[i, j] = new Node() { Id = { I = i, J = j }, Traversable = false };
                }
            }
        }

        private bool DoesOpenedContain(Node node)
        {
            for (int i = 0; i < Opened.Count; i++)
            {
                if (Opened[i].Id.I == node.Id.I && Opened[i].Id.J == node.Id.J)
                {
                    return true;
                }
            }
            return false;
        }

        private bool DoesClosedContain(Node node)
        {
            for (int i = 0; i < Closed.Count; i++)
            {
                if (Closed[i].Id.I == node.Id.I && Closed[i].Id.J == node.Id.J)
                {
                    return true;
                }
            }
            return false;
        }


        private int GetDistance(Node StartingNode, Node EndingNode)
        {
            int distance = 0;
            distance += StartingNode.Id.I - EndingNode.Id.I - 1;
            distance += StartingNode.Id.J - EndingNode.Id.J - 1;

            if (distance < 0)
                distance = -distance;
            return distance;
        }

        private Node.NodeId[] NodeNeighbour(Node node)
        {
            Node.NodeId[] neighbours = new Node.NodeId[4];
            neighbours[0] = new Node.NodeId { I = node.Id.I + 1, J = node.Id.J };
            neighbours[1] = new Node.NodeId { I = node.Id.I - 1, J = node.Id.J };
            neighbours[2] = new Node.NodeId { I = node.Id.I, J = node.Id.J + 1 };
            neighbours[3] = new Node.NodeId { I = node.Id.I, J = node.Id.J - 1 };
            return neighbours;
        }

        private Node NodeWithLowestF()
        {
            Node node = new Node() { G = 10000000, H = 10000000 };
            for (int i = 0; i < Opened.Count; i++)
            {
                if (Opened[i].F < node.F)
                    node = Opened[i];
            }
            return node;
        }
    }

    public struct Node
    {
        /// <summary>
        /// Distance from starting node
        /// </summary>
        public float G { get; set; }
        /// <summary>
        /// Distance from end node
        /// </summary>
        public float H { get; set; }
        /// <summary>
        /// Total cost
        /// </summary>
        public float F { get => G + H; }
        public bool Traversable;

        public NodeId Id;
        public NodeId ParentId;

        public struct NodeId
        {
            public int I { get; set; }
            public int J { get; set; }
        }
    }
}

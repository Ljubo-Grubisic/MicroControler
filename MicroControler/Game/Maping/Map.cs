using MicroControler.Game.Text;
using MicroControler.Game.Entity;
using MicroControler.Shapes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Threading;

namespace MicroControler.Game.Maping
{
    public class Map
    {
        #region Public variables
        private byte[,] data;
        public Vector2i DataSize { get; private set; }
        public int SquareSize { get; set; }

        public Window MinimapWindow;
        public Window MapWindow;

        #region Chunks
        public Vector2i ChunkWithPlayer;
        public Vector2i ChunkWithPlayerOnScreen;
        public Vector2i ChunkNum;
        public Vector2i ChunkSize;
        /// <summary>
        /// Left 0, right 1, top 2, bottom 3
        /// </summary>
        private int[] ChunksAroundMain = new int[4];
        private Vector2i ChunksOnScreen;
        private Vector2i SquareStarting;
        #endregion
        #endregion

        #region Private variables
        private Color OutLineColor = new Color(240, 240, 240);
        private readonly Rectangle EmptySquare;
        private readonly Rectangle FullSquare;
        private readonly Rectangle ChunkRectangle = new Rectangle(0f, 0f, 0f, 0f) { FillColor = Color.Transparent, OutlineColor = Color.Red, OutlineThickness = 2f };
        #endregion

        #region Getters and Setters
        public byte[,] Data
        {
            get { return data; }
            set
            {
                data = value;
                this.DataSize = new Vector2i(Data.GetLength(0), Data.GetLength(1));
            }
        }
        #endregion

        public Map(byte[,] data, int squareSize, Window mapWindow)
        {
            this.Data = data;
            this.MapWindow = mapWindow;
            this.SquareSize = squareSize;
            CheckIfMapWindowLarger();
            RoundMapWindowSize();
            CalculateChunkSize();
            ChunkNum.X = DataSize.X / ChunkSize.X;
            ChunkNum.Y = DataSize.Y / ChunkSize.Y;
            ChunkWithPlayer = new Vector2i(0, 0);
            ChunksOnScreen = new Vector2i(MapWindow.Size.Y / SquareSize / ChunkSize.X, MapWindow.Size.X / SquareSize / ChunkSize.Y);
            UpdateChunksOnScreen();

            this.EmptySquare = new Rectangle(0f, 0f, SquareSize, SquareSize) { FillColor = Color.Black, OutlineColor = OutLineColor, OutlineThickness = 1f };
            this.FullSquare = new Rectangle(0f, 0f, SquareSize, SquareSize) { FillColor = Color.White, OutlineColor = OutLineColor, OutlineThickness = 1f };
        }
        public Map(byte[,] data, int squareSize, Window mapWindow, Color emptySquareColor, Color fullSquareColor)
        {
            this.Data = data;
            this.MapWindow = mapWindow;
            this.SquareSize = squareSize;
            CheckIfMapWindowLarger();
            RoundMapWindowSize();

            this.EmptySquare = new Rectangle(0f, 0f, SquareSize, SquareSize) { FillColor = emptySquareColor, OutlineColor = OutLineColor, OutlineThickness = 1f };
            this.FullSquare = new Rectangle(0f, 0f, SquareSize, SquareSize) { FillColor = fullSquareColor, OutlineColor = OutLineColor, OutlineThickness = 1f };
        }
        public Map(byte[,] data, int squareSize, Vector2i mapWindowPosition)
        {
            this.Data = data;
            this.SquareSize = squareSize;
            this.MapWindow = new Window { Position = mapWindowPosition, Size = new Vector2i(DataSize.Y * SquareSize, DataSize.X * SquareSize) };

            this.EmptySquare = new Rectangle(0f, 0f, SquareSize, SquareSize) { FillColor = Color.Black, OutlineColor = OutLineColor, OutlineThickness = 1f };
            this.FullSquare = new Rectangle(0f, 0f, SquareSize, SquareSize) { FillColor = Color.White, OutlineColor = OutLineColor, OutlineThickness = 1f };
        }
        public Map(byte[,] data, int squareSize, Vector2i mapWindowPosition, Color emptySquareColor, Color fullSquareColor)
        {
            this.Data = data;
            this.SquareSize = squareSize;
            this.MapWindow = new Window { Position = mapWindowPosition, Size = new Vector2i(DataSize.Y * SquareSize, DataSize.X * SquareSize) };

            this.EmptySquare = new Rectangle(0f, 0f, SquareSize, SquareSize) { FillColor = emptySquareColor, OutlineColor = OutLineColor, OutlineThickness = 1f };
            this.FullSquare = new Rectangle(0f, 0f, SquareSize, SquareSize) { FillColor = fullSquareColor, OutlineColor = OutLineColor, OutlineThickness = 1f };
        }

        public void SquareFillWindow()
        {
            this.SquareSize = MapWindow.Size.X / DataSize.Y;
            this.EmptySquare.Width = this.SquareSize;
            this.EmptySquare.Height = this.SquareSize;
            this.FullSquare.Width = this.SquareSize;
            this.FullSquare.Height = this.SquareSize;
        }

        public void CheckMapBorder(Player player, RenderWindow window)
        {
            ChunkWithPlayerOnScreen.X = (int)player.Position.Y / (ChunkSize.X * SquareSize);
            ChunkWithPlayerOnScreen.Y = (int)player.Position.X / (ChunkSize.Y * SquareSize);

            if (ChunkWithPlayerOnScreen.Y > ChunksAroundMain[0] && ChunkWithPlayer.Y + ChunkWithPlayerOnScreen.Y < ChunkNum.Y - ChunksAroundMain[1] + 1)
            {
                player.PositionX -= ChunkSize.Y * SquareSize;
                ChunkWithPlayer.Y++;
            }
            if (ChunkWithPlayerOnScreen.X > ChunksAroundMain[2] && ChunkWithPlayer.X + ChunkWithPlayerOnScreen.X < ChunkNum.X - ChunksAroundMain[3] + 1)
            {
                player.PositionY -= ChunkSize.X * SquareSize;
                ChunkWithPlayer.X++;
            }
            if (ChunkWithPlayerOnScreen.Y < ChunksAroundMain[0] && ChunkWithPlayer.Y > 0)
            {
                player.PositionX += ChunkSize.Y * SquareSize;
                ChunkWithPlayer.Y--;
            }
            if (ChunkWithPlayerOnScreen.X < ChunksAroundMain[2] && ChunkWithPlayer.X > 0)
            {
                player.PositionY += ChunkSize.X * SquareSize;
                ChunkWithPlayer.X--;
            }
        }

        public void DrawChunks(RenderWindow window)
        {
            for (int Row = 0; Row < ChunkNum.X && Row < ChunksOnScreen.X; Row++)
            {
                for (int Column = 0; Column < ChunkNum.Y && Column < ChunksOnScreen.Y; Column++)
                {
                    ChunkRectangle.PositionX = (Column * ChunkSize.Y * SquareSize) + MapWindow.Position.X;
                    ChunkRectangle.PositionY = (Row * ChunkSize.X * SquareSize) + MapWindow.Position.Y;
                    ChunkRectangle.Width = ChunkSize.Y * SquareSize;
                    ChunkRectangle.Height = ChunkSize.X * SquareSize;
                    ChunkRectangle.Draw(window);
                }
            }
        }

        public void DrawMap(RenderWindow window)
        {
            RoundMapWindowSize();
            CheckIfMapWindowLarger();
            UpdateChunksOnScreen();

            SquareStarting.X = ChunkWithPlayer.X * ChunkSize.X;
            SquareStarting.Y = ChunkWithPlayer.Y * ChunkSize.Y;
            if (SquareStarting.X > DataSize.X - MapWindow.Size.Y / SquareSize)
            {
                SquareStarting.X = DataSize.X - MapWindow.Size.Y / SquareSize;
            }
            if (SquareStarting.X < 0)
            {
                SquareStarting.X = 0;
            }
            if (SquareStarting.Y > DataSize.Y - MapWindow.Size.X / SquareSize)
            {
                SquareStarting.Y = DataSize.Y - MapWindow.Size.X / SquareSize;
            }
            if (SquareStarting.Y < 0)
            {
                SquareStarting.Y = 0;
            }

            for (int Row = 0; Row < MapWindow.Size.Y / SquareSize; Row++)
            {
                for (int Column = 0; Column < MapWindow.Size.X / SquareSize; Column++)
                {
                    switch (Data[Row + SquareStarting.X, Column + SquareStarting.Y])
                    {
                        case 0:
                            EmptySquare.PositionX = Column * SquareSize + MapWindow.Position.X;
                            EmptySquare.PositionY = Row * SquareSize + MapWindow.Position.Y;
                            window.Draw(EmptySquare);
                            break;
                        case 1:
                            FullSquare.PositionX = Column * SquareSize + MapWindow.Position.X;
                            FullSquare.PositionY = Row * SquareSize + MapWindow.Position.Y;
                            window.Draw(FullSquare);
                            break;
                    }
                }
            }
        }

        #region Private Functions
        private void RoundMapWindowSize()
        {
            if (MapWindow.Size != (MapWindow.Size / SquareSize) * SquareSize)
            {
                MapWindow.Size = (MapWindow.Size / SquareSize) * SquareSize;
            }
        }

        /// <summary>
        /// Checks if the map window size is larger then the map data size and adjusts it
        /// </summary>
        private void CheckIfMapWindowLarger()
        {
            if (MapWindow.Size.X > SquareSize * DataSize.Y)
            {
                MapWindow.Size.X = SquareSize * DataSize.Y;
            }
            if (MapWindow.Size.Y > SquareSize * DataSize.X)
            {
                MapWindow.Size.Y = SquareSize * DataSize.X;
            }
        }

        private void UpdateChunksOnScreen()
        {
            ChunksOnScreen.X = MapWindow.Size.Y / SquareSize / ChunkSize.X;
            ChunksOnScreen.Y = MapWindow.Size.X / SquareSize / ChunkSize.Y;
            if (ChunksOnScreen.X % 2 == 1)
            {
                ChunksAroundMain[2] = ChunksOnScreen.X / 2;
                ChunksAroundMain[3] = ChunksAroundMain[2] + 1;
            }
            else
            {
                ChunksAroundMain[2] = ChunksOnScreen.X / 2;
                ChunksAroundMain[3] = ChunksAroundMain[2] - 1;
            }
            if (ChunksOnScreen.Y % 2 == 1)
            {
                ChunksAroundMain[0] = ChunksOnScreen.Y / 2;
                ChunksAroundMain[1] = ChunksAroundMain[0] + 1;
            }
            else
            {
                ChunksAroundMain[0] = ChunksOnScreen.Y / 2;
                ChunksAroundMain[1] = ChunksAroundMain[0] - 1;
            }
        }

        private void CalculateChunkSize()
        {
            ChunkSize = new Vector2i(-1, -1);
            for (int i = 3; i < 10; i++)
            {
                if (DataSize.X % i == 0)
                {
                    ChunkSize.X = i;
                    break;
                }
            }
            for (int i = 3; i < 10; i++)
            {
                if (DataSize.Y % 2 == 0)
                {
                    ChunkSize.Y = i;
                    break;
                }
            }
            if (ChunkSize.X == -1)
            {
                ChunkSize.X = 5;
            }
            if (ChunkSize.Y == -1)
            {
                ChunkSize.Y = 5;
            }
        }
        #endregion

        #region Public Helper Functions
        public static byte[,] GenerateMapWithWall(Vector2i size)
        {
            byte[,] map = new byte[size.X, size.Y];
            for (int Row = 0; Row < size.X; Row++)
            {
                for (int Column = 0; Column < size.Y; Column++)
                {
                    if (Row == 0 || Row == size.X - 1 || Column == 0 || Column == size.Y - 1)
                    {
                        map[Row, Column] = 1;
                    }
                    else
                    {
                        map[Row, Column] = 0;
                    }
                }
            }
            return map;
        }
        public static byte[,] GenerateMapWithWall(int sizeX, int sizeY)
        {
            byte[,] map = new byte[sizeX, sizeY];
            for (int Row = 0; Row < sizeX; Row++)
            {
                for (int Column = 0; Column < sizeY; Column++)
                {
                    if (Row == 0 || Row == sizeX - 1 || Column == 0 || Column == sizeY - 1)
                    {
                        map[Row, Column] = 1;
                    }
                    else
                    {
                        map[Row, Column] = 0;
                    }
                }
            }
            return map;
        }
        public static byte[,] GenerateMapWithWallRandom(int sizeX, int sizeY)
        {
            Random random = new Random();
            int randomNumber = 0;
            byte[,] map = new byte[sizeX, sizeY];
            for (int Row = 0; Row < sizeX; Row++)
            {
                for (int Column = 0; Column < sizeY; Column++)
                {
                    if (Row == 0 || Row == sizeX - 1 || Column == 0 || Column == sizeY - 1)
                    {
                        map[Row, Column] = 1;
                    }
                    else
                    {
                        randomNumber = random.Next(3);
                        if (randomNumber == 0 || randomNumber == 1)
                        {
                            map[Row, Column] = 0;
                        }
                        else
                        {
                            map[Row, Column] = 1;
                        }
                    }
                }
            }
            return map;
        }

        public int GetValueFromData(int position)
        {
            int rowCount = Data.GetLength(0);
            int colCount = Data.GetLength(1);

            if (position < 0 || position >= rowCount * colCount)
            {
                throw new IndexOutOfRangeException();
            }

            int row = position / colCount;
            int col = position % colCount;

            return Data[row, col];
        }
        #endregion
    }
    public struct Window
    {
        public Vector2i Position;
        public Vector2i Size;
    }
}

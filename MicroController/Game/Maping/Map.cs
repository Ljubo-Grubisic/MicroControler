  using MicroController.Game.Entities;
using MicroController.Shapes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Runtime.InteropServices;

namespace MicroController.Game.Maping
{
    public class Map
    {
        #region Public variables
        private byte[,] data;
        public Vector2i DataSize { get; private set; }
        public int SquareSize;

        public Window MinimapWindow;
        public Window MapWindow;

        public Color Image0FillColor = Color.White;
        public Color Image1FillColor = new Color(15, 15, 15, 255);
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
        private readonly Rectangle ChunkRectangle = new Rectangle(0f, 0f, 0f, 0f) { FillColor = Color.Transparent, OutlineColor = Color.Red, OutlineThickness = 2f };

        private Image Image0;
        private Image Image1;
        private Texture Texture;
        private Sprite Sprite;
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

            this.Image0 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image0FillColor);
            this.Image1 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image1FillColor);

            this.Texture = new Texture((uint)MapWindow.Size.X, (uint)MapWindow.Size.Y);
            this.Sprite = new Sprite(Texture);
        }
        public Map(byte[,] data, int squareSize, Vector2i mapWindowPosition)
        {
            this.Data = data;
            this.SquareSize = squareSize;
            this.MapWindow = new Window { Position = mapWindowPosition, Size = new Vector2i(DataSize.Y * SquareSize, DataSize.X * SquareSize) };
            CheckIfMapWindowLarger();
            RoundMapWindowSize();
            CalculateChunkSize();
            ChunkNum.X = DataSize.X / ChunkSize.X;
            ChunkNum.Y = DataSize.Y / ChunkSize.Y;
            ChunkWithPlayer = new Vector2i(0, 0);
            ChunksOnScreen = new Vector2i(MapWindow.Size.Y / SquareSize / ChunkSize.X, MapWindow.Size.X / SquareSize / ChunkSize.Y);
            UpdateChunksOnScreen();

            this.Image0 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image0FillColor);
            this.Image1 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image1FillColor);

            this.Texture = new Texture((uint)MapWindow.Size.X, (uint)MapWindow.Size.Y);
            this.Sprite = new Sprite(Texture);
        }

        public void SquareFillWindow()
        {
            this.SquareSize = MapWindow.Size.X / DataSize.Y;
        }

        public void ChunkSizeToWindow()
        {
            this.ChunkSize.X = MapWindow.Size.Y / SquareSize;
            this.ChunkSize.Y = MapWindow.Size.X / SquareSize;
        }

        /*
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
        }*/

        public void DrawChunks(RenderWindow window)
        {
            for (int Row = 0; Row < ChunkNum.X && Row < ChunksOnScreen.X; Row++)
            {
                for (int Column = 0; Column < ChunkNum.Y && Column < ChunksOnScreen.Y; Column++)
                {
                    ChunkRectangle.PositionX = (Column * ChunkSize.Y * SquareSize) + MapWindow.Position.X;
                    ChunkRectangle.PositionY = (Row * ChunkSize.X * SquareSize) + MapWindow.Position.Y;
                    ChunkRectangle.SizeX = ChunkSize.Y * SquareSize;
                    ChunkRectangle.SizeY = ChunkSize.X * SquareSize;
                    ChunkRectangle.Draw(window);
                }
            }
        }

        public void DrawMap(RenderWindow window)
        {
            RoundMapWindowSize();
            CheckIfMapWindowLarger();
            UpdateChunksOnScreen();
            UpdateTextureSize();
            UpdateSquareImages();

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

            UpdateMapTexture(MapWindow.Size, SquareStarting, SquareSize);
            Sprite.Position = (Vector2f)MapWindow.Position;
            window.Draw(Sprite, new RenderStates(Texture));
        }

        #region Private Functions
        #region Drawing - Image - Texture Functions
        private Vector2i OldWindowSize = new Vector2i(-1, -1);
        private Vector2i OldSquareStarting = new Vector2i(-1, -1);
        private int OldSquareSize = -1;
        private void UpdateMapTexture(Vector2i windowSize, Vector2i squareStarting, int squareSize)
        {
            if (OldWindowSize != windowSize || OldSquareStarting != squareStarting || OldSquareSize != squareSize)
            {
                for (int Row = 0; Row < MapWindow.Size.Y / SquareSize; Row++)
                {
                    for (int Column = 0; Column < MapWindow.Size.X / SquareSize; Column++)
                    {
                        uint xPos = (uint)(Column * SquareSize);
                        uint yPos = (uint)(Row * SquareSize);

                        switch (Data[Row + SquareStarting.X, Column + SquareStarting.Y])
                        {
                            case 0:
                                Sprite.Texture.Update(Image0, xPos, yPos);
                                break;
                            case 1:
                                Sprite.Texture.Update(Image1, xPos, yPos);
                                break;
                        }
                    }
                }
                OldWindowSize = windowSize;
                OldSquareStarting = squareStarting;
                OldSquareSize = squareSize;
            }
        }

        private void UpdateSquareImages()
        {
            if (this.Image0.Size.X != SquareSize || this.Image0.Size.Y != SquareSize)
            {
                this.Image0 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image0FillColor);
            }
            if (this.Image1.Size.X != SquareSize || this.Image1.Size.Y != SquareSize)
            {
                this.Image1 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image1FillColor);
            }
        }

        private void UpdateTextureSize()
        {
            if (Texture.Size != (Vector2u)MapWindow.Size)
            {
                this.Texture.Dispose();
                this.Texture = new Texture((uint)MapWindow.Size.X, (uint)MapWindow.Size.Y);
                this.Sprite = new Sprite(this.Texture);
            }
        }
        #endregion

        #region Window Functions
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

        private void RoundMapWindowSize()
        {
            if (MapWindow.Size != (MapWindow.Size / SquareSize) * SquareSize)
            {
                MapWindow.Size = (MapWindow.Size / SquareSize) * SquareSize;
            }
        }
        #endregion

        #region Chunk Functions
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

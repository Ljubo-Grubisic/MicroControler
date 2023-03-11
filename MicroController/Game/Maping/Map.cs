using MicroController.Game.Entities;
using SFML.Graphics;
using SFML.System;
using System;
using MicroController.InputOutput;
using MicroController.SFMLHelper;
using MicroController.GameLooping;

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

        public Vector2i SquareStarting;
        #endregion

        #region Private variables

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

            this.Image0 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image0FillColor);
            this.Image1 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image1FillColor);

            this.Texture = new Texture((uint)MapWindow.Size.X, (uint)MapWindow.Size.Y);
            this.Sprite = new Sprite(Texture);
        }

        public void SquareFillWindow()
        {
            this.SquareSize = MapWindow.Size.X / DataSize.Y;
        }

        public void Update(Entity entity, GameTime time)
        {
            RoundMapWindowSize();
            CheckIfMapWindowLarger();
            UpdateTextureSize();
            UpdateSquareImages();

            TrackEntityPosition(entity);
            KeyBoardInput(time);
            CheckSquareStarting();

            UpdateMapTexture(MapWindow.Size, SquareStarting, SquareSize);
        }

        public void DrawMap(RenderWindow window)
        {
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

        #region SquareStarting
        private void TrackEntityPosition(Entity entity)
        {
            // Checking the left border
            if (entity.DrawingPosition.X - MapWindow.Position.X < 0)
            {
                SquareStarting.Y -= (MapWindow.Size.X / SquareSize) / 2;
            }
            // Checking the right border
            if (entity.DrawingPosition.X - MapWindow.Position.X > MapWindow.Size.X)
            {
                SquareStarting.Y += (MapWindow.Size.X / SquareSize) / 2;
            }
            // Checking the top border
            if (entity.DrawingPosition.Y - MapWindow.Position.Y < 0)
            {
                SquareStarting.X -= (MapWindow.Size.Y / SquareSize) / 2;
            }
            // Checking the bottom border
            if (entity.DrawingPosition.Y - MapWindow.Position.Y > MapWindow.Size.Y)
            {
                SquareStarting.X += (MapWindow.Size.Y / SquareSize) / 2;
            }
        }
        private void CheckSquareStarting()
        {
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
        }
        private void KeyBoardInput(GameTime time)
        {
            if (KeyboardManager.OnKeyPress(SFML.Window.Keyboard.Key.Left, 1))
            {
                SquareStarting.Y--;
            }
            if (KeyboardManager.OnKeyPress(SFML.Window.Keyboard.Key.Right, 2))
            {
                SquareStarting.Y++;
            }
            if (KeyboardManager.OnKeyPress(SFML.Window.Keyboard.Key.Up, 3))
            {
                SquareStarting.X--;
            }
            if (KeyboardManager.OnKeyPress(SFML.Window.Keyboard.Key.Down, 4))
            {
                SquareStarting.X++;
            }


            if (KeyboardManager.OnKeyDownForTime(SFML.Window.Keyboard.Key.Left, time, 0, 1))
            {
                SquareStarting.Y--;
            }
            if (KeyboardManager.OnKeyDownForTime(SFML.Window.Keyboard.Key.Right, time, 1, 1))
            {
                SquareStarting.Y++;
            }
            if (KeyboardManager.OnKeyDownForTime(SFML.Window.Keyboard.Key.Up, time, 2, 1))
            {
                SquareStarting.X--;
            }
            if (KeyboardManager.OnKeyDownForTime(SFML.Window.Keyboard.Key.Down, time, 3, 1))
            {
                SquareStarting.X++;
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

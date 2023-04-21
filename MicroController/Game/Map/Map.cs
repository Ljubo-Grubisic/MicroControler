using microController.game.entities;
using SFML.Graphics;
using SFML.System;
using System;
using microController.helpers;
using microController.system;
using SFML.Window;

namespace microController.game
{
    public class Map
    {
        #region Public variables
        private byte[,] data;
        public Vector2i DataSize { get; private set; }
        public int SquareSize;
        public int OldSquareSize;

        public Window Window;

        public Color Image0FillColor = Color.White;
        public Color Image1FillColor = new Color(15, 15, 15, 255);
        public Color OutlineColor = Color.Black;

        public Vector2i SquareStarting;
        #endregion

        #region Private variables
        private Image Image0;
        private Image Image1;
        private Texture Texture;
        private Sprite Sprite;
        #endregion

        #region Getters and Setters
        private byte[,] Data
        {
            get { return data; }
            set
            {
                data = value;
                DataSize = new Vector2i(Data.GetLength(0), Data.GetLength(1));
            }
        }
        #endregion

        public Map(byte[,] data, int squareSize, Window window, Game game)
        {
            Data = data;
            Window = window;
            SquareSize = squareSize;
            CheckIfMapWindowLarger();
            RoundMapWindowSize();
            game.Window.MouseWheelMoved += OnMouseWheelScroll;

            Image0 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image0FillColor);
            Image1 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image1FillColor);

            Texture = new Texture((uint)Window.Size.X, (uint)Window.Size.Y);
            Sprite = new Sprite(Texture);
        }
        public Map(byte[,] data, int squareSize, Vector2i windowPosition, Game game)
        {
            Data = data;
            SquareSize = squareSize;
            Window = new Window { Position = windowPosition, Size = new Vector2i(DataSize.Y * SquareSize, DataSize.X * SquareSize) };
            CheckIfMapWindowLarger();
            RoundMapWindowSize();
            game.Window.MouseWheelMoved += OnMouseWheelScroll;

            Image0 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image0FillColor, OutlineColor);
            Image1 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image1FillColor, OutlineColor);

            Texture = new Texture((uint)Window.Size.X, (uint)Window.Size.Y);
            Sprite = new Sprite(Texture);
        }

        public void SquareFillWindow()
        {
            SquareSize = Window.Size.X / DataSize.Y;
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

            UpdateMapTexture(Window.Size, SquareStarting, SquareSize);
        }

        public void DrawMap(RenderWindow window)
        {
            Sprite.Position = (Vector2f)Window.Position;
            window.Draw(Sprite, new RenderStates(Texture));
        }

        #region Private Functions
        #region Drawing - Image - Texture Functions
        private Vector2i WindowSizeBuffer = new Vector2i(-1, -1);
        private Vector2i SquareStartingBuffer = new Vector2i(-1, -1);
        private int SquareSizeBuffer = -1;
        private void UpdateMapTexture(Vector2i windowSize, Vector2i squareStarting, int squareSize)
        {
            if (WindowSizeBuffer != windowSize || SquareStartingBuffer != squareStarting || SquareSizeBuffer != squareSize)
            {
                UpdateMapTextureForce();

                WindowSizeBuffer = windowSize;
                SquareStartingBuffer = squareStarting;
                SquareSizeBuffer = squareSize;
            }
        }
        public void UpdateMapTextureForce()
        {
            for (int Row = 0; Row < Window.Size.Y / SquareSize; Row++)
            {
                for (int Column = 0; Column < Window.Size.X / SquareSize; Column++)
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
        }

        private void UpdateSquareImages()
        {
            if (Image0.Size.X != SquareSize || Image0.Size.Y != SquareSize)
            {
                Image0 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image0FillColor, OutlineColor);
            }
            if (Image1.Size.X != SquareSize || Image1.Size.Y != SquareSize)
            {
                Image1 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image1FillColor, OutlineColor);
            }
        }
        public void UpdateSquareImagesForce()
        {
            Image0 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image0FillColor, OutlineColor);
            Image1 = ImageHelper.CreateImage(new Vector2u((uint)SquareSize, (uint)SquareSize), Image1FillColor, OutlineColor);
        }

        private void UpdateTextureSize()
        {
            if (Texture.Size != (Vector2u)Window.Size)
            {
                Texture.Dispose();
                Texture = new Texture((uint)Window.Size.X, (uint)Window.Size.Y);
                Sprite = new Sprite(Texture);
            }
        }
        #endregion

        #region Window Functions
        /// <summary>
        /// Checks if the map window size is larger then the map data size and adjusts it
        /// </summary>
        private void CheckIfMapWindowLarger()
        {
            if (Window.Size.X > SquareSize * DataSize.Y)
            {
                Window.Size.X = SquareSize * DataSize.Y;
            }
            if (Window.Size.Y > SquareSize * DataSize.X)
            {
                Window.Size.Y = SquareSize * DataSize.X;
            }
        }

        private void RoundMapWindowSize()
        {
            if (Window.Size != Window.Size / SquareSize * SquareSize)
            {
                Window.Size = Window.Size / SquareSize * SquareSize;
            }
        }
        #endregion

        #region SquareStarting
        private void TrackEntityPosition(Entity entity)
        {
            // Checking the left border
            if (entity.DrawingPosition.X - Window.Position.X < 0)
            {
                SquareStarting.Y -= Window.Size.X / SquareSize / 2;
            }
            // Checking the right border
            if (entity.DrawingPosition.X - Window.Position.X > Window.Size.X)
            {
                SquareStarting.Y += Window.Size.X / SquareSize / 2;
            }
            // Checking the top border
            if (entity.DrawingPosition.Y - Window.Position.Y < 0)
            {
                SquareStarting.X -= Window.Size.Y / SquareSize / 2;
            }
            // Checking the bottom border
            if (entity.DrawingPosition.Y - Window.Position.Y > Window.Size.Y)
            {
                SquareStarting.X += Window.Size.Y / SquareSize / 2;
            }
        }
        private void CheckSquareStarting()
        {
            if (SquareStarting.X > DataSize.X - Window.Size.Y / SquareSize)
            {
                SquareStarting.X = DataSize.X - Window.Size.Y / SquareSize;
            }
            if (SquareStarting.X < 0)
            {
                SquareStarting.X = 0;
            }
            if (SquareStarting.Y > DataSize.Y - Window.Size.X / SquareSize)
            {
                SquareStarting.Y = DataSize.Y - Window.Size.X / SquareSize;
            }
            if (SquareStarting.Y < 0)
            {
                SquareStarting.Y = 0;
            }
        }
        private void KeyBoardInput(GameTime time)
        {
            if (KeyboardManager.OnKeyPressed(Keyboard.Key.Left))
            {
                SquareStarting.Y--;
            }
            if (KeyboardManager.OnKeyPressed(Keyboard.Key.Right))
            {
                SquareStarting.Y++;
            }
            if (KeyboardManager.OnKeyPressed(Keyboard.Key.Up))
            {
                SquareStarting.X--;
            }
            if (KeyboardManager.OnKeyPressed(Keyboard.Key.Down))
            {
                SquareStarting.X++;
            }


            if (KeyboardManager.OnKeyDownForTime(Keyboard.Key.Left, 1))
            {
                SquareStarting.Y--;
            }
            if (KeyboardManager.OnKeyDownForTime(Keyboard.Key.Right, 1))
            {
                SquareStarting.Y++;
            }
            if (KeyboardManager.OnKeyDownForTime(Keyboard.Key.Up, 1))
            {
                SquareStarting.X--;
            }
            if (KeyboardManager.OnKeyDownForTime(Keyboard.Key.Down, 1))
            {
                SquareStarting.X++;
            }
        }
        #endregion

        #region MouseWheelScroll
        private void OnMouseWheelScroll(object sender, MouseWheelEventArgs e)
        {
            OldSquareSize = SquareSize;
            if (e.Delta == 1)
            {
                SquareSize += 1;
            }
            else
            {
                SquareSize -= 1;
            }
            if (SquareSize < 4)
            {
                SquareSize = 4;
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
        public int GetValueFromData(int row, int column)
        {
            if (row < 0 || column < 0 || row > DataSize.X || column > DataSize.Y)
            {
                throw new IndexOutOfRangeException();
            }

            return Data[row, column];
        }
        public void SetValueToData(int row, int column, byte value)
        {
            if (row < 0 || column < 0 || row > DataSize.X || column > DataSize.Y)
            {
                throw new IndexOutOfRangeException();
            }

            Data[row, column] = value;
        }
        #endregion
    }
    public struct Window
    {
        public Vector2i Position;
        public Vector2i Size;
    }
}

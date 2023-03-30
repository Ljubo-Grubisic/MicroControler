using MicroController.Game.Entities.Sensors;
using SFML.Graphics;
using SFML.System;
using System;
using System.Runtime.Versioning;

namespace MicroController.SFMLHelper
{
    public static class ImageHelper
    {
        private static readonly string ImgsPath = "Resources/Imgs/";
        private static readonly Color[] CameraBackgroundColors = new Color[]
        {
            new Color(228, 234, 236),
            new Color(215, 222, 226),
            new Color(233, 241, 244),
            new Color(209, 214, 219),
            new Color(142, 155, 163),
            new Color(149, 160, 166),
            new Color(152, 163, 170),
            new Color(185, 189, 193),
            new Color(197, 201, 203),
            new Color(166, 171, 174),
            new Color(176, 180, 183),
            new Color(179, 182, 184)
        };
        private static readonly Color[] DHT11BackgroundColors = new Color[]
        {
            new Color(252, 253, 255),
            new Color(223, 222, 224),
            new Color(223, 223, 221),
            new Color(202, 207, 211),
            new Color(223, 220, 221),
            new Color(221, 222, 220),
            new Color(222, 223, 223),
            new Color(222, 220, 221),
            new Color(220, 221, 224)
        };
        private static readonly Color[] FlameSensorBackgroundColors = new Color[]
        {
            new Color(252, 253, 255),
            new Color(223, 222, 224),
            new Color(223, 223, 221),
            new Color(202, 207, 211),
            new Color(223, 220, 221),
            new Color(221, 222, 220),
            new Color(222, 223, 223),
            new Color(222, 220, 221),
            new Color(220, 221, 224),
            new Color(182, 188, 169),
            new Color(223, 223, 224),
            new Color(212, 217, 220)
        };

        public static Texture LoadImgNoBackground(string filename)
        { 
            Image image = new Image(ImgsPath + filename);
            Color[] colorFilter;
            byte range = 255;
            switch (filename)
            {
                case "Camera.png":
                    colorFilter = CameraBackgroundColors;
                    range = 215;
                    break;
                case "HC-SR04.jpg":
                    colorFilter = null;
                    range = 254;
                    break;
                case "DHT11.png":
                    colorFilter = DHT11BackgroundColors;
                    range = 200;
                    break;
                case "FlameSensor.png":
                    colorFilter = FlameSensorBackgroundColors;
                    range = 200;
                    break;
                default:
                    colorFilter = null; 
                    break;
            }
            if (colorFilter != null)
            {
                foreach (Color color in colorFilter)
                {
                    image.CreateMaskFromColor(color);
                }
            }

            for (byte i = 255; i > range; i--)
            {
                image.CreateMaskFromColor(new Color(i, i, i));
            }

            Texture texture = new Texture(image);
            image.Dispose();

            return texture;
        }
        public static Image CreateImage(Vector2u imageSize, Color fillColor, Color outlineColor)
        {
            Color[,] pixelData2DColor = new Color[imageSize.X, imageSize.Y];
            for (int row = 0; row < imageSize.Y; row++)
            {
                for (int column = 0; column < imageSize.X; column++)
                {
                    if (row == 0 || row == imageSize.Y || column == 0 || column == imageSize.X)
                    {
                        pixelData2DColor[row, column] = outlineColor;
                    }
                    else
                    {
                        pixelData2DColor[row, column] = fillColor;
                    }
                }
            }

            return new Image(pixelData2DColor);
        }
        public static Image CreateImage(Vector2u imageSize, Color fillColor)
        {
            Color[,] pixelData2DColor = new Color[imageSize.X, imageSize.Y];
            for (int row = 0; row < imageSize.Y; row++)
            {
                for (int column = 0; column < imageSize.X; column++)
                {
                    if (row == 0 || row == imageSize.Y || column == 0 || column == imageSize.X)
                    {
                        pixelData2DColor[row, column] = Color.Black;
                    }
                    else
                    {
                        pixelData2DColor[row, column] = fillColor;
                    }
                }
            }

            return new Image(pixelData2DColor);
        }

        public static byte[] FlattenByteArray(byte[,] byteArray)
        {
            int height = byteArray.GetLength(0);
            int width = byteArray.GetLength(1);

            byte[] flattenedArray = new byte[height * width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    flattenedArray[i * width + j] = byteArray[i, j];
                }
            }

            return flattenedArray;
        }

        public static byte[,] UnflattenByteArray(byte[] flattenedArray, int height, int width)
        {
            byte[,] byteArray = new byte[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byteArray[i, j] = flattenedArray[i * width + j];
                }
            }

            return byteArray;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace MicroControler
{
    public static class ImageManager
    {
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
    }
}

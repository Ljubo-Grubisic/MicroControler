using microController.graphics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace microController.helpers
{
    static public class MathHelper
    { 
        public static Vector2f[] CalculateVerticiesEquilateral(float distance)
        {
            float x = 0;
            float y = 0;
            float surfaceArea = (distance * distance * (float)Math.Sin(DegreesToRadians(60))) / 2f;

            float height = (surfaceArea * 2) / distance;
            float Bx = x - (distance / 2);
            float By = y - height;
            float Cx = x + (distance / 2);
            float Cy = y - height;

            return new Vector2f[]
            {
                new Vector2f(x, -y),
                new Vector2f(Bx, -By),
                new Vector2f(Cx, -Cy)
            };
        }
        public static Vector2f[] CalculateVerticiesEquilateral(float x, float y, float distance)
        {
            float surfaceArea = (distance * distance * (float)Math.Sin(DegreesToRadians(60))) / 2f;
            float height = (surfaceArea * 2) / distance;
            float Bx = x - (distance / 2);
            float By = y - height;
            float Cx = x + (distance / 2);
            float Cy = y - height;

            return new Vector2f[]
            {
                new Vector2f(x, -y),
                new Vector2f(Bx, -By),
                new Vector2f(Cx, -Cy)
            };
        }

        public static Vector2f[] CalculateVerticiesIsosceles(float x, float y, float distanceIsceles, float distance)
        {
            float s = (distance + (2 * distanceIsceles)) / 2;
            float surfaceArea = (float)Math.Sqrt(s * (s - distance) * (2 * (s - distanceIsceles)));
            float height = (surfaceArea * 2) / distance;
            float Bx = x - (distance / 2);
            float By = y - height;
            float Cx = x + (distance / 2);
            float Cy = y - height;

            return new Vector2f[]
            {
                new Vector2f(x, -y),
                new Vector2f(Bx, -By),
                new Vector2f(Cx, -Cy)
            };
        }
        public static Vector2f[] CalculateVerticiesIsosceles(float distanceIsceles, float distance)
        {
            float x = 0;
            float y = 0;

            float s = (distance + (2 * distanceIsceles)) / 2;
            float surfaceArea = (float)Math.Sqrt(s * (s - distance) * (2 * (s - distanceIsceles)));
            float height = (surfaceArea * 2) / distance;
            float Bx = x - (distance / 2);
            float By = y - height;
            float Cx = x + (distance / 2);
            float Cy = y - height;

            return new Vector2f[]
            {
                new Vector2f(x, -y),
                new Vector2f(Bx, -By),
                new Vector2f(Cx, -Cy)
            };
        }

        public static float DegreesToRadians(float angle)
        {
            return (angle * (float)Math.PI) / 180;
        }
        public static float RadiansToDegrees(float angle)
        {
            return (angle * 180) / (float)Math.PI;
        }

        public static float Map(float inputMin, float inputMax, float outputMin, float outputMax, float value)
        {
            // Calculate the input range and output range
            float inputRange = inputMax - inputMin;
            float outputRange = outputMax - outputMin;

            // Calculate the normalized value of the input
            float normalizedValue = (value - inputMin) / inputRange;

            // Scale the normalized value to the output range
            float scaledValue = outputMin + (normalizedValue * outputRange);

            // Return the scaled value
            return scaledValue;
        }
        public static float Map(Vector2f inputScale, Vector2f outputScale, float value)
        {
            // Calculate the input range and output range
            float inputRange = inputScale.Y - inputScale.X;
            float outputRange = outputScale.Y - outputScale.X;

            // Calculate the normalized value of the input
            float normalizedValue = (value - inputScale.X) / inputRange;

            // Scale the normalized value to the output range
            float scaledValue = outputScale.X + (normalizedValue * outputRange);

            // Return the scaled value
            return scaledValue;
        }

        public static float[] Map(float inputMin, float inputMax, float outputMin, float outputMax, float[] value)
        {
            float[] exit = new float[value.Length];
            float inputScale = inputMax - inputMin;
            float outputScale = outputMax - outputMin;

            for (int i = 0; i < value.Length; i++)
            {
                exit[i] = value[i] / inputScale * outputScale;
            }

            return exit;
        }
        public static byte FloatToByte(float value)
        {
            return (byte)(value / 1.0 * 255);
        }

        public static Vector2f CenterRectangle(Vector2f position, Vector2f size)
        {
            return new Vector2f(position.X - (size.X / 2), position.Y - (size.Y / 2));
        }
        public static Vector2f CenterRectangle(float positionX, float positionY, Vector2f size)
        {
            return new Vector2f(positionX - (size.X / 2), positionY - (size.Y / 2));
        }
        public static Vector2f CenterRectangle(Vector2f position, float sizeX, float sizeY)
        {
            return new Vector2f(position.X - (sizeX / 2), position.Y - (sizeY / 2));
        }
        public static Vector2f CenterRectangle(float positionX, float positionY, float sizeX, float sizeY)
        {
            return new Vector2f(positionX - (sizeX / 2), positionY - (sizeY / 2));
        }

        public static Vector2f AddDeltaPosition(Vector2f position, Vector2f delta, float scale)
        {
            return new Vector2f(position.X + (delta.X * scale), position.Y + (delta.Y * scale));
        }

        public static float DistanceTriangleHypotenuse(Vector2f a, Vector2f b)
        {
            return (float)Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2));
        }
        public static float DistanceTriangleHypotenuse(float ax, float ay, Vector2f b)
        {
            return (float)Math.Sqrt(Math.Pow((b.X - ax), 2) + Math.Pow((b.Y - ay), 2));
        }
        public static float DistanceTriangleHypotenuse(Vector2f a, float bx, float by)
        {
            return (float)Math.Sqrt(Math.Pow((bx - a.X), 2) + Math.Pow((by - a.Y), 2));
        }
        public static float DistanceTriangleHypotenuse(float ax, float ay, float bx, float by)
        {
            return (float)Math.Sqrt(Math.Pow((bx - ax), 2) + Math.Pow((by - ay), 2));
        }

        private static Vector2f Buffer = new Vector2f();
        public static Vector2f CenterTextInRectangle(Vector2f rectanglePosition, Vector2f rectangleSize, FloatRect textRect)
        {
            Buffer.X = rectanglePosition.X + (rectangleSize.X / 2);
            Buffer.Y = rectanglePosition.Y + (rectangleSize.Y / 2);

            Buffer = CenterRectangle(Buffer, textRect.Width - textRect.Left, textRect.Height - textRect.Top) - new Vector2f(0f, textRect.Top);

            return Buffer;
        }

        public static Vector2f CenterRectangleInRectangle(Rectangle rectangle, Rectangle rectangleCentering)
        {
            Buffer.X = rectangle.Position.X + (rectangle.Size.X / 2);
            Buffer.Y = rectangle.Position.Y + (rectangle.Size.Y / 2);

            Buffer = CenterRectangle(Buffer, rectangleCentering.Size.X - rectangleCentering.Position.X, rectangleCentering.Size.Y - rectangleCentering.Position.Y);

            return Buffer;
        }

        public static bool IsMouseInRectangle(Vector2f Position, Vector2f Size, Vector2i mousePosition)
        {
            if (mousePosition.X > Position.X && mousePosition.X < Position.X + Size.X &&
                mousePosition.Y > Position.Y && mousePosition.Y < Position.Y + Size.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsMouseInRectangle(Rectangle rectangle, Vector2i mousePosition)
        {
            if (mousePosition.X > rectangle.Position.X && mousePosition.X < rectangle.Position.X + rectangle.Size.X &&
                mousePosition.Y > rectangle.Position.Y && mousePosition.Y < rectangle.Position.Y + rectangle.Size.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsMouseInCircle(Vector2f position, float radius, Vector2i mousePos)
        {
            double distance = Math.Sqrt(Math.Pow(((position.X + radius) - mousePos.X), 2) + Math.Pow(((position.Y + radius) - mousePos.Y), 2));
            return distance <= radius;
        }
        public static bool IsMouseInCircle(Circle circle, Vector2i mousePos)
        {
            double distance = Math.Sqrt(Math.Pow(((circle.Position.X + circle.Radius) - mousePos.X), 2) + Math.Pow(((circle.Position.Y + circle.Radius) - mousePos.Y), 2));
            return distance <= circle.Radius;
        }
    }
}

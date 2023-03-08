using SFML.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroControler.Mathematics
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
            float inputScale = inputMax - outputMin;
            float outputScale = outputMax - outputMin;

            return value / inputScale * outputScale;
        }
        public static float[] Map(float inputMin, float inputMax, float outputMin, float outputMax, float[] value)
        {
            float[] exit = new float[value.Length];
            float inputScale = inputMax - outputMin;
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
    }
}

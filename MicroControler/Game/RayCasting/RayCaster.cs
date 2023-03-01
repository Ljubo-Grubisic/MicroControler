using MicroControler.Mathematics;
using MicroControler.Shapes;
using SFML.Graphics;
using SFML.System;
using System;
using MicroControler.Game.Maping;

namespace MicroControler.Game.RayCasting
{
    public class RayCaster
    {
        #region Public variables
        /// <summary>
        /// Field of view
        /// </summary>
        private float fov;
        /// <summary>
        /// The spacing in degrees between rays
        /// </summary>
        private float angleSpacingRay;
        /// <summary>
        /// The thickness of the line in the 3D render
        /// </summary>
        private float rayThickness;
        /// <summary>
        /// How far the ray can go / how many check will a ray do
        /// </summary>
        public float DepthOffFeild;
        /// <summary>
        /// The position where the 3D window will show
        /// </summary>
        public Vector2i WindowPosition;
        /// <summary>
        /// The size of the 3D window
        /// </summary>
        private Vector2i windowSize;
        /// <summary>
        /// The Color of the ray in the map
        /// </summary>
        public Color RayMapColor;
        /// <summary>
        /// The Color of a strip created by a horizontal ray / the color of horizontal walls
        /// </summary>
        public Color HorizontalColor;
        /// <summary>
        /// The Color of a strip created by a vertical ray / the color of vertical walls
        /// </summary>
        public Color VerticalColor;
        /// <summary>
        /// Turns off or on the drawing off the map rays
        /// </summary>
        public bool DrawMapRays;

        /// <summary>
        /// The thickness of the line in the 3D render, value should not be changed
        /// </summary>
        public float RayThickness
        {
            get { return rayThickness; }
            set
            {
                rayThickness = value;
                Line3D.OutlineThickness = rayThickness;
            }
        }
        /// <summary>
        /// The size of the 3D window
        /// </summary>
        public Vector2i WindowSize
        {
            get { return windowSize; }
            set
            {
                windowSize = value;
                this.RayThickness = WindowSize.X / ((Fov / AngleSpacingRay) + 1f);
            }
        }
        /// <summary>
        /// Field of view
        /// </summary>
        public float Fov
        {
            get { return fov; }
            set
            {
                fov = value;
                this.RayThickness = WindowSize.X / ((Fov / AngleSpacingRay) + 1f);
            }
        }
        /// <summary>
        /// The spacing in degrees between rays
        /// </summary>
        public float AngleSpacingRay
        {
            get { return angleSpacingRay; }
            set
            {
                angleSpacingRay = value;
                this.RayThickness = WindowSize.X / ((Fov / AngleSpacingRay) + 1f);
            }
        }
        #endregion

        #region Private Object used for drawing and casting rays
        private Line LineMap;
        private Line Line3D;
        private sRay Ray;
        private sRay HorizontalRay;
        private sRay VerticalRay;
        #endregion

        #region Private variables used in the drawing function
        private float DepthOffFeildCounter;
        static private readonly double P2 = Math.PI / 2;
        static private readonly double P3 = 3 * Math.PI / 2;
        private float aTan;
        private float nTan;
        /// <summary>
        /// The position of the ray in the map
        /// </summary>
        private Vector2i MapPosition;
        /// <summary>
        /// The index of the square the ray is in  
        /// </summary>
        private int MapDataPosition;
        private float CameraAngle;
        private float LineHeight3D;
        private float LineOffset3D;
        #endregion

        public RayCaster(float fov, float angleSpacingRay, float depthOffFeild, Vector2i windowPosition, Vector2i windowSize, Color rayMapColor, Color horizontalColor, Color verticalColor, bool drawMapRays)
        {
            this.fov = fov;
            this.angleSpacingRay = angleSpacingRay;
            this.DepthOffFeild = depthOffFeild;
            this.WindowPosition = windowPosition;
            this.windowSize = windowSize;
            this.RayMapColor = rayMapColor;
            this.HorizontalColor = horizontalColor;
            this.VerticalColor = verticalColor;
            this.DrawMapRays = drawMapRays;

            this.rayThickness = WindowSize.X / ((Fov / AngleSpacingRay) + 1f);
            this.LineMap = new Line(new Vector2f(0, 0), new Vector2f(0, 0)) { OutlineColor = rayMapColor };
            this.Line3D = new Line(new Vector2f(0, 0), new Vector2f(0, 0)) { OutlineThickness = RayThickness };
            this.Ray = new sRay { Position = new Vector2f() };
            this.HorizontalRay = new sRay { };
            this.VerticalRay = new sRay { };
        }

        public void Draw(RenderWindow window, ref Map Map, RayCastEable rayCastingObject)
        {
            Ray.Angle = rayCastingObject.Rotation - MathHelper.DegreesToRadians(Fov / 2);
            if (Ray.Angle < 0)
            {
                Ray.Angle += 2 * (float)Math.PI;
            }
            else if (Ray.Angle > 2 * (float)Math.PI)
            {
                Ray.Angle -= 2 * (float)Math.PI;
            }
            for (int i = 0; i < Fov / AngleSpacingRay; i++)
            {
                // Horizontal ray check
                DepthOffFeildCounter = 0;
                HorizontalRay.Position = rayCastingObject.Position;
                HorizontalRay.Lenght = 1000000;

                aTan = (float)(-1 / Math.Tan(Ray.Angle));

                // Looking down
                if (Ray.Angle > Math.PI)
                {
                    Ray.Position.Y = (((int)rayCastingObject.Position.Y / Map.SquareSize) * Map.SquareSize) - 0.0001f;
                    Ray.Position.X = (rayCastingObject.Position.Y - Ray.Position.Y) * aTan + rayCastingObject.Position.X;
                    Ray.PositionOffset.Y = -Map.SquareSize;
                    Ray.PositionOffset.X = -Ray.PositionOffset.Y * aTan;
                }
                // Looking up
                if (Ray.Angle < Math.PI)
                {
                    Ray.Position.Y = (((int)rayCastingObject.Position.Y / Map.SquareSize) * Map.SquareSize) + Map.SquareSize;
                    Ray.Position.X = (rayCastingObject.Position.Y - Ray.Position.Y) * aTan + rayCastingObject.Position.X;
                    Ray.PositionOffset.Y = Map.SquareSize;
                    Ray.PositionOffset.X = -Ray.PositionOffset.Y * aTan;
                }
                // Looking straight left or right
                if (Ray.Angle == 0 || Ray.Angle == Math.PI)
                {
                    Ray.Position = rayCastingObject.Position;
                    DepthOffFeildCounter = DepthOffFeild;
                }
                while (DepthOffFeildCounter < DepthOffFeild)
                {
                    MapPosition.X = (int)(Ray.Position.X) / Map.SquareSize;
                    MapPosition.Y = (int)(Ray.Position.Y) / Map.SquareSize;
                    MapDataPosition = MapPosition.Y * Map.DataSize.Y + MapPosition.X;  // ???? Map.Size.X or Map.Size.Y Check

                    if (MapDataPosition > 0 && MapDataPosition < Map.DataSize.Y * Map.DataSize.X && Map.GetValueFromData(MapDataPosition) == 1)
                    {
                        HorizontalRay.Position = Ray.Position;
                        HorizontalRay.Lenght = MathHelper.DistanceTriangleHypotenuse(rayCastingObject.Position, HorizontalRay.Position);
                        DepthOffFeildCounter = DepthOffFeild;
                    }
                    else
                    {
                        Ray.Position += Ray.PositionOffset;
                        DepthOffFeildCounter++;
                    }
                }

                // Vertical ray check
                DepthOffFeildCounter = 0;
                VerticalRay.Position = rayCastingObject.Position;
                VerticalRay.Lenght = 1000000;
                nTan = (float)(-Math.Tan(Ray.Angle));
                // Looking left
                if (Ray.Angle > P2 && Ray.Angle < P3)
                {
                    Ray.Position.X = (int)rayCastingObject.Position.X / Map.SquareSize * Map.SquareSize - 0.0001f;
                    Ray.Position.Y = (rayCastingObject.Position.X - Ray.Position.X) * nTan + rayCastingObject.Position.Y;
                    Ray.PositionOffset.X = -Map.SquareSize;
                    Ray.PositionOffset.Y = -Ray.PositionOffset.X * nTan;
                }
                // Looking right
                if (Ray.Angle < P2 || Ray.Angle > P3)
                {
                    Ray.Position.X = (int)rayCastingObject.Position.X / Map.SquareSize * Map.SquareSize + Map.SquareSize;
                    Ray.Position.Y = (rayCastingObject.Position.X - Ray.Position.X) * nTan + rayCastingObject.Position.Y;
                    Ray.PositionOffset.X = Map.SquareSize;
                    Ray.PositionOffset.Y = -Ray.PositionOffset.X * nTan;
                }
                // Looking up or down 
                if (Ray.Angle == 0 || Ray.Angle == Math.PI)
                {
                    Ray.Position = rayCastingObject.Position;
                    DepthOffFeildCounter = DepthOffFeild;
                }
                while (DepthOffFeildCounter < DepthOffFeild)
                {
                    MapPosition.X = (int)Ray.Position.X / Map.SquareSize;
                    MapPosition.Y = (int)Ray.Position.Y / Map.SquareSize;
                    MapDataPosition = MapPosition.Y * Map.DataSize.Y + MapPosition.X;

                    if (MapDataPosition > 0 && MapDataPosition < Map.DataSize.Y * Map.DataSize.X && Map.GetValueFromData(MapDataPosition) == 1)
                    {
                        VerticalRay.Position = Ray.Position;
                        VerticalRay.Lenght = MathHelper.DistanceTriangleHypotenuse(rayCastingObject.Position, VerticalRay.Position);
                        DepthOffFeildCounter = DepthOffFeild;
                    }
                    else
                    {
                        Ray.Position += Ray.PositionOffset;
                        DepthOffFeildCounter++;
                    }
                }

                if (VerticalRay.Lenght < HorizontalRay.Lenght)
                {
                    Ray.Position = VerticalRay.Position;
                    Ray.Lenght = VerticalRay.Lenght;
                    Line3D.OutlineColor = VerticalColor;
                }
                if (VerticalRay.Lenght > HorizontalRay.Lenght)
                {
                    Ray.Position = HorizontalRay.Position;
                    Ray.Lenght = HorizontalRay.Lenght;
                    Line3D.OutlineColor = HorizontalColor;
                }
                if (DrawMapRays)
                {
                    LineMap.Position0 = rayCastingObject.Position + (Vector2f)Map.MapWindow.Position;
                    LineMap.Position1 = Ray.Position + (Vector2f)Map.MapWindow.Position;
                    LineMap.Draw(window);
                }

                // Draw 3D rays
                CameraAngle = rayCastingObject.Rotation - Ray.Angle;
                if (CameraAngle < 0)
                {
                    CameraAngle += 2 * (float)Math.PI;
                }
                if (CameraAngle > 2 * (float)Math.PI)
                {
                    CameraAngle -= 2 * (float)Math.PI;
                }
                Ray.Lenght = Ray.Lenght * (float)Math.Cos(CameraAngle);
                LineHeight3D = (Map.SquareSize * WindowSize.Y) / Ray.Lenght;
                if (LineHeight3D > WindowSize.Y)
                {
                    LineHeight3D = WindowSize.Y;
                }
                LineOffset3D = (WindowPosition.Y + WindowSize.Y / 2) - LineHeight3D / 2;
                Line3D.Position0X = i * RayThickness + WindowPosition.X + RayThickness;
                Line3D.Position0Y = LineOffset3D + WindowPosition.Y;
                Line3D.Position1X = i * RayThickness + WindowPosition.X + RayThickness;
                Line3D.Position1Y = LineHeight3D + LineOffset3D;
                Line3D.Draw(window);


                Ray.Angle += MathHelper.DegreesToRadians(AngleSpacingRay);
                if (Ray.Angle < 0)
                {
                    Ray.Angle += 2 * (float)Math.PI;
                }
                if (Ray.Angle > 2 * (float)Math.PI)
                {
                    Ray.Angle -= 2 * (float)Math.PI;
                }
            }
        }

        private struct sRay
        {
            public Vector2f Position;
            /// <summary>
            /// The offset of the ray that when added gets the next Line
            /// </summary>
            public Vector2f PositionOffset;
            public float Angle;
            public float Lenght;
        }
    }
}

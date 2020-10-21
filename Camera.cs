using System;
using VectorAndPolygonMath;

namespace VisibilityPolygon
{
    public class Camera
    {
        private Vector2D visBorder1;
        private Vector2D visBorder2;
        public float HalfViewAngleRadians { get; set; }
        public float VisionDistance { get; set; }
        public float ROfBody { get; set; }
        public Vector2D Location { get; set; }
        public Vector2D Direction { get; set; }
        public Vector2D VisBorder1 { get => visBorder1; }
        public Vector2D VisBorder2 { get => visBorder2; }

        public Camera()
        {
        }
        public Camera(Vector2D location, float visionDistance, float radians, float rOfBody)
        {
            VisionDistance = visionDistance;
            Location = location;
            HalfViewAngleRadians = radians / 2;
            ROfBody = rOfBody;
        }
        public void UpdateTo(Vector2D direction, Vector2D loc)
        {
            Location = loc;
            Direction = (direction - Location).Normalize();
            float alf = (float)Math.Asin(Direction.Y);
            if (Direction.X > 0)
            {
                visBorder1 = new Vector2D((float)Math.Cos(alf + HalfViewAngleRadians), (float)Math.Sin(alf + HalfViewAngleRadians));
                visBorder2 = new Vector2D((float)Math.Cos(alf - HalfViewAngleRadians), (float)Math.Sin(alf - HalfViewAngleRadians));
            }
            else
            {
                visBorder1 = new Vector2D(-(float)Math.Cos(alf - HalfViewAngleRadians), (float)Math.Sin(alf - HalfViewAngleRadians));
                visBorder2 = new Vector2D(-(float)Math.Cos(alf + HalfViewAngleRadians), (float)Math.Sin(alf + HalfViewAngleRadians));
            }
        }
    }
}

using System;
using VectorAndPolygonMath;

namespace VisibilityPolygon
{
    class Cam
    {
        public float HalfViewAngleRadians { get; private set; }
        public float VisionDistance { get; private set; }
        public float ROfBody { get; private set; }
        public Vector2D Location { get; private set; }
        public Vector2D Direction { get; private set; }
        public Vector2D VisBorder1 { get; private set; }
        public Vector2D VisBorder2 { get; private set; }
        public Cam(Vector2D location, float visionDistance, float radians, float rOfBody)
        {
            VisionDistance = visionDistance;
            Location = location;
            HalfViewAngleRadians = radians / 2;
            ROfBody = rOfBody;
        }
        public void UpdateTo(Vector2D direction, Vector2D loc)
        {
            Location = loc;
            Direction = (direction - Location).Normalise();
            float alf = (float)Math.Asin(Direction.Y);
            if (Direction.X > 0)
            {
                VisBorder1 = new Vector2D((float)Math.Cos(alf + HalfViewAngleRadians), (float)Math.Sin(alf + HalfViewAngleRadians));
                VisBorder2 = new Vector2D((float)Math.Cos(alf - HalfViewAngleRadians), (float)Math.Sin(alf - HalfViewAngleRadians));
            }
            else
            {
                VisBorder1 = new Vector2D(-(float)Math.Cos(alf - HalfViewAngleRadians), (float)Math.Sin(alf - HalfViewAngleRadians));
                VisBorder2 = new Vector2D(-(float)Math.Cos(alf + HalfViewAngleRadians), (float)Math.Sin(alf + HalfViewAngleRadians));
            }
        }
    }
}

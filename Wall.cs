using VectorAndPolygonMath;

namespace VisibilityPolygon
{
    public class Wall
    {
        public bool visible;
        public Vector2D V1;
        public Vector2D V2;
        public Wall(Vector2D v1, Vector2D v2)
        {
            V1 = v1;
            V2 = v2;
            visible = false;
        }
        public Wall()
        {
            visible = false;
        }
    }
}

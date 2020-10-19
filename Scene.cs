using System.Collections.Generic;
using VectorAndPolygonMath;

namespace VisibilityPolygon
{
    class Scene
    {
        public List<Wall> Walls;
        public List<Wall> WallsInVisZone;
        public List<Wall> VisWalls;
        public Cam Camera;
        public Scene(List<Wall> walls, Cam cam)
        {
            Walls = walls;
            Camera = cam;
        }

        public void Update(Vector2D vector, Vector2D move)
        {
            if (Colis(move))
            {
                Camera.UpdateTo(vector, move);
            }
            FindWallsInVisZone();
            FindVisWalls();
        }

        void FindWallsInVisZone()
        {
            WallsInVisZone = new List<Wall>();
            for (int i = 0; i < Walls.Count; i++)
            {
                if (Geometry.LineSide(Camera.Location, new Vector2D(Camera.Direction.Y, -Camera.Direction.X) + Camera.Location, Walls[i].V1) > 0 || Geometry.LineSide(Camera.Location, new Vector2D(Camera.Direction.Y, -Camera.Direction.X) + Camera.Location, Walls[i].V2) > 0)
                {
                    if ((Geometry.LineSide(Camera.Location, Camera.VisBorder2 + Camera.Location, Walls[i].V1) > 0 ||
                        Geometry.LineSide(Camera.Location, Camera.VisBorder2 + Camera.Location, Walls[i].V2) > 0)
                        &&
                        (Geometry.LineSide(Camera.Location, Camera.VisBorder1 + Camera.Location, Walls[i].V1) < 0 ||
                        Geometry.LineSide(Camera.Location, Camera.VisBorder1 + Camera.Location, Walls[i].V2) < 0))
                    {
                        WallsInVisZone.Add(Walls[i]);
                    }
                }
            }
        }
        void FindVisWalls()
        {
            VisWalls = new List<Wall>();
            Wall visWall;
            GetNearestWallAndLengthRayCast(Camera.Location, Camera.VisBorder2 + Camera.Location, out visWall);
            if (visWall != null)
            {
                VisWalls.Add(visWall);
            }
            GetNearestWallAndLengthRayCast(Camera.Location, Camera.VisBorder1 + Camera.Location, out visWall);
            if (visWall != null)
            {
                VisWalls.Add(visWall);
            }
            for (int i = 0; i < WallsInVisZone.Count; i++)
            {
                if (WallPointIn(WallsInVisZone[i].V1) && !VisWalls.Contains(WallsInVisZone[i]))
                {
                    VisWalls.Add(WallsInVisZone[i]);
                }
                if (WallPointIn(WallsInVisZone[i].V2) && !VisWalls.Contains(WallsInVisZone[i]))
                {
                    VisWalls.Add(WallsInVisZone[i]);
                }
            }
        }

        private bool WallPointIn(Vector2D point)
        {
            if (Geometry.LineSide(Camera.Location, Camera.VisBorder2 + Camera.Location, point) > 0 &&
            Geometry.LineSide(Camera.Location, Camera.VisBorder1 + Camera.Location, point) < 0)
            {
                float SqrMinLength = GetNearestWallAndLengthRayCast(Camera.Location, point, out Wall visWall);
                if (visWall != null)
                {
                    if (!VisWalls.Contains(visWall))
                        VisWalls.Add(visWall);

                    if (SqrMinLength > (point - Camera.Location).SqrLength)
                        return true;
                }
                else
                    return true;
            }
            return false;
        }

        bool Colis(Vector2D move)
        {
            foreach (var wall in Walls)
            {
                if ((Geometry.MinDistansePointLineSigment(wall.V1, wall.V2, move) - move).SqrLength < Camera.ROfBody * Camera.ROfBody)
                {
                    return false;
                }
            }
            return true;
        }
        float GetNearestWallAndLengthRayCast(Vector2D a, Vector2D b, out Wall NearestHitedWall)  //From a to b
        {
            float sqrMinLength = float.MaxValue;
            Vector2D intersecPoint;
            NearestHitedWall = null;
            foreach (var wall in WallsInVisZone)
            {
                if (b == wall.V1 || b == wall.V2)
                {
                    if ((b.X != wall.V1.X || b.Y != wall.V1.Y) && (b.X != wall.V2.X || b.Y != wall.V2.Y))
                    {
                        int era = 0;
                    }
                    continue;
                }
                if (Geometry.IntersectionOfLineAndLineSigment(a, b, wall.V1, wall.V2, out intersecPoint) && (b - a).X * (intersecPoint - a).X >= 0 && (b - a).Y * (intersecPoint - a).Y >= 0)
                {
                    if ((intersecPoint - Camera.Location).SqrLength < sqrMinLength)
                    {
                        NearestHitedWall = wall;
                        sqrMinLength = (intersecPoint - Camera.Location).SqrLength;
                    }
                }
            }
            return sqrMinLength;
        }
    }
}

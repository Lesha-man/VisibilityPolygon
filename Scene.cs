using System;
using System.Collections.Generic;
using VectorAndPolygonMath;

namespace VisibilityPolygon
{
    [Serializable]
    public class Scene
    {
        public Camera MainCamera;
        public List<Wall> VisWalls;
        public List<Wall> Walls;
        public List<Wall> WallsInVisZone;
        public Scene(List<Wall> walls, Camera cam)
        {
            Walls = walls;
            MainCamera = cam;
        }
        public Scene()
        {
        }

        public void Update(Vector2D vector, Vector2D move)
        {
            if (Colis(move))
            {
                MainCamera.UpdateTo(vector, move);
            }
            FindWallsInVisZone();
            FindVisWalls();
        }

        bool Colis(Vector2D move)
        {
            foreach (var wall in Walls)
            {
                if ((Geometry.MinDistansePointLineSigment(wall.V1, wall.V2, move) - move).SqrLength < MainCamera.ROfBody * MainCamera.ROfBody)
                {
                    return false;
                }
            }
            return true;
        }

        void FindVisWalls()
        {
            VisWalls = new List<Wall>();
            Wall visWall;
            GetNearestWallAndLengthRayCast(MainCamera.Location, MainCamera.VisBorder2 + MainCamera.Location, out visWall);
            if (visWall != null)
            {
                VisWalls.Add(visWall);
            }
            GetNearestWallAndLengthRayCast(MainCamera.Location, MainCamera.VisBorder1 + MainCamera.Location, out visWall);
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

        void FindWallsInVisZone()
        {
            WallsInVisZone = new List<Wall>();
            for (int i = 0; i < Walls.Count; i++)
            {
                if (Geometry.LineSide(MainCamera.Location, new Vector2D(MainCamera.Direction.Y, -MainCamera.Direction.X) + MainCamera.Location, Walls[i].V1) > 0 || Geometry.LineSide(MainCamera.Location, new Vector2D(MainCamera.Direction.Y, -MainCamera.Direction.X) + MainCamera.Location, Walls[i].V2) > 0)
                {
                    if ((Geometry.LineSide(MainCamera.Location, MainCamera.VisBorder2 + MainCamera.Location, Walls[i].V1) > 0 ||
                        Geometry.LineSide(MainCamera.Location, MainCamera.VisBorder2 + MainCamera.Location, Walls[i].V2) > 0)
                        &&
                        (Geometry.LineSide(MainCamera.Location, MainCamera.VisBorder1 + MainCamera.Location, Walls[i].V1) < 0 ||
                        Geometry.LineSide(MainCamera.Location, MainCamera.VisBorder1 + MainCamera.Location, Walls[i].V2) < 0))
                    {
                        WallsInVisZone.Add(Walls[i]);
                    }
                }
            }
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
                    continue;
                }
                if (Geometry.IntersectionOfLineAndLineSigment(a, b, wall.V1, wall.V2, out intersecPoint) && (b - a).X * (intersecPoint - a).X >= 0 && (b - a).Y * (intersecPoint - a).Y >= 0)
                {
                    if ((intersecPoint - MainCamera.Location).SqrLength < sqrMinLength)
                    {
                        NearestHitedWall = wall;
                        sqrMinLength = (intersecPoint - MainCamera.Location).SqrLength;
                    }
                }
            }
            return sqrMinLength;
        }

        private bool WallPointIn(Vector2D point)
        {
            if (Geometry.LineSide(MainCamera.Location, MainCamera.VisBorder2 + MainCamera.Location, point) > 0 &&
            Geometry.LineSide(MainCamera.Location, MainCamera.VisBorder1 + MainCamera.Location, point) < 0)
            {
                float SqrMinLength = GetNearestWallAndLengthRayCast(MainCamera.Location, point, out Wall visWall);
                if (visWall != null)
                {
                    if (!VisWalls.Contains(visWall))
                        VisWalls.Add(visWall);

                    if (SqrMinLength > (point - MainCamera.Location).SqrLength)
                        return true;
                }
                else
                    return true;
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VectorAndPolygonMath;

namespace VisibilityPolygon
{
    class Drawer
    {
        PictureBox pictureBox;
        Graphics g;
        Bitmap bmp;
        Scene scene;
        public Drawer(PictureBox pBox, Scene sc)
        {
            pictureBox = pBox;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            g = Graphics.FromImage(bmp);
            scene = sc;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        public void DrawAllLightOff()
        {
            g.Clear(Color.Black);
            DrawCam();
            foreach (var wall in scene.VisWalls)
            {
                Vector2D LV1 = (wall.V1 - scene.Camera.Location).Normalise() * scene.Camera.VisionDistance * 2 + scene.Camera.Location;
                Vector2D LV2 = (wall.V2 - scene.Camera.Location).Normalise() * scene.Camera.VisionDistance * 2 + scene.Camera.Location;
                Vector2D pointToWall = Geometry.MinDistansePointLineSigment(wall.V1, wall.V2, scene.Camera.Location);
                if (pointToWall != wall.V1 && pointToWall != wall.V2)
                {
                    Vector2D LV3 = (pointToWall - scene.Camera.Location).Normalise() * 1000 + pointToWall;
                    g.FillPolygon(Brushes.Black, new PointF[] { new PointF(wall.V1.X, wall.V1.Y), new PointF(wall.V2.X, wall.V2.Y), new PointF(LV2.X, LV2.Y), new PointF(LV3.X, LV3.Y), new PointF(LV1.X, LV1.Y) });
                }
                else
                {
                    g.FillPolygon(Brushes.Black, new PointF[] { new PointF(wall.V1.X, wall.V1.Y), new PointF(wall.V2.X, wall.V2.Y), new PointF(LV2.X, LV2.Y), new PointF(LV1.X, LV1.Y) });
                }
            }
            pictureBox.Image = bmp;
        }
        public void DrawAllLightOn()
        {
            g.Clear(Color.White);
            DrawCam();
            foreach (var wall in scene.VisWalls)
            {
                Vector2D LV1 = (wall.V1 - scene.Camera.Location).Normalise() * scene.Camera.VisionDistance * 2 + scene.Camera.Location;
                Vector2D LV2 = (wall.V2 - scene.Camera.Location).Normalise() * scene.Camera.VisionDistance * 2 + scene.Camera.Location;
                Vector2D pointToWall = Geometry.MinDistansePointLineSigment(wall.V1, wall.V2, scene.Camera.Location);
                if (pointToWall != wall.V1 && pointToWall != wall.V2)
                {
                    Vector2D LV3 = (pointToWall - scene.Camera.Location).Normalise() * scene.Camera.VisionDistance * 2 + scene.Camera.Location;
                    g.FillPolygon(Brushes.White, new PointF[] { new PointF(wall.V1.X, wall.V1.Y), new PointF(wall.V2.X, wall.V2.Y), new PointF(LV2.X, LV2.Y), new PointF(LV3.X, LV3.Y), new PointF(LV1.X, LV1.Y) });
                }
                else
                {
                    g.FillPolygon(Brushes.White, new PointF[] { new PointF(wall.V1.X, wall.V1.Y), new PointF(wall.V2.X, wall.V2.Y), new PointF(LV2.X, LV2.Y), new PointF(LV1.X, LV1.Y) });
                }
            }
            foreach (var wall in scene.Walls)
            {
                DrawWall(wall, Brushes.Black);
            }
            foreach (var wall in scene.WallsInVisZone)
            {
                DrawWall(wall, Brushes.Red);
            }
            foreach (var wall in scene.VisWalls)
            {
                DrawWall(wall, Brushes.Green);
            }
            pictureBox.Image = bmp;
        }

        public void DrawWall(Wall wall, Brush brush)
        {
            g.DrawLine(new Pen(brush, 4), new PointF(wall.V1.X, wall.V1.Y), new PointF(wall.V2.X, wall.V2.Y));
        }

        public void DrawCam()
        {
            g.DrawEllipse(new Pen(Brushes.Blue, 1), scene.Camera.Location.X - scene.Camera.ROfBody, scene.Camera.Location.Y - scene.Camera.ROfBody, scene.Camera.ROfBody + scene.Camera.ROfBody, scene.Camera.ROfBody + scene.Camera.ROfBody);
            float alf = (float)Math.Asin(scene.Camera.Direction.Y);
            if (scene.Camera.Direction.X > 0)
                g.FillPie(Brushes.Yellow, scene.Camera.Location.X - scene.Camera.VisionDistance, scene.Camera.Location.Y - scene.Camera.VisionDistance, scene.Camera.VisionDistance + scene.Camera.VisionDistance, scene.Camera.VisionDistance + scene.Camera.VisionDistance, (alf - scene.Camera.HalfViewAngleRadians) * 57.3f, scene.Camera.HalfViewAngleRadians * 114.6f);
            else
                g.FillPie(Brushes.Yellow, scene.Camera.Location.X - scene.Camera.VisionDistance, scene.Camera.Location.Y - scene.Camera.VisionDistance, scene.Camera.VisionDistance + scene.Camera.VisionDistance, scene.Camera.VisionDistance + scene.Camera.VisionDistance, (3.1416f - alf + scene.Camera.HalfViewAngleRadians) * 57.3f, -scene.Camera.HalfViewAngleRadians * 114.6f);
        }

        public void DrawPolygon(Brush brush, List<float> coordMas)
        {
            if (coordMas.Count % 2 == 1)
            {
                throw new Exception("Ban");
            }
            PointF[] points = new PointF[coordMas.Count / 2];
            g.DrawPolygon(new Pen(brush, 4), points);
        }
    }
}

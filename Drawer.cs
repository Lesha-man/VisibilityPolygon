using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VectorAndPolygonMath;

namespace VisibilityPolygon
{
    class Drawer
    {
        private const float DegsPerRadians = 57.3f;
        private const float PI = (float)Math.PI;
        private Bitmap bmp;
        private Graphics g;
        private PictureBox pictureBox;
        public Drawer(PictureBox pBox)
        {
            pictureBox = pBox;
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        public void DrawAllLightOff(Scene scene)
        {
            g.Clear(Color.Black);
            DrowUnvisualRegions(scene, Color.Black);
            pictureBox.Image = bmp;
        }

        public void DrawAllLightOn(Scene scene)
        {
            g.Clear(Color.White);
            DrowUnvisualRegions(scene, Color.White);
            foreach (var wall in scene.Walls)
            {
                DrawWallBlack(wall);
            }
            foreach (var wall in scene.WallsInVisZone)
            {
                DrawWallRed(wall);
            }
            foreach (var wall in scene.VisWalls)
            {
                DrawWallGreen(wall);
            }
            pictureBox.Image = bmp;
        }

        public void DrawCam(Camera camera)
        {
            g.DrawEllipse(new Pen(Brushes.Blue, 1), camera.Location.X - camera.ROfBody, camera.Location.Y - camera.ROfBody, camera.ROfBody + camera.ROfBody, camera.ROfBody + camera.ROfBody);
            float alf = (float)Math.Asin(camera.Direction.Y);
            float startAngle;
            float endAngle;
            if (camera.Direction.X < 0)
            {
                startAngle = (PI - alf + camera.HalfViewAngleRadians) * DegsPerRadians;
                endAngle = -camera.HalfViewAngleRadians * 2.0f * DegsPerRadians;
            }
            else
            {
                startAngle = (alf - camera.HalfViewAngleRadians) * DegsPerRadians;
                endAngle = camera.HalfViewAngleRadians * 2.0f * DegsPerRadians;
            }
            g.FillPie(Brushes.Yellow, camera.Location.X - camera.VisionDistance, camera.Location.Y - camera.VisionDistance, camera.VisionDistance + camera.VisionDistance, camera.VisionDistance + camera.VisionDistance, startAngle, endAngle);
        }

        public void DrawWallGreen(Wall wall)
        {
            g.DrawLine(new Pen(Brushes.LightGreen, 4), new PointF(wall.V1.X, wall.V1.Y), new PointF(wall.V2.X, wall.V2.Y));
        }
        public void DrawWallRed(Wall wall)
        {
            g.DrawLine(new Pen(Brushes.Red, 4), new PointF(wall.V1.X, wall.V1.Y), new PointF(wall.V2.X, wall.V2.Y));
        }
        public void DrawWallBlack(Wall wall)
        {
            g.DrawLine(new Pen(Brushes.Black, 4), new PointF(wall.V1.X, wall.V1.Y), new PointF(wall.V2.X, wall.V2.Y));
        }

        private void DrowUnvisualRegions(Scene scene, Color color)
        {
            DrawCam(scene.MainCamera);
            foreach (var wall in scene.VisWalls)
            {
                Vector2D LV1 = (wall.V1 - scene.MainCamera.Location).Normalize(scene.MainCamera.VisionDistance * 2) + scene.MainCamera.Location;
                Vector2D LV2 = (wall.V2 - scene.MainCamera.Location).Normalize(scene.MainCamera.VisionDistance * 2) + scene.MainCamera.Location;
                Vector2D pointToWall = Geometry.MinDistansePointLineSigment(wall.V1, wall.V2, scene.MainCamera.Location);
                Vector2D LV3 = LV2;
                if (pointToWall != wall.V1 && pointToWall != wall.V2)
                {
                    LV3 = (pointToWall - scene.MainCamera.Location).Normalize() * scene.MainCamera.VisionDistance * 2 + pointToWall;
                }
                g.FillPolygon(
                       new SolidBrush(color),
                       new PointF[]
                       {
                            new PointF(wall.V1.X, wall.V1.Y),
                            new PointF(wall.V2.X, wall.V2.Y),
                            new PointF(LV2.X, LV2.Y),
                            new PointF(LV3.X, LV3.Y),
                            new PointF(LV1.X, LV1.Y)
                       });
            }
        }
    }
}
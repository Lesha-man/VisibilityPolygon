using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VectorAndPolygonMath;

namespace VisibilityPolygon
{
    public partial class Form1 : Form
    {
        Scene scene;
        Drawer drawer;
        Keys key;
        Dictionary<Keys, Action> KeysActivitis  = new Dictionary<Keys, Action>();
        public Form1()
        {
            InitializeComponent();
            KeysActivitis.Add(Keys.W, () => SceneUpd(new Vector2D(scene.Camera.Location.X, scene.Camera.Location.Y - 4)));
            KeysActivitis.Add(Keys.A, () => SceneUpd(new Vector2D(scene.Camera.Location.X - 4, scene.Camera.Location.Y))); 
            KeysActivitis.Add(Keys.S, () => SceneUpd(new Vector2D(scene.Camera.Location.X, scene.Camera.Location.Y + 4)));
            KeysActivitis.Add(Keys.D, () => SceneUpd(new Vector2D(scene.Camera.Location.X + 4, scene.Camera.Location.Y)));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<Wall> walls = new List<Wall>();
            walls.Add(new Wall(new Vector2D(10, 10), new Vector2D(100, 100)));
            walls.Add(new Wall(new Vector2D(1000, 1000), new Vector2D(2000, 1000)));
            walls.Add(new Wall(new Vector2D(1000, 900), new Vector2D(1100, 900)));
            walls.Add(new Wall(new Vector2D(110, 10), new Vector2D(200, 100)));
            walls.Add(new Wall(new Vector2D(10, 210), new Vector2D(100, 300)));
            walls.Add(new Wall(new Vector2D(490, 490), new Vector2D(700, 700)));
            walls.Add(new Wall(new Vector2D(670, 700), new Vector2D(800, 800)));
            walls.Add(new Wall(new Vector2D(900, 900), new Vector2D(800, 770)));
            walls.Add(new Wall(new Vector2D(1500, 500), new Vector2D(1700, 700)));
            walls.Add(new Wall(new Vector2D(1500, 400), new Vector2D(1700, 400)));
            walls.Add(new Wall(new Vector2D(500, 0), new Vector2D(0, 500)));
            walls.Add(new Wall(new Vector2D(500, 100), new Vector2D(100, 500)));
            //walls.Add(new Wall(new Vector2D(700, 250), new Vector2D(600, 350)));
            scene = new Scene(walls, new Cam(new Vector2D(pictureBox1.Width / 2, pictureBox1.Height / 2), 1000, 1f, 30));
            drawer = new Drawer(pictureBox1, scene);
        }
        private void TickUpdate()
        {
            if (KeysActivitis.ContainsKey(key))
            {
                KeysActivitis[key]();
            }
            else
                SceneUpd(scene.Camera.Location);
            key = 0;
            if (checkBox1.Checked)
            {
                drawer.DrawAllLightOn();

            }
            else
            {
                drawer.DrawAllLightOff();
            }
        }
        private void SceneUpd(Vector2D locOffset)
        {
            scene.Update(new Vector2D(Cursor.Position.X, Cursor.Position.Y), locOffset);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TickUpdate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            TickUpdate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            key = e.KeyCode;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = !checkBox2.Checked;
        }
    }
}

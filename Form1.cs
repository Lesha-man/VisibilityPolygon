﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using VectorAndPolygonMath;

namespace VisibilityPolygon
{
    public partial class Form1 : Form
    {
        private Wall tempWall;
        private readonly Dictionary<Keys, Action> KeysActivitis;
        private Drawer drawer;
        private Keys key;
        private Point oldCursorPos;
        private Scene scene;
        public Form1()
        {
            InitializeComponent();
            KeysActivitis = new Dictionary<Keys, Action>
            {
                { Keys.W, () => SceneUpd(new Vector2D(scene.MainCamera.Location.X, scene.MainCamera.Location.Y - 4)) },
                { Keys.A, () => SceneUpd(new Vector2D(scene.MainCamera.Location.X - 4, scene.MainCamera.Location.Y)) },
                { Keys.S, () => SceneUpd(new Vector2D(scene.MainCamera.Location.X, scene.MainCamera.Location.Y + 4)) },
                { Keys.D, () => SceneUpd(new Vector2D(scene.MainCamera.Location.X + 4, scene.MainCamera.Location.Y)) },
            };
            CursorPosChenget += myMouseMove;
            KeyDown += myKeyDown;
        }

        event EventHandler CursorPosChenget;

        public Point OldCursorPos
        {
            get => oldCursorPos;
            set
            {
                CursorPosChenget?.Invoke(this, null);
                oldCursorPos = value;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            TickUpdate();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = !checkBox2.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Scene));
            using (FileStream file = new FileStream("data.xml", FileMode.Create))
            {
                xml.Serialize(file, scene);
            }
            drawer = new Drawer(pictureBox1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Scene));
            using (FileStream file = new FileStream("data.xml", FileMode.Open))
            {
                scene = xml.Deserialize(file) as Scene;
            }
            drawer = new Drawer(pictureBox1);
        }

        private void myKeyDown(object sender, EventArgs e)
        {
            key = ((KeyEventArgs)e).KeyCode;
            TickUpdate();
        }

        private void myMouseMove(object sender, EventArgs e)
        {
            TickUpdate();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OldCursorPos = OldCursorPos != Cursor.Position ? Cursor.Position : OldCursorPos;
        }

        private void SceneUpd(Vector2D locOffset)
        {
            scene.Update(new Vector2D(Cursor.Position.X, Cursor.Position.Y), locOffset);
        }

        private void TickUpdate()
        {
            if (KeysActivitis.ContainsKey(key))
            {
                KeysActivitis[key]();
            }
            else
            {
                SceneUpd(scene.MainCamera.Location);
            }
            if (checkBox1.Checked)
                drawer.DrawAllLightOn(scene);
            else
                drawer.DrawAllLightOff(scene);
            key = 0;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (OldCursorPos != Cursor.Position)
            {
                OldCursorPos = Cursor.Position;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            tempWall.V2 = new Vector2D(Cursor.Position.X, Cursor.Position.Y);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Vector2D clickDown = new Vector2D(Cursor.Position.X, Cursor.Position.Y);
            tempWall = new Wall(clickDown, clickDown);
            scene.Walls.Add(tempWall);
            timer2.Enabled = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            timer2.Enabled = false;
        }
    }
}

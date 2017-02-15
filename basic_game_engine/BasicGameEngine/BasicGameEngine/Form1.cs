using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicGameEngine
{
  
    public partial class gameForm : Form
    {

        [DllImport("kernel32.dll")]
        private static extern long GetTickCount();

        private System.Drawing.Bitmap screenBuffer;
        private System.Drawing.Bitmap[] gameGraphics;

        private System.Drawing.PointF screenCenter;
        private System.Drawing.PointF cameraPos;
        private System.Drawing.PointF shipPos;
        private System.Drawing.PointF shipVel;

        private float gameTime = 0.0f;
        private float timePool = 0.0f;
        private float timeStep = 1.0f / 60.0f;
        private long lastTickCount = GetTickCount();
        private long curTickCount = 0;
        private float timeDelta = 0.0f;
        
        private bool gameRunning = true;

        private bool upPressed = false;
        private bool downPressed = false;
        private bool leftPressed = false;
        private bool rightPressed = false;
        
        private Graphics graphicsObject;

        public gameForm()
        {
            InitializeComponent();
            
        }

        private void gameForm_Load(object sender, EventArgs e)
        {
            graphicsObject = this.CreateGraphics();
            gameGraphics = new System.Drawing.Bitmap[1];
            gameGraphics[0] = (Bitmap) Image.FromFile(@"Images\ship.png");

            screenBuffer = Image.FromHbitmap(new Bitmap(this.Width, this.Height).GetHbitmap());

            shipPos = new System.Drawing.PointF(0.0f, 0.0f);
            shipVel = new System.Drawing.PointF(0.0f, 0.0f);
            cameraPos = new System.Drawing.PointF(0.0f, 0.0f);
            screenCenter = new System.Drawing.PointF(this.Width * 0.5f, this.Height * 0.5f);

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                           ControlStyles.UserPaint |
                           ControlStyles.DoubleBuffer,
                           true);
        }

        public void gameLoop()
        {
            while (gameRunning)
            {

                curTickCount = GetTickCount();
                timeDelta = (curTickCount - lastTickCount) * 0.001f;
                lastTickCount = curTickCount;

                timePool += timeDelta;

                while (timePool >= timeStep)
                {

                    gameTime += timeStep;
                    timePool -= timeStep;

                    gameUpdate();
                    cameraPos = shipPos;
                    gameRender(Graphics.FromImage(screenBuffer));

                }


                graphicsObject.DrawImage(screenBuffer, new Point(0,0));

                Application.DoEvents();


            }
        }

        private void gameUpdate()
        {

            if (upPressed) shipVel.Y -= 30 * timeStep;
            if (downPressed) shipVel.Y += 30 * timeStep;
            if (leftPressed) shipVel.X -= 30 * timeStep;
            if (rightPressed) shipVel.X += 30 * timeStep;
            
            shipPos.X += (shipVel.X * timeStep);
            shipPos.Y += (shipVel.Y * timeStep);
            
        }

        private void gameRender(Graphics g)
        {
            
            g.Clear(Color.Black);

            System.Drawing.PointF drawPos = new PointF();

            drawPos.X = screenCenter.X + cameraPos.X - (shipPos.X + (gameGraphics[0].Width * 0.5f));
            drawPos.Y = screenCenter.Y + cameraPos.Y - (shipPos.Y + (gameGraphics[0].Height * 0.5f));

            g.DrawImage(gameGraphics[0], drawPos);


        }

        private void gameForm_KeyDown(object sender, KeyEventArgs e)
        {

            System.Diagnostics.Debug.Write(e.KeyCode);

            if (e.KeyCode == Keys.Up)
            {
                upPressed = true;
            }

            if (e.KeyCode == Keys.Down)
            {
                downPressed = true;
            }

            if (e.KeyCode == Keys.Left)
            {
                leftPressed = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                rightPressed = true;
            }
        }

        private void gameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                upPressed = false;
            }

            if (e.KeyCode == Keys.Down)
            {
                downPressed = false;
            }

            if (e.KeyCode == Keys.Left)
            {
                leftPressed = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                rightPressed = false;
            }
        }

        private void gameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            gameRunning = false;
        }

        private void gameForm_Paint(object sender, PaintEventArgs e)
        {
            
            //gameRender(e.Graphics);
            //System.Diagnostics.Debug.Print("painting");
        }
    }
}

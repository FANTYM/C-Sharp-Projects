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

        private Bitmap screenBuffer;
        private Dictionary <long, gameObject> gameObjects;
        private Physics gamePhysics;

        private PointF screenCenter;
        private PointF cameraPos;

        private gameObject playerShip;
        
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

        private long nextObjID = 0;

        private System.Random random = new System.Random();
        
        private Graphics graphicsObject;

        public gameForm()
        {
            InitializeComponent();
            
        }

        private void gameForm_Load(object sender, EventArgs e)
        {

            screenBuffer = Image.FromHbitmap(new Bitmap(this.Width, this.Height).GetHbitmap());

            cameraPos = new System.Drawing.PointF(0.0f, 0.0f);
            screenCenter = new System.Drawing.PointF(this.Width * 0.5f, this.Height * 0.5f);
            
            graphicsObject = this.CreateGraphics();

            gameObjects = new Dictionary<long, gameObject>();

            gamePhysics = new Physics(ref gameObjects);

            playerShip = createObject("player", @"Images\ship.png", new PointF(0.0f, 0.0f), new PointF(0.0f, 0.0f)) ;
            
            for (int i = 0; i < 100; i++)
            {
                int rndRoid = random.Next(1, 4);
                createObject("asteroid" + i, @"Images\asteroid" + rndRoid + ".png", new PointF(random.Next(-500, 500), random.Next(-500, 500)), new PointF(0.0f, 0.0f));
            }

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

                    gameInput();
                    gameUpdate();
                    cameraPos = playerShip.pos;
                    gameRender(Graphics.FromImage(screenBuffer));

                }


                graphicsObject.DrawImage(screenBuffer, new Point(0,0));

                Application.DoEvents();


            }
        }

        private void gameInput()
        {

            if (upPressed) playerShip.AddVelocity(0, 10);
            if (downPressed) playerShip.AddVelocity(0, -10);
            if (leftPressed) playerShip.AddVelocity(10, 0);
            if (rightPressed) playerShip.AddVelocity(-10, 0);

        }

        private void gameUpdate()
        {
            gamePhysics.runPhysics(timeStep);
            
        }

        private void gameRender(Graphics g)
        {
            
            g.Clear(Color.Black);

            System.Drawing.PointF drawPos = new PointF();
            
            foreach (KeyValuePair<long, gameObject> curPair in gameObjects)
            {

                drawPos.X = screenCenter.X + cameraPos.X - (curPair.Value.pos.X + (curPair.Value.Width * 0.5f));
                drawPos.Y = screenCenter.Y + cameraPos.Y - (curPair.Value.pos.Y + (curPair.Value.Height * 0.5f));

                g.DrawImage(curPair.Value.objBitmap, drawPos);

            }


        }

        private gameObject createObject(string objName, string objImageFileName, PointF objPos, PointF objVel)
        {
            long objID = nextObjID;

            nextObjID++;

            gameObject newObject = new gameObject(objID, objName, objImageFileName, objPos, objVel);

            gameObjects.Add(objID, newObject);

            return newObject;

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

    }
}

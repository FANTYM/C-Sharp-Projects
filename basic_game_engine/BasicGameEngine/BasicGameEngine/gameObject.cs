using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BasicGameEngine
{

    
    class gameObject
    {
        private PointF position = new PointF(0.0f,0.0f);
        public PointF pos
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        private PointF velocity = new PointF(0.0f, 0.0f);
        public PointF vel
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        private Bitmap bitmap;
        public Bitmap objBitmap
        {
            get
            {
                return bitmap;
            }
        }

        private long id = -1;
        public long ID
        {
            get
            {
                return id;
            }
        }

        private string name = "";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private float width = 0;
        public float Width
        {
            get
            {
                return width;
            }
            
        }
        private float height = 0;
        public float Height
        {
            get
            {
                return height;
            }

        }

        private PointF min;
        public PointF Min
        {
            get
            {
                min.X = position.X - (width * 0.5f);
                min.Y = position.Y - (height * 0.5f);

                return min;
            }
        }

        private PointF max;
        public PointF Max
        {
            get
            {
                max.X = position.X + (width * 0.5f);
                max.Y = position.Y + (height * 0.5f);

                return max;
            }
        }

        public gameObject(long objID, string objName, string objImageFileName, PointF objPos, PointF objVel)
        {

            this.id = objID;
            this.Name = objName;
            this.bitmap = (Bitmap)Image.FromFile(objImageFileName);
            this.width = this.bitmap.Width;
            this.height = this.bitmap.Height;
            this.position = objPos;
            this.velocity = objVel;

        }

        public void AddVelocity(float x, float y)
        {
            velocity.X = velocity.X + x;
            velocity.Y = velocity.Y + y;
        }

        public void AddVelocity(PointF addVel)
        {
            velocity.X = velocity.X + addVel.X;
            velocity.Y = velocity.Y + addVel.Y;
        }

        public void integrateVelocity(float timeStep)
        {
            position.X += velocity.X * timeStep;
            position.Y += velocity.Y * timeStep;
        }
    }
}

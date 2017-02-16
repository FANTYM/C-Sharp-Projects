using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BasicGameEngine
{
    class Physics
    {

        private PointF cellSize = new PointF(128, 128);
        public PointF CellSize
        {
            get
            {
                return cellSize;
            }
            set
            {
                cellSize = value;
            }
        }

        private Dictionary<long, gameObject> gameObjs;
        private Dictionary<string, ArrayList> gameCells = new Dictionary<string, ArrayList>();

        public Physics(ref Dictionary<long, gameObject> gameObjects)
        {
            gameObjs = gameObjects;
        }

        public void runPhysics(float timeStep)
        {

            gameCells.Clear();
            
            foreach (KeyValuePair<long, gameObject> curPair in gameObjs)
            {
                curPair.Value.integrateVelocity(timeStep);
                placeInCells(curPair.Value);
                checkForCollision(curPair.Value);

            }
        }


        private void checkForCollision(gameObject toCheck)
        {

            ArrayList curCells = getObjectCells(toCheck);


        }

        private void placeInCells(gameObject curObj)
        {

            ArrayList curCells = getObjectCells(curObj);
            ArrayList thisCell;

            foreach (Point curCell in curCells) {

                string cellIndex = (curCell.X + "." + curCell.Y);

                if (!gameCells.ContainsKey(cellIndex))
                {
                    gameCells.Add(cellIndex, new ArrayList());
                }

                gameCells.TryGetValue(cellIndex, out thisCell);

                thisCell.Add(curObj.ID);
            }
            
        }

        private ArrayList getObjectCells(gameObject curObj)
        {

            ArrayList retPoints = new ArrayList();

            Point curPoint = new Point();

            // Center
            curPoint.X = (int)Math.Floor(curObj.pos.X / cellSize.X);
            curPoint.Y = (int)Math.Floor(curObj.pos.Y / cellSize.Y);
            if (!retPoints.Contains(curPoint)) retPoints.Add(curPoint);

            // Upper Left
            curPoint.X = (int) Math.Floor(curObj.Min.X / cellSize.X);
            curPoint.Y = (int) Math.Floor(curObj.Min.Y / cellSize.Y);
            if (!retPoints.Contains(curPoint)) retPoints.Add(curPoint);

            // Upper Right
            curPoint.X = (int)Math.Floor(curObj.Max.X / cellSize.X);
            curPoint.Y = (int)Math.Floor(curObj.Min.Y / cellSize.Y);
            if (!retPoints.Contains(curPoint)) retPoints.Add(curPoint);

            // Lower Right
            curPoint.X = (int)Math.Floor(curObj.Max.X / cellSize.X);
            curPoint.Y = (int)Math.Floor(curObj.Max.Y / cellSize.Y);
            if (!retPoints.Contains(curPoint)) retPoints.Add(curPoint);

            // Lower Right
            curPoint.X = (int)Math.Floor(curObj.Min.X / cellSize.X);
            curPoint.Y = (int)Math.Floor(curObj.Max.Y / cellSize.Y);
            if (!retPoints.Contains(curPoint)) retPoints.Add(curPoint);

            return retPoints;
        }



    }
}

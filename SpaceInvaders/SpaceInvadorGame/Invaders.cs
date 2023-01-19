using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    /*
       * Invader class keeps an eye on a single invader
       */
    public class Invaders
    {
      
        public Invaders()
        {
        }
        // determines how much the invader moves right or left.
        private const int HorizontalInterval = 10;
        //determines how much it moves downwards
        private const int verticalInterval = 40;
        public Rectangle Area{ get; set; }
       
        //Enum that determines what kind of ship it is.
        public enum Type { Bug, Saucer, Satelite, SpaceShip, Star };

        private Bitmap img; 

        public Point Location { get;  set; }

        public Type InvaderType { get;  set; }

        //gets size  of the image and location. enable the method Rectangle contains() method.
        // inside LINQ query to detect shots that were collided.



        
        public void Move(Direction direction)
        {
            /*
             tells the invaders which way to move.
             Game keeps up with the invaders location.
             Game also sees if it is time for the invaders to fire a shot and if it is, it will add it a shot.
             */
            switch (direction)
            {
                case Direction.LEFT: Location = new Point(movementX -= HorizontalInterval, Location.Y); break;
                case Direction.RIGHT: Location = new Point(movementX += HorizontalInterval, Location.Y); break;
                case Direction.DOWN: Location = new Point(Location.X , movementY += verticalInterval); break;
            }

            if (movementY > 600)
            {
                Game.InvaderPassedTheBoundaryGameOver = true;
            }
        }

        public static int Score { get;  set; }
        public int movementX;
        public int movementY;
        public int addedScore;
        public Invaders(Type invaderType, Point location, int score)
        {
            this.InvaderType = invaderType;
            this.Location = location;
            this.addedScore = score;
            movementX = Location.X;
            movementY = Location.Y;
        }

        public void Draw(Graphics g, int animationCell)
        {
            Bitmap[] bitmapArray=new Bitmap[3];  
            //This method needs to draw the ship, by using the correct drawing cell.
            switch (InvaderType)
            {
                case Type.Bug: bitmapArray = new Bitmap[] { new Bitmap(Properties.Resources.bug1), new Bitmap(Properties.Resources.bug2), new Bitmap(Properties.Resources.bug3), new Bitmap(Properties.Resources.bug4) }; break;
                case Type.Satelite: bitmapArray = new Bitmap[] { new Bitmap(Properties.Resources.satellite1), new Bitmap(Properties.Resources.satellite2), new Bitmap(Properties.Resources.satellite3), new Bitmap(Properties.Resources.satellite4) }; break;
                case Type.Saucer: bitmapArray = new Bitmap[] { new Bitmap(Properties.Resources.watchit1), new Bitmap(Properties.Resources.watchit2), new Bitmap(Properties.Resources.watchit3), new Bitmap(Properties.Resources.watchit4) }; break;
                case Type.SpaceShip: bitmapArray = new Bitmap[] { new Bitmap(Properties.Resources.spaceship1), new Bitmap(Properties.Resources.spaceship2), new Bitmap(Properties.Resources.spaceship3), new Bitmap(Properties.Resources.spaceship4) }; break;
                case Type.Star: bitmapArray = new Bitmap[] { new Bitmap(Properties.Resources.star1), new Bitmap(Properties.Resources.star2), new Bitmap(Properties.Resources.star3), new Bitmap(Properties.Resources.star4) }; break;
            }
            //img = bitmapArray[animationCell];
            g.DrawImage(img = bitmapArray[animationCell], Location.X, Location.Y);
            Area = new Rectangle(new Point(movementX, movementY), img.Size);
           
        }
 


        //private Bitmap InvaderImage()
        //{
        
        //    //returns the right bitmap for a specific cell.
         


        //    return new Bitmap();
        //}
    }
}

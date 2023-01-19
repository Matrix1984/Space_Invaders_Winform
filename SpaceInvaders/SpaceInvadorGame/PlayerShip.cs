using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    public class PlayerShip
    {
        public static Bitmap SHIP = Properties.Resources.player;
        public Point Location;
        public static int move=10;
        public Rectangle Area { set { } get { return new Rectangle(new Point(move, 600), Properties.Resources.player.Size); } }
        public static bool IsLaser { get; set; }
        
        public void Draw(Graphics g)
        {

       
           g.DrawImage(SHIP, move, 600);  
        }



        public void Move(Direction d)
        {
           
            switch (d)
            {
                case Direction.LEFT:  move -= 10; break;
                case Direction.RIGHT: move += 10; break; 
            }
            Area = new Rectangle(new Point(move, 600), SHIP.Size);  
        }
    }
}

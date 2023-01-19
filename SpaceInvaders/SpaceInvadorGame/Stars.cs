using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    /*
     * Twinkle has 300 stars. 5 stars appear and disappear each time the twinkle() is called.
     */ 
    public class Stars
    {

   
        public Stars()
        {

        }

          int randomStarY;
          int randomStarX;
            //method removes 5 stars at random and adds 5 new ones.
            // the method is called from the game object
            //the method is called several times a second , to make the stars twinkle.
            public Stars(Rectangle r, Random random)
            {
                 randomStarY = random.Next(r.Y);
                 randomStarX = random.Next(r.X);
            }

            

  
          
            public void Draw(Graphics g, int animationCell)
            {
                //should draw all stars
                Random r = new Random();
                int randomColor=r.Next(5);//randomStarX,randomStarY ,3,3
               
                switch (randomColor)
                {
                    case 1:  g.DrawEllipse(Pens.Blue,  new Rectangle(randomStarX,randomStarY ,3,3)); break;
                    case 2:  g.DrawEllipse(Pens.Red,   new Rectangle(randomStarX,randomStarY ,3,3)); break;
                    case 3:  g.DrawEllipse(Pens.Purple,new Rectangle(randomStarX,randomStarY ,3,3)); break;
                    case 4:  g.DrawEllipse(Pens.Yellow,new Rectangle(randomStarX,randomStarY ,3,3)); break;
                    case 5:  g.DrawEllipse(Pens.Green, new Rectangle(randomStarX,randomStarY, 3, 3)); break;
                }
  
            }

            // would set the stars with a random color.
    
       
    }


}

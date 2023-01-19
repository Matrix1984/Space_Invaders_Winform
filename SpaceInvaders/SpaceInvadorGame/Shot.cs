using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    /*
     * Need to make sure that Draw() takes Graphics object 
     * and draws a yellow rectangle. Then Move() should move
     * the shot up or down and return true if the shot is within
     * the game boundaries.
     */ 
    public class Shot
    {

        public Shot()
        {
        }
        
        private const int moveInterval=80;
        private const int width = 5;
        private const int height = 15;
        private const int MOVE_AT_FASTER_INTERVAL = 120;
        //The shot updates its location in the Move()
        public Point Location { get; set; }
        private Direction direction;
        private Rectangle boundaries;
        int moveShotUp ;
        int moveShotDown;
       
       
        public Rectangle Area { get; set; }
        public WEAPON Weapon;
        public bool IsWeaponOn { get; set; }
        private Bitmap playersShotImage;
        public static int[] FourRandomInvaderLocations { get; set; }
        const int SPEED_MOVEMENT_HOMINGMISSILE = 40;
        public int shotLocationInvader { get; set; }
        public int  shotLocationPlayer { get; set; }
        public Shot(Point location, Direction direction, Rectangle boundaries, WEAPON weaponRecieved, bool isWeaponArmed )
        {
            // the game passes those parameters. so the methods will find out when a shot is off screen
            this.boundaries = boundaries;
            this.direction = direction;
            this.Location = location;
            shotLocationInvader = Location.Y+10;
            Weapon = weaponRecieved;
            IsWeaponOn = isWeaponArmed;
            shotLocationPlayer = 600;
        }




        public void Draw(Graphics g, int animationCell)
        {         
            /*
             * Handles drawing a little triangle.
             * Game will call this method each time, the screen needs to be updated.
             */
            if (IsWeaponOn)
            {
            

                switch (Weapon)
                {
                    case WEAPON.GUIDINGLASER: GuidingLaser(g); break;
                    case WEAPON.LASER: Laser(g); break;
                    case WEAPON.MULTIPLESHOTS: MultipleShots(g); break;
                    case WEAPON.MULTIROCKETS: MultiRocketShots(g); break;
                    case WEAPON.ROCKET: rocket(g); break;
                    default: IsWeaponOn = false; break;

                }

            }

            if(!IsWeaponOn)
            {
                switch (direction)
                {
                    case Direction.UP: g.DrawRectangle(Pens.Yellow, Location.X, shotLocationPlayer -= moveInterval, width, height); break;
                    case Direction.DOWN: g.DrawRectangle(Pens.Red, Location.X, shotLocationInvader += moveInterval, width, height); break;
                }

                switch (direction)
                {
                    case Direction.UP: Location = new Point(Location.X, shotLocationPlayer); break;
                    case Direction.DOWN: Location = new Point(Location.X, shotLocationInvader); break;
                }
            }
            
        }

           //WEAPON Types:
          //case WEAPON.GUIDINGLASER: playersShotImage = new Bitmap(Properties.Resources.bug2); break;
          //          case WEAPON.LASER: playersShotImage = new Bitmap(Properties.Resources.bug2); break;
          //          case WEAPON.MULTIPLESHOTS: playersShotImage = new Bitmap(Properties.Resources.bug2); break;
          //          case WEAPON.MULTIROCKETS: playersShotImage = new Bitmap(Properties.Resources.bug2); break;
          //          case WEAPON.ROCKET: playersShotImage = new Bitmap(Properties.Resources.SmallHomingMissile); break;

        void GuidingLaser(Graphics g)
        {
            
            PlayerShip.IsLaser = true;
            playersShotImage = new Bitmap(Properties.Resources.SmallHomingMissile);
            //g.DrawLine(Pens.Sienna, new Point(PlayerShip.move+20, 600 ), new Point(PlayerShip.move, 200));
            g.DrawLine(Pens.Sienna, new Point(PlayerShip.move + 27, laserBeamStartPoint+200), new Point(PlayerShip.move + 27, 200));
            g.DrawImage(playersShotImage,new Point(PlayerShip.move,shotLocationPlayer -= moveInterval));
            //////////NeedToComplete new Point(PlayerShip.move, shotLocationPlayer)
            Area = new Rectangle(new Point(PlayerShip.move, shotLocationPlayer), playersShotImage.Size);
        }

        const int LASERFADESAWAY=200;
        int laserBeamStartPoint=700;
        int laserStartpoint = PlayerShip.move;
        int laserUp;
        void Laser(Graphics g)
        {
            PlayerShip.IsLaser = false;
            laserUp = 800;
            g.FillRectangle(Brushes.Plum, new Rectangle(Location.X-13, laserBeamStartPoint -= LASERFADESAWAY, 30, 400));
            //Me: Draw a laser line that follows the player. once it hits the player , it is being removed.
          //  g.DrawLine(Pens.Sienna, new Point(Location.X, laserBeamStartPoint -= LASERFADESAWAY), new Point(Location.X, 0));
            //////////NeedToComplete
            Area = new Rectangle(Location.X - 13, laserBeamStartPoint, 30, 400);
        }

        void MultipleShots(Graphics g)
        {
            PlayerShip.IsLaser = false;

            g.DrawEllipse(Pens.Yellow, Location.X - 5, shotLocationPlayer -= MOVE_AT_FASTER_INTERVAL, width, height);
            g.DrawEllipse(Pens.Yellow, Location.X + 5, shotLocationPlayer -= MOVE_AT_FASTER_INTERVAL, width, height);
            g.DrawEllipse(Pens.Yellow, Location.X, shotLocationPlayer -= MOVE_AT_FASTER_INTERVAL, width, height);
            //////////NeedToComplete
            Area = new Rectangle(PlayerShip.move, shotLocationPlayer, 3, 600);
          
        }
        int multiRocketSpeedA = 600;
        int multiRocketSpeedB = 600;
        int multiRocketSpeedC = 600;
        int multiRocketSpeedD = 600;

        void MultiRocketShots(Graphics g)
        {
            PlayerShip.IsLaser = false;
                //Four pictures
            int tempPlayerDistance = Location.X;
                Bitmap[] homingMissiles = new Bitmap[] { Properties.Resources.SmallHomingMissile, Properties.Resources.SmallHomingMissile, Properties.Resources.SmallHomingMissile, Properties.Resources.SmallHomingMissile };
                //Draw the four pictures. The x axis changes to move left or right. The Y axis moves the missiles upwards.


                      /*
                       * multiRocketSpeedA = distance traveling from the bottom up.
                       * tempPlayerDistance, is the distance from which the player travels.
                       */ 
                    g.DrawImage(homingMissiles[0], new Point(tempPlayerDistance -= 30, multiRocketSpeedA -= 30));
                    g.DrawImage(homingMissiles[1], new Point(tempPlayerDistance += 20, multiRocketSpeedB -= 30));
                    g.DrawImage(homingMissiles[2], new Point(tempPlayerDistance += 20, multiRocketSpeedC -= 30));
                    g.DrawImage(homingMissiles[3], new Point(tempPlayerDistance += 20, multiRocketSpeedD -= 30));
                    
            //////////NeedToComplete
                    Area = new Rectangle(Location.X, multiRocketSpeedA, homingMissiles[0].Size.Width, homingMissiles[0].Size.Height-120);
                        //Location.X, multiRocketSpeedD,
          
        }

        void rocket(Graphics g)
        {
            PlayerShip.IsLaser = false;
            playersShotImage = new Bitmap(Properties.Resources.SmallHomingMissile);
            g.DrawImage(playersShotImage,Location.X, shotLocationPlayer);
            // public Rectangle Area { get { return new Rectangle(Location, img.Size); } }
            Area = new Rectangle(new Point(Location.X, shotLocationPlayer), playersShotImage.Size);
        }


     


        public bool Move()
        {
            /*
             * The shot moves the object up and down, and keeps the object within
             * the game boundary.
             */
            if (shotLocationPlayer < 5 || laserBeamStartPoint < 5 || multiRocketSpeedD <5)
            {
                return true;
            }
            return false;
        }

        public bool RemoveInvaderShots()
        {
            if (shotLocationInvader > 990)
            {
                return true;
            }
            return false;
        }
    }
    public enum WEAPON{ROCKET, MULTIROCKETS, GUIDINGLASER, MULTIPLESHOTS, LASER, DEFAULT}
}

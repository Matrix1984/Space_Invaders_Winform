using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    /*
     * The Game object has 5 different methods that get triggered by different
     * events in the form. - Those are:
     * Draw()
     * Twinkle()
     * MovePlayer()
     * FireShot()
     * Go()
     * 
     * Methods to add:
     * CheckedForPlayersCollisions()
     * CheckForInvadersCollisions()
     * MoveInvaders()
     * ReturnFire()
     */ 
    public class Game
    {
        PlayerShip playerShip;
      
        Shot shot;
        Stars star;
        SpaceInvaders spaceInvaders;
        Game game;
        Invaders invader;
        public Rectangle gameBoundaries;

        public Game(Rectangle rectangle)
        {
            gameBoundaries = rectangle;
        }
        public void getShotReference(Shot sho)
        {
            shot = sho;
        }
        public void getStarReference(Stars sta)
        {
            star = sta;
        }
        public  void getSpaceInvadoerReference(SpaceInvaders space)
        {
            spaceInvaders = space;
        }    

        public void getSpaceInvadersReference(SpaceInvaders spaceInvader)
        {
            spaceInvaders = spaceInvader;
        }

        public void getGameReference(Game gam)
        {
            game = gam;
        }

        public void getShipReference(PlayerShip playerS)
        {
            playerShip = playerS;
        }

        public WEAPON WeaponChosenGame { get; set; }

        public bool isWeapon { get; set; }
      

        /*
         * The event GameOver should be fired when the game doesnt have
         * any lives left. Event handler method should be built in the From
         * and hook it into the Games' GameOver event
         */

        public event EventHandler GameOver;
        //The three variables underneath keep the state of the game.
        private int score;
        private int livesLeft = 2;
        private int wave = 0;
        public static bool gameOverToAll;
        /*
         The frame field is used to slow down animation.
         * The first frame should skip 6 frames before they move to the left.
         * The next wave should skip 5, 
         * and the next should skip 4 etc.
         */
        private int frameSkipped = 0;
        List<Invaders> invaders = new List<Invaders>();
        PlayerShip playership;
        List<Shot> Invadershots = new List<Shot>();
        List<Shot> playerShots = new List<Shot>();
        //The stars object keeps track of the twinkling stars in the background.
        List<Stars> starList = new List<Stars>();
        //Alive: The animation should be done by PlayerShip class
        public bool Alive { get; set; }
        //if any of the aliens were hit, the Game object removes those Aliens from the list.
        public static bool InvaderPassedTheBoundaryGameOver;
        public void RandomPen()
        {
            for (int i = 1; i < 300; i++)
            {
                starList.Add(new Stars(spaceInvaders.DisplayRectangle, new Random()));
            }
        }


        public void gameReset()
        {
            invaders.Clear();
            Invadershots.Clear();
            playerShots.Clear();
        }
  
        public void Go()
        {
            if (InvaderPassedTheBoundaryGameOver)
            {
                Alive=false;
            }
           
            PlayerAliveEventArgs playerAlive = new PlayerAliveEventArgs(Alive);
            GameOver(this, playerAlive);
            //in this function all the game is being managed
            //if the player runs out of lives, the game declares a game over and raises the game over event.
            //checks everything from movement to shots to see how the game runs.
            /*
             * The animation timer calls the go method anywhere between 10 and 30 times a second.
             * The method does everything for the game to advance itself frame by frame.
             * The game checks if the player is alive by checking the Alive property.
             * If the player is dead, the animation timer would stop with a Stop() method.
             * So the method wont do anything until the player is alive again, it would simply return.
             * 
             * Each shot needs to be updated, if the shot went off the screen the shot should be deleted.
             * The game then lets the invaders move and return fire.The game needs to loop through the 
             * List<Shot> object, calling the Move() of each. if Move() returns false, it means the shot 
             * went off the screen.
             * 
             * It should also check for collision, if the pictures overlap, the two should be removed.
             * We will Add Rectangle property called Area, to the invader and playerShip class..
             * so we can use the Contains() method to see if the area overlaps with the shot
             */

            if (!Alive)
            {
                Stop();
                return;
            }

          
             MoveInvaders();
             ReturnFire();
            
             CheckedForPlayersCollisions();

            // will remove shots if out of the screen

             Shot invaderFired = new Shot();
             if (Invadershots != null)
             {
                 foreach (var shotItem in Invadershots)
                 {
                     if (shotItem.RemoveInvaderShots())
                     {

                         invaderFired = shotItem;
                         break;
                     }
                 }
                 Invadershots.Remove(invaderFired);
             }
        }

        public void Stop()
        {
            spaceInvaders.stopAllAnimation = true;
        }
     
        int invaderDirection=10;
        
        public void NextWave()
        {
            /*
             * This method should assign a list of invaders.
             * Add 30 invaders in 6 columns...
             * increase the wave field by 1, and set their invaderDirection
             * field to start them moving towards right side of the screen.
             * You will also change the frameSkipped field.
             */
            int[] positionsX = new int[] { 100, 200, 300, 400, 500 };
            int[] positionsY = new int[] { 10, 70, 130, 190,  260 };

            invaders.Add(new Invaders(Invaders.Type.Satelite, new Point(positionsX[0], positionsY[0]), 50));
            invaders.Add(new Invaders(Invaders.Type.Satelite, new Point(positionsX[1], positionsY[0]), 50));
            invaders.Add(new Invaders(Invaders.Type.Satelite, new Point(positionsX[2], positionsY[0]), 50));
            invaders.Add(new Invaders(Invaders.Type.Satelite, new Point(positionsX[3], positionsY[0]), 50));
            invaders.Add(new Invaders(Invaders.Type.Satelite, new Point(positionsX[4], positionsY[0]), 50));


            invaders.Add(new Invaders(Invaders.Type.Bug, new Point(positionsX[0], positionsY[1]), 40));
            invaders.Add(new Invaders(Invaders.Type.Bug, new Point(positionsX[1], positionsY[1]), 40));
            invaders.Add(new Invaders(Invaders.Type.Bug, new Point(positionsX[2], positionsY[1]), 40));
            invaders.Add(new Invaders(Invaders.Type.Bug, new Point(positionsX[3], positionsY[1]), 40));
            invaders.Add(new Invaders(Invaders.Type.Bug, new Point(positionsX[4], positionsY[1]), 40));


            invaders.Add(new Invaders(Invaders.Type.Saucer, new Point(positionsX[0], positionsY[2]), 30));
            invaders.Add(new Invaders(Invaders.Type.Saucer, new Point(positionsX[1], positionsY[2]), 30));
            invaders.Add(new Invaders(Invaders.Type.Saucer, new Point(positionsX[2], positionsY[2]), 30));
            invaders.Add(new Invaders(Invaders.Type.Saucer, new Point(positionsX[3], positionsY[2]), 30));
            invaders.Add(new Invaders(Invaders.Type.Saucer, new Point(positionsX[4], positionsY[2]), 30));


            invaders.Add(new Invaders(Invaders.Type.SpaceShip, new Point(positionsX[0], positionsY[3]), 20));
            invaders.Add(new Invaders(Invaders.Type.SpaceShip, new Point(positionsX[1], positionsY[3]), 20));
            invaders.Add(new Invaders(Invaders.Type.SpaceShip, new Point(positionsX[2], positionsY[3]), 20));
            invaders.Add(new Invaders(Invaders.Type.SpaceShip, new Point(positionsX[3], positionsY[3]), 20));
            invaders.Add(new Invaders(Invaders.Type.SpaceShip, new Point(positionsX[4], positionsY[3]), 20));


            invaders.Add(new Invaders(Invaders.Type.Star, new Point(positionsX[0], positionsY[4]), 10));
            invaders.Add(new Invaders(Invaders.Type.Star, new Point(positionsX[1], positionsY[4]), 10));
            invaders.Add(new Invaders(Invaders.Type.Star, new Point(positionsX[2], positionsY[4]), 10));
            invaders.Add(new Invaders(Invaders.Type.Star, new Point(positionsX[3], positionsY[4]), 10));
            invaders.Add(new Invaders(Invaders.Type.Star, new Point(positionsX[4], positionsY[4]), 10));
          
        }
    
        public void MovePlayer(Direction direction)
        {
            /*
             * checks whether the player is dead.
             * calls  playership.Move() to effect the movement of the ship
             * need to take a method with a parameter of enum Direction
             */
          
            if (Alive)
            {
                playership.Move(direction);
            }
        }

        public void FireShot()
        {
            Shot playerShipFired = new Shot();
          //Fires players shot.

            if (playerShots != null)
            {
                foreach (var shotItem in playerShots)
                {
                    if (shotItem.Move())
                    {
                        playerShipFired = shotItem;
                        break;
                    }
                }
                playerShots.Remove(playerShipFired);
            }
      

            if (playerShots.Count < 4)
            {
                playerShots.Add(new Shot(new Point(PlayerShip.move + 25, 900), Direction.UP, gameBoundaries, WeaponChosenGame, isWeapon));   
            }

            if (invaders.Count > 4)
            {
                /*
                 * If the invader count is less than 4, choose random 4 invaders and pass it to the Shot class.
                 * So that the player will have the shot.
                 */ 
                Random r = new Random();
                Invaders invaderA=invaders[r.Next(0,invaders.Count)];
                Invaders invaderB = invaders[r.Next(0, invaders.Count)];
                Invaders invaderC = invaders[r.Next(0, invaders.Count)];
                Invaders invaderD = invaders[r.Next(0, invaders.Count)];
                Shot.FourRandomInvaderLocations = new int[] { invaderA.Location.X, invaderB.Location.X, invaderC.Location.X, invaderD.Location.X };
            }

        }
      
        public void Draw(Graphics  g,int animationCell)
        {
            /*
             * it should draw a black rectangle that fills the whole form.
             * using display rectangle set in boundaries, received from Form.
             * The method should draw the stars, invaders, players ship, shots.
             * it should draw the score in the upper left corner.
             * and a big "Game over" if game over is found to be true.
            //it will tell each invader which cell to be drawn based on the animationCell being passed.
             * 
             */
            g.Clear(Color.Black);
            Bitmap weaponDisplay;
            switch (WeaponChosenGame)
            {
                case WEAPON.GUIDINGLASER: weaponDisplay = new Bitmap(Properties.Resources.WEAPON_DISPLAY_GuidedLaserRocket); WeaponChosenGame = WEAPON.GUIDINGLASER; isWeapon = true; break;
                case WEAPON.LASER: weaponDisplay = new Bitmap(Properties.Resources.WEAPON_DISPLAY_Laser); WeaponChosenGame = WEAPON.LASER; isWeapon = true; break;
                case WEAPON.MULTIPLESHOTS: weaponDisplay = new Bitmap(Properties.Resources.WEAPON_DISPLAY_multiShots); WeaponChosenGame = WEAPON.MULTIPLESHOTS; isWeapon = true; break;
                case WEAPON.MULTIROCKETS: weaponDisplay = new Bitmap(Properties.Resources.WEAPON_DISPLAY_multiRockets); WeaponChosenGame = WEAPON.MULTIROCKETS; isWeapon = true; break;
                default: weaponDisplay = new Bitmap(Properties.Resources.WEAPON_DISPLAY_RegularShots); WeaponChosenGame = WEAPON.DEFAULT; isWeapon = false; break;
            }
            g.DrawImage(weaponDisplay, new Point(0,0));
            g.DrawRectangle(Pens.Black, new Rectangle(new Point(0, 0), this.gameBoundaries.Size));

            if (invaders.Count == 0)
            {
                using (Font arial28bold = new Font("Arial", 24, FontStyle.Bold))
                {
                    g.DrawString("YOU WON THE GAME!!!", arial28bold, Brushes.BlueViolet, 300, 400);
                }
            }

            //players Score
            using (Font arial28bold = new Font("Arial", 54, FontStyle.Bold))
            {
                g.DrawString(Invaders.Score.ToString(), arial28bold, Brushes.Yellow, 800, 15);
            }

            foreach (Invaders item in invaders)
            {
                if (item != null)
                    item.Draw(g, animationCell);
            }

            playerShip.Draw(g);

            foreach (Shot item in Invadershots)
            {
                if (item != null)
                    item.Draw(g, animationCell);
            }

            foreach (Shot item in playerShots)
            {
                if (item != null)
                    item.Draw(g, animationCell);
            }

            foreach (Stars item in starList)
            {
                if (item != null)
                    item.Draw(g, animationCell);
            }
        }

        static bool changingDirction;

        private void MoveInvaders()
        {   
            /*
             * part of LINQ
             * First thing that this method should do is update frameSkipped field.
             * and return if Frames should be skipped, (Depending on the level).
             * then, it should check which direction the invaders are moving..
             * if they are moving to the right. MoveInvaders should search the 
             * invaderCollection for any invaders whose X location  is within 
             * 100 pixels of right hand boundary.
             * 
             * if it finds any, it should tell the invaders to march downwards, ( Move(Direction direction) )
             * and then set invaderDirection to Direction.Left. if not it can tell
             * the invaders to march to the left..
             * then , it should do the opposite with LINQ
             */ 
        //    public int formsWidth  { get; set; }
        //public int formsHeight { get; set; }
            int boundaryToStartMovingDownLeftToRight = gameBoundaries.Width - 100;
            int boundaryToStartMovingDownRightToLeft =  100;
            int width;

            if (!changingDirction)
            {
               var outofBounds = from InvadersPassedBoundary in invaders
                                 where InvadersPassedBoundary.movementX > boundaryToStartMovingDownLeftToRight
                                 select InvadersPassedBoundary;


               List<Invaders> howMany = outofBounds.ToList();
             
               if (howMany.Count==0)
                {
                    foreach (var item in invaders)
                    {
                        item.Move(Direction.RIGHT);

                    }      
                }
                else
                {
                    foreach (var item in invaders)
                    {
                        item.Move(Direction.DOWN);
                    }
                    changingDirction = true;

                }
            }
            else
            {
                var outofBounds = from InvadersPassedBoundary in invaders
                                  where InvadersPassedBoundary.movementX < 100
                                  select InvadersPassedBoundary;

                List<Invaders> howMany = outofBounds.ToList();

                if (howMany.Count == 0)
                {
                    foreach (var item in invaders)
                    {
                        item.Move(Direction.LEFT);
                    }
                }
                else
                {
                    foreach (var item in invaders)
                    {
                        item.Move(Direction.DOWN);
                    }
                    changingDirction = false;
                }        
            }
        }

        private void ReturnFire() 
        {
            /*
             * First thing it should return the invaders shot list 
             * already has wave+1.
             * 
             * it should also return random.Next(10)<10-wave.
             * (that makes the invaders fire at random and not all the time.
             * 
             * if it past both tests LINQ should sort all invaders in descending order by location.X
             * 
             *  Once it has got the groups, it can choose a group at random, and find with the method First()
             *  the invader at the bottom of the column.
             *  
             * You can add a shot to the invaders list. just below the middle of the invader (Use invaders area
             * to set the shots location).
             * 
             */
            Random r = new Random();
            int InvadorNumberA=r.Next(0,5);
            int randomShot = r.Next(10);

            List<Invaders> invadersShooting = new List<Invaders>();
            Invaders invaderA=new Invaders();
      
            var invaderByLocationX = from invadersSortByLocation in invaders
                                     group invadersSortByLocation by invadersSortByLocation.Location.Y
                                     into invaderGroup
                                     orderby invaderGroup.Key
                                     select invaderGroup;

            var invader = invaderByLocationX.LastOrDefault();
            if (invader != null)
            {

                if (invaderByLocationX != null)
                {
                    invadersShooting = invaderByLocationX.Last().ToList();

                    invaderA = invadersShooting[r.Next(0, invadersShooting.Count)];

                    if (r.Next(5) < 4 - randomShot)
                    {
                        Invadershots.Add(new Shot(invaderA.Location, Direction.DOWN, gameBoundaries, WEAPON.DEFAULT, isWeapon));
                    }
                }
            }
        }

        public void CheckedForPlayersCollisions()
        {
            /*
             * //Part of LINQ
             * There are three collisions to check for..
             * and the Rectangles struct's Contains method will come handy here..
             * just pass it any Point. and return true if the point is inside the rectangle.
             * 
             * use LINQ to loop over all the shots the player fired and select any invader
             * where invader.Area contains shots location. Remove the invader and the shot.
             * 
             * Add querry to figure out if any of the invaders reached the bottom of the screen.
             * 
             * loop inside the players Area property to see if he was shot.
            */
            //invadersItem
            bool removeShot = false;
            bool removeInvaderBool = false;
            Shot removePlayerShot = new Shot();
            Invaders removeInvader = new Invaders();
            List<Invaders> hitInvaders = new List<Invaders>();
            var AliensHit = from invader in invaders
                            select invader;
            Random rex = new Random();
            if (playerShots.Count != 0)
            {
                foreach (var playerShot in playerShots)
                {
                    if (isWeapon)
                    {
                        //foreach (Invaders Invaderitem in invaders)
                        //{
                        //    if (Invaderitem.Area.Contains(player.Area))
                        //    {
                        //        removeInvaderBool = true;
                        //        removeInvader =  Invaderitem;
                        //        break;
                        //    }
                        //}player.Area

                        AliensHit = from invader2 in invaders
                                    where invader2.Area.IntersectsWith(playerShot.Area)
                                    select invader2;

                        //Test
                        if (playerShot.shotLocationPlayer < 100)
                        {
                            bool seeeme = false;
                           
                            int d = playerShot.Area.Y;
                           int d2 = removeInvader.Area.Y;
                           hitInvaders = AliensHit.ToList();
                         
                        }
                    }
                    else
                    {
                        AliensHit = from invader in invaders
                                    where invader.Area.Contains(playerShot.Location)
                                        select invader;
                    }


                    hitInvaders = AliensHit.ToList();
                    if (hitInvaders.Count != 0)
                    {
                        removeShot = true;
                        removePlayerShot = playerShot;
                        removeInvaderBool = true;
                        removeInvader = hitInvaders[0];
                        break;
                    }
                }

                

                foreach (var invaderItem in hitInvaders)
                {
                    // if the area of hte alien contains the shot remove the shot.
                    foreach (var player in playerShots)
                	{
                        if (isWeapon)
                        {
                            //Me: Will remove players shot.
                            if (invaderItem.Area.IntersectsWith(player.Area))
                            {
                             
                                removeShot = true;
                                removePlayerShot = player;
                                removeInvaderBool = true;
                            }
                          
                        }
                        else
                        {
                            //Me: Will remove players shot.
                            if (invaderItem.Area.Contains(player.Location))
                            {
                                removeShot = true;
                                removePlayerShot = player;
                                removeInvaderBool = true;
                            }
                        }
	                }
                    // remove the alien that contains the shot
                 
                    removeInvader = invaderItem;
                    
                }
                if (removeShot)
                {
                      Invaders.Score += removeInvader.addedScore;  
                    playerShots.Remove(removePlayerShot);
                }
                if (removeInvaderBool)
                {
                    Invaders.Score += removeInvader.addedScore;  
                    bool ok=invaders.Remove(removeInvader);
                    System.Diagnostics.Debug.Assert(ok);
                    if (!ok)
                    {
                        bool debugme = false;
                    }
                }
            }

            // Checks if the invader reached the bottom
              var reachedBottom = from invader in invaders
                                       where invader.movementX == gameBoundaries.Height
                                       select invader;
         
              bool playersWasShot=false;

              List<Shot> hitPlayer;
            ////////////////////Test//////////////////////////////

                  var PlayersHit = from shotsInvader in Invadershots
                                   where playerShip.Area.Contains(shotsInvader.Location)
                                   select shotsInvader;

                  hitPlayer = PlayersHit.ToList();

                  if (hitPlayer.Count != 0)
                  {
                      playersWasShot = true;
                  }
          

          
                 


              ////////////////////Test//////////////////////////////

              //foreach (var shotsInvader in Invadershots)
              //{
              //    if (playerShip.Area.Contains(shotsInvader.Location))
              //    {
              //        //player was shot game over.
              //        playersWasShot=true;
              //        break;
              //    }
              //}
              List<Invaders> invadersShotPlayer = reachedBottom.ToList();
              if (invadersShotPlayer.Count!=0 || playersWasShot)
              {
                  //Game is over
                 Alive = false;
              }     
        }


  
    }

  public  class PlayerAliveEventArgs : EventArgs
  {

        public bool Alive { get; set; }

        public PlayerAliveEventArgs(bool deadOrAlive)
        {
            Alive = deadOrAlive;
        }
  }
   
}

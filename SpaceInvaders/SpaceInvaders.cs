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
    public partial class SpaceInvaders : Form
    {
        /*
         * The form passes in an initialized Random object and its own
         * DisplayRectangle struct. So the game can figure out the boundaries of 
         * the battlefield.
         */
        public SpaceInvaders()
        {
            InitializeComponent();
        }

        Game game;
        public PlayerShip playerShip;
        public Shot shot;
        public Stars star;
        Invaders invaders;



        public void SpaceInvaders_Load(object sender, EventArgs e)
        {
            //passes the height and the width of the screen to game. - to know the boundaries fo the form.this.DisplayRectangle.Height, this.DisplayRectangle.Width
            //Passing all references. Preventing multiple objects from being created.
            playerShip = new PlayerShip();
            
            game = new Game(this.DisplayRectangle);
            game.getShipReference(playerShip);
            game.getSpaceInvadersReference(this);
            game.NextWave();
            game.Alive = true;
            game.GameOver += Form1_GameOverMethod;
        }

        public void Form1_GameOverMethod(object sender, EventArgs e)
        {
            PlayerAliveEventArgs d = (PlayerAliveEventArgs)e;
            if (!d.Alive)
            {
                GameTimer.Enabled = false;
                gameOver = true;
                Invalidate();
                Refresh();
            }
        }

        //List of keys needs to be saved for the timers to be used.
        //Those keys will be used for game timer
        List<Keys> keyPressed = new List<Keys>();

         public bool gameOver { get; set; }

         private void SpaceInvaders_KeyDown(object sender, KeyEventArgs e)
         {
            
            if (e.KeyCode == Keys.Q)
            {
                Application.Exit();
            }

            //if (gameOver)
            //{
                //pressing S should restart the game over.
            switch (e.KeyCode)
            {
                case Keys.NumPad1: game.WeaponChosenGame = WEAPON.GUIDINGLASER; break;
                case Keys.NumPad2: game.WeaponChosenGame = WEAPON.LASER; break;
                case Keys.NumPad3: game.WeaponChosenGame = WEAPON.MULTIPLESHOTS; break;
                case Keys.NumPad4: game.WeaponChosenGame = WEAPON.MULTIROCKETS; break;
                case Keys.NumPad5: game.WeaponChosenGame = WEAPON.ROCKET; break;
                case Keys.NumPad0: game.WeaponChosenGame = WEAPON.DEFAULT;
                    break;
            }
                if (e.KeyCode == Keys.S)
                {
                    Game.InvaderPassedTheBoundaryGameOver = false;
                    game.gameReset();
                    GameTimer.Enabled = true;
                    game.Alive = true;
                    gameOver = false;
                    Invaders.Score = 0;
                    game.NextWave();
                }
                //code to reset the game and start the timers
            //}
            //else
            //{
                if (e.KeyCode == Keys.Space)
                {
                    game.FireShot();
                }
                if (e.KeyCode == Keys.Right)
                {
                    playerShip.Move(Direction.RIGHT);
                }
                if (e.KeyCode == Keys.Left)
                {
                    playerShip.Move(Direction.LEFT);
                }
                //by removing a key and adding it, we make it the "first" in the list.
                if (keyPressed.Contains(e.KeyCode))
                {
                    keyPressed.Remove(e.KeyCode);
                    keyPressed.Add(e.KeyCode);
                }
            //}
        }

        //if key was released they key will be removed.
         private void SpaceInvaders_KeyUp(object sender, KeyEventArgs e)
         {
            if(keyPressed.Contains(e.KeyCode))
            {
                keyPressed.Remove(e.KeyCode);
            }
        }

        //My: Gets input from the player..and passes them on. Calls game.Go()
        private void GameTimer_Tick(object sender, EventArgs e)
        {
    
            if (keyPressed.Count >= 1)
            {
                //0 will be your most recent key pressed.
                switch (keyPressed[0])
                {
                    case Keys.Left:
                        game.MovePlayer(Direction.LEFT);
                        break;
                    case Keys.Right:
                        game.MovePlayer(Direction.RIGHT);
                        break;
                }
            }
            game.Go();//that function lets the game continue.
        }

        int animationTimerCounter;

        public bool stopAllAnimation { get; set; }

        //My: counts 4 times. Refreshes the form.
        private void animationTimer_Tick(object sender, EventArgs e)
        {
            //timer should go 0,1,2,3..and then reset
           
                animationTimerCounter++;
                if (animationTimerCounter > 3)
                {
                    animationTimerCounter = 0;
                }

                //screen gets repainted.
                Refresh();
        }

        //My: Repaints the screen. calls game.Draw().
        private void SpaceInvaders_Paint(object sender, PaintEventArgs e)
        {
            //the animation has 4 cell to draw, so the arguments are passed to the game objects instructing it to which cells to draw.
            game.Draw(e.Graphics, animationTimerCounter);
            Graphics g = e.Graphics;

            if (gameOver)
            {
                // set code that writes game over in yellow letters.
                using (Font arial28bold = new Font("Arial", 54, FontStyle.Bold))
                {
                    g.DrawString("Game Over", arial28bold, Brushes.Red, 300 , 400);
                }
                using (Font arial28bold = new Font("Arial", 24, FontStyle.Bold))
                {
                    g.DrawString("Press S to start a new game or Q to quit", arial28bold, Brushes.Yellow, 200, 605);
                }
            }
        }        
    }
    public enum Direction { LEFT, RIGHT, UP, DOWN }
}

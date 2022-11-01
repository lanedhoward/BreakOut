using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;

namespace BreakoutBoring
{
    class Paddle : DrawableSprite
    {
        //Service Dependencies
        GameConsole console;

        //Dependencies
        PaddleController controller;
        Ball ball;      //Need reference to ball for collision

        bool autopaddle;  //cheat

        float influence;
        float maxInfluenceFactor;

        Vector2 lastDirection;
        float dashDistance;
        
        public Paddle(Game game, Ball b)
            : base(game)
        {

            this.autopaddle = false; //just press A while playing to toggle
            this.Speed = 350f;
            this.ball = b;
            this.influence = 0.2f;
            maxInfluenceFactor = 2.0f;
            controller = new PaddleController(game, ball);
            lastDirection = new Vector2(1,0);
            dashDistance = 150f;

            //Lazy load GameConsole
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console make a new one and add it to the game
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }

            r = new Random();
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("paddleSmall");
#if DEBUG   //Show markers if we are in debug mode
            this.ShowMarkers = true;
#endif
            SetInitialLocation();
            base.LoadContent();
        }

        public void SetInitialLocation()
        {
            this.Location = new Vector2(300, 450); //Shouldn't hard code inital position TODO set to be realtive to windows size

        }

        Rectangle collisionRectangle;  //Rectangle for paddle collision uses just the top of the paddle instead of the whole sprite

        public override void Update(GameTime gameTime)
        {
            //Update Collision Rect
            collisionRectangle = new Rectangle((int)this.Location.X, (int)this.Location.Y, this.spriteTexture.Width, 1);

            //Deal with ball state
            switch (ball.State)
            {
                case BallState.OnPaddleStart:
                    //Move the ball with the paddle until launch
                    UpdateMoveBallWithPaddle();
                    break;
                case BallState.Playing:
                    UpdateCheckBallCollision();
                    break;
            }

            //Movement from controller
            controller.HandleInput(gameTime);
            
            this.Direction = controller.Direction;
            
            if (Direction.X != 0)
            {
                lastDirection = Direction;
            }

            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            if (controller.dashPressed)
            {
                this.Location += this.lastDirection * (this.dashDistance);
            }
            if (controller.autoPressed)
            {
                this.autopaddle = !this.autopaddle;
            }

            KeepPaddleOnScreen();

            if(autopaddle && ball.State == BallState.Playing) //Alllow cheating
            {
                this.Location.X = ball.Location.X - ((int)this.spriteTexture.Width/2 * this.scale);
            }

            base.Update(gameTime);
        }

        private void UpdateMoveBallWithPaddle()
        {
            ball.Speed = 0;
            ball.Direction = Vector2.Zero;
            ball.Location = new Vector2(this.Location.X + (this.LocationRect.Width/2 - ball.SpriteTexture.Width/2), this.Location.Y - ball.SpriteTexture.Height);
        }

        Vector2 initBallDir;
        private void UpdateCheckBallCollision()
        {
            //Ball Collsion
            //Very simple collision with ball only uses rectangles
            // check if ball is headed down, avoid spam collisions glitch
            if (collisionRectangle.Intersects(ball.LocationRect) && (ball.Direction.Y > 0))
            {
                initBallDir = ball.Direction;
                ball.Direction.Y *= -1;
                UpdateBallCollisionBasedOnPaddleImpactLocation();

                ScoreManager.ResetMultiplier();

                //console.GameConsoleWrite("Paddle collision ballXDirStart:" + initBallDir.X + " ballXDirEnd:" + ball.Direction.X);
            }
        }

        Random r;

        /// <summary>
        /// Adds a bit of randomness to the ball bounce
        /// </summary>
        private void UpdateBallCollisionRandomFuness()
        {
            /// 
            /// Adds a bit of entropy to bounce nothing should be perfect
            /// 
            /// 
            ball.Direction.Y = GetReflectEntropy();
        }


        private float GetReflectEntropy()
        {
            return -1 + ((r.Next(0, 3) - 1) * this.influence); //return -.9, -1 or -1.1
        }

        /// <summary>
        /// Makes the paddle more able to direct the ball
        /// </summary>
        private void UpdateBallCollisionBasedOnPaddleImpactLocation()
        {
            //Change anlge based on side of paddle
            //First Third

            if ((ball.Location.X > this.Location.X) && (ball.Location.X < this.Location.X + this.spriteTexture.Width / 3))
            {
                console.GameConsoleWrite("1st Third");
                ball.Direction.X -= this.influence;
            }
            if ((ball.Location.X > this.Location.X + (this.spriteTexture.Width / 3)) && (ball.Location.X < this.Location.X + (this.spriteTexture.Width / 3) * 2))
            {
                console.GameConsoleWrite("2nd third");
            }
            if ((ball.Location.X > (this.Location.X + (this.spriteTexture.Width / 3) * 2)) && (ball.Location.X < this.Location.X + (this.spriteTexture.Width)))
            {
                console.GameConsoleWrite("3rd third");
                ball.Direction.X += this.influence;
            }

            ball.Direction.X = Math.Clamp(ball.Direction.X, -1 - (this.maxInfluenceFactor * this.influence), 1 + (this.maxInfluenceFactor * this.influence));

            //not fun!
            //ball.Direction.Normalize();
        }

        private void KeepPaddleOnScreen()
        {
            this.Location.X = MathHelper.Clamp(this.Location.X, 0, this.Game.GraphicsDevice.Viewport.Width - this.spriteTexture.Width);
        }
    }
}

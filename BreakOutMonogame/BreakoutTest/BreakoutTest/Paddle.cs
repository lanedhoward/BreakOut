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

namespace BreakoutTest
{
    class Paddle : DrawableSprite
    {
        //Service Dependencies
        GameConsole console;

        //Depandencies
        PaddleController controller;

        Ball ball;      //Need reference to ball for collision
        Rectangle collisionRectangle;  //Rectangle for paddle collision

        public Paddle(Game game, Ball b)
            : base(game)
        {
            this.Speed = 200;
            this.ball = b;
            controller = new PaddleController(game, ball);

            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("paddleSmall");
            this.ShowMarkers = true;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Movement from controller
            controller.HandleInput(gameTime);
            this.Direction = controller.Direction;

            //Collision Rect
            collisionRectangle = new Rectangle((int)this.Location.X, (int)this.Location.Y, this.spriteTexture.Width, 1);

            if (ball.IsOnPaddle)
            {
                //Move the ball with the paddle until launch
                ball.Speed = 0;
                ball.Direction = Vector2.Zero;
                ball.Location = new Vector2(this.Location.X + this.LocationRect.Width / 2, this.Location.Y - ball.SpriteTexture.Height);
            }
            else
            {
                //Ball Collsion
                //Very simple collision with ball only uses rectangles
                if (collisionRectangle.Intersects(ball.LocationRect))
                {
                    //TODO Change angle based on location of collision or direction of paddle
                    ball.Direction.Y *= -1;
                    //Change angle based on paddle movement
                    if (this.Direction.X > 0)
                    {
                        ball.Direction.X += .5f;
                    }
                    if (this.Direction.X < 0)
                    {
                        ball.Direction.X -= .5f;
                    }
                    //Change anlge based on side of paddle
                    //First Third

                    if ((ball.Location.X > this.Location.X) && (ball.Location.X < this.Location.X + this.spriteTexture.Width / 3))
                    {
                        console.GameConsoleWrite("1st Third");
                        ball.Direction.X += .1f;
                    }
                    if ((ball.Location.X > this.Location.X + (this.spriteTexture.Width / 3)) && (ball.Location.X < this.Location.X + (this.spriteTexture.Width / 3) * 2))
                    {
                        console.GameConsoleWrite("2nd third");
                    }
                    if ((ball.Location.X > (this.Location.X + (this.spriteTexture.Width / 3) * 2)) && (ball.Location.X < this.Location.X + (this.spriteTexture.Width)))
                    {
                        console.GameConsoleWrite("3rd third");
                        ball.Direction.X -= .1f;
                    }
                    console.GameConsoleWrite("Paddle collision ballLoc:" + ball.Location + " paddleLoc:" + this.Location.ToString());

                }
            }

            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);
            KeepPaddleOnScreen();
            base.Update(gameTime);
        }

        private void KeepPaddleOnScreen()
        {
            this.Location.X = MathHelper.Clamp(this.Location.X, 0, this.Game.GraphicsDevice.Viewport.Width - this.spriteTexture.Width);
        }
    }
}

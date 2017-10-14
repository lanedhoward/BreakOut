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
    enum BallState {  OnPaddleStart, Playing }

    class Ball : DrawableSprite
    {
        public bool IsOnPaddle;
        BallState State;

        GameConsole console;

        public Ball(Game game)
            : base(game)
        {
            this.IsOnPaddle = true;     //When game is started ball is on paddle
            this.State = BallState.OnPaddleStart;

            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }
            this.ShowMarkers = true;
        }

        public void LaunchBall(GameTime gameTime)
        {
            this.Speed = 190;
            this.Direction = new Vector2(1, 1);
            this.IsOnPaddle = false;
            this.console.GameConsoleWrite("Ball Launched " + gameTime.TotalGameTime.ToString());
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("ballSmall");
            base.LoadContent();
        }

        private void resetBall(GameTime gameTime)
        {
            this.Speed = 0;
            this.IsOnPaddle = true;
            this.console.GameConsoleWrite("Ball Reset " + gameTime.TotalGameTime.ToString());

        }

        public override void Update(GameTime gameTime)
        {
            if (IsOnPaddle)
            {
                return;
            }
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            //bounce off wall
            //Left and Right
            if ((this.Location.X + this.spriteTexture.Width > this.Game.GraphicsDevice.Viewport.Width)
                ||
                (this.Location.X < 0))
            {
                this.Direction.X *= -1;
            }
            //bottom Miss
            if (this.Location.Y + this.spriteTexture.Height > this.Game.GraphicsDevice.Viewport.Height)
            {
                this.resetBall(gameTime);
            }

            //Top
            if (this.Location.Y < 0)
            {
                this.Direction.Y *= -1;
            }

            base.Update(gameTime);
        }
    }
}

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
    public enum BallState {  OnPaddleStart, Playing }

    public class Ball : DrawableSprite
    {
        
        public BallState State { get; private set; }

        GameConsole console;

        public Ball(Game game)
            : base(game)
        {
            this.State = BallState.OnPaddleStart;

            //Lazy load GameConsole
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console let's add a new one
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }
#if DEBUG
            this.ShowMarkers = true;
#endif
        }

        public void SetInitialLocation()
        {
            this.Location = new Vector2(200, 300); //Hard coded position TODO fix this
        }

        public void LaunchBall(GameTime gameTime)
        {
            this.Speed = 190; //hard coded speed TODO fix this
            this.Direction = new Vector2(1, -1); //hard coded launch direction TODO fix this
            this.State = BallState.Playing;
            this.console.GameConsoleWrite("Ball Launched " + gameTime.TotalGameTime.ToString());
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("ballSmall");
            SetInitialLocation();
            base.LoadContent();
        }

        public void resetBall(GameTime gameTime)
        {
            resetBall();
            this.console.GameConsoleWrite("Ball Reset " + gameTime.TotalGameTime.ToString());
        }
        public void resetBall()
        {
            this.Speed = 0;
            this.State = BallState.OnPaddleStart;
        }

        public override void Update(GameTime gameTime)
        {
            switch(this.State)
            {
                case BallState.OnPaddleStart:
                    break;

                case BallState.Playing:
                    UpdateBall(gameTime);
                    break;
            }
            
            base.Update(gameTime);
        }


        bool reflectedX = false;
        bool reflectedY = false;
        private void UpdateBall(GameTime gameTime)
        {
            reflectedX = false;
            reflectedY = false;

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
                ScoreManager.LoseLife();
            }

            //Top
            if (this.Location.Y < 0)
            {
                this.Direction.Y *= -1;
            }
        }

        public void Reflect(MonogameBlock block)
        {
            
            // find center of ball and center of block
            // todo: refactor and add a FindCenter method to sprite class
            Vector2 ballCenter = new Vector2(Location.X + spriteTexture.Width / 2, Location.Y + spriteTexture.Height / 2);
            Vector2 blockCenter = new Vector2(block.Location.X + block.spriteTexture.Width / 2, block.Location.Y + block.spriteTexture.Height / 2);

            // get vertical and horizontal distance between centers
            float hDistance = blockCenter.X - ballCenter.X;
            float vDistance = blockCenter.Y - ballCenter.Y;

            // reflect along the largest difference
            if (Math.Abs(hDistance) > Math.Abs(vDistance))
            {
                //horizontal biggest, must be a side bounce
                if (reflectedX == false && reflectedY == false)
                {
                    this.Direction.X *= -1;
                    reflectedX = true;
                }
            }
            else
            {
                //vertical biggeest, must be a top/bottom bounce
                if (reflectedX == false && reflectedY == false)
                {
                    this.Direction.Y *= -1;
                    reflectedY = true;
                }
            }
        }

    }
}

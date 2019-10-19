using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroGameLibrary.Sprite2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IntroGameLibrary.Util;

namespace BreakoutTest
{
    class Paddle : DrawableSprite2
    {
        InputHandler input;
        Ball ball;
        Rectangle top;
        
        public Paddle(Game game, Ball b)
            : base(game)
        {
            input = (InputHandler)this.Game.Services.GetService(typeof(IInputHandler));
            this.Speed = 200;
            this.ball = b;
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("paddleSmall");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //simple keys
            this.Direction = Vector2.Zero;
            if(input.KeyboardState.IsKeyDown(Keys.Left))
            {
                this.Direction = new Vector2(-1,0);
            }
            if (input.KeyboardState.IsKeyDown(Keys.Right))
            {
                this.Direction = new Vector2(1, 0);
            }

            //Collision Rect
            top = new Rectangle((int)this.Location.X, (int) this.Location.Y, this.spriteTexture.Width, 1);
            //Ball Collsion
            if (top.Intersects(ball.LocationRect))
            {
                ball.Direction.Y *= -1;
            }

            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds/1000);

            base.Update(gameTime);
        }
    }
}

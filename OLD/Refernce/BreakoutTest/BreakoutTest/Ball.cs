using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroGameLibrary.Sprite2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace BreakoutTest
{
    class Ball  : DrawableSprite2
    {
        public Ball(Game game) : base (game)
        {

        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("ballSmall");
            this.Speed = 190;
            this.Direction = new Vector2(1, 1);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            //bounce off wall
            //Left and Right
            if ((this.Location.X + this.spriteTexture.Width > this.Game.GraphicsDevice.Viewport.Width)
                ||
                (this.Location.X < 0))
            {
                this.Direction.X *= -1;
            }
            //Top
            if ((this.Location.Y + this.spriteTexture.Height > this.Game.GraphicsDevice.Viewport.Height)
                ||
                (this.Location.Y < 0))
            {
                this.Direction.Y *= -1;
            }

            base.Update(gameTime);
        }
    }
}

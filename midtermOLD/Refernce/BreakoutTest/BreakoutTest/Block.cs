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
    class Block : DrawableSprite2
    {
        public Block(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D> ("block_blue");
            base.LoadContent();
        }
    }
}

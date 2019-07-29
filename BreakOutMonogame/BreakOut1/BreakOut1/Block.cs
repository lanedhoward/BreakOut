using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakOut1
{
    class Block : DrawableSprite
    {

        protected string blockTextureName;

        public Block(Game game)
        : base(game)
        {
            blockTextureName = "block_blue";
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>(blockTextureName);
            base.LoadContent();
        }
    }
}

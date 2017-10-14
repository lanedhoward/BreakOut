using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakoutTest
{
    class BlockManager : GameComponent
    {

        public List<Block> Blocks { get; private set; }
        int width, height;
        int margin;
        Ball ball;

        List<Block> blocksToRemove;
        bool reflected;
        
        public BlockManager(Game game, Ball b)
            : base(game)
        {
            this.Blocks = new List<Block>();
            this.blocksToRemove = new List<Block>();
            this.width = 24;
            this.height = 2;
            this.margin = 1;
            this.ball = b;
        }

        public override void Initialize()
        {
            Block b;
            //b.Initialize();
            
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    b = new Block(this.Game);
                    b.Initialize();
                    b.Location = new Vector2(5 + (w * b.SpriteTexture.Width + (w * margin)), 50 + (h * b.SpriteTexture.Height + (h * margin)));
                    Blocks.Add(b);
                }
            }
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.reflected = false;
            foreach (Block b in Blocks)
            {
                if (b.Enabled)
                {
                    b.Update(gameTime);
                    //Ball Collision
                    if (b.Intersects(ball))
                    {
                        b.Enabled = false;
                        b.Visible = false;
                        blocksToRemove.Add(b);
                        if (!reflected)
                        {
                            ball.Direction.Y *= -1;
                            this.reflected = true;
                        }
                    }
                }
            }

            //remove disaplbled blocks
            foreach (var block in blocksToRemove)
            {
                Blocks.Remove(block);
                ScoreManager.Score++;
            }
            blocksToRemove.Clear();

            base.Update(gameTime);
        }
    }
}

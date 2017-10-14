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
    class BlockManager : DrawableGameComponent
    {

        public List<Block> Blocks { get; private set; } //List of Blocks the are managed by Block Manager

        
        Ball ball;

        List<Block> blocksToRemove; //list of block to remove probably because they were hit
        
        
        public BlockManager(Game game, Ball b)
            : base(game)
        {
            this.Blocks = new List<Block>();
            this.blocksToRemove = new List<Block>();
            
            this.ball = b;
        }

        public override void Initialize()
        {
            LoadBlocks();
            base.Initialize();
        }


        
        public virtual void LoadBlocks()
        {
            Block b;
            int width, height; //widh and height of block grid measured in block
            int margin; //Margin between blocks
            width = 24; 
            height = 2;
            margin = 1;

            //Create grid of blocks
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

        }

        bool reflected; //the ball should only reflect once even if it hits two bricks
        public override void Update(GameTime gameTime)
        {
            this.reflected = false; //only reflect once per update
            UpdateCheckBlocksForCollision(gameTime);
            UpdateRemoveDisabledBlocks();

            base.Update(gameTime);
        }

        /// <summary>
        /// Removes disabled blocks from list
        /// </summary>
        private void UpdateRemoveDisabledBlocks()
        {
            //remove disabled blocks
            foreach (var block in blocksToRemove)
            {
                Blocks.Remove(block);
                ScoreManager.Score++;
            }
            blocksToRemove.Clear();
        }

        private void UpdateCheckBlocksForCollision(GameTime gameTime)
        {
            foreach (Block b in Blocks)
            {
                if (b.Enabled)
                {
                    b.Update(gameTime);
                    //Ball Collision
                    if (b.Intersects(ball)) //rectagle collision
                    {
                        //hit
                        b.Enabled = false;  //Don't update block anymore
                        b.Visible = false;  //Don't draw clock anymore
                        blocksToRemove.Add(b);  //Ball is hit add it to remove list
                        if (!reflected) //only reflect once
                        {
                            ball.Direction.Y *= -1;
                            this.reflected = true;
                        }
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            
            foreach (var block in this.Blocks)
            {
                block.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}

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
        Ball ball;
        List<Block> blocksToRemove;
        bool reflected;
        
        public BlockManager(Game game, Ball b)
            : base(game)
        {
            this.Blocks = new List<Block>();
            this.blocksToRemove = new List<Block>();
            this.ball = b;
        }

        public override void Initialize()
        {
            LoadLevel();
            
            base.Initialize();
        }

        /// <summary>
        /// Replacable Method to Load a Level by filling the Blocks List with Blocks
        /// </summary>
        protected virtual void LoadLevel()
        {
            CreateBlockArrayByWidthAndHeight(24, 2, 1);
        }

        private void CreateBlockArrayByWidthAndHeight(int width, int height, int margin)
        {
            Block b;
            //Create Block Array based on with and hieght
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

        public override void Update(GameTime gameTime)
        {
            this.reflected = false;     //Only Reflect the Ball once per update
            foreach (Block b in Blocks)
            {
                if (b.Enabled)  //Only chack active blocks
                {
                    b.Update(gameTime);//Update Block
                    //Ball Collision
                    if (b.Intersects(ball)) //Check for collision with current block
                    {
                        b.Enabled = false;
                        b.Visible = false;
                        blocksToRemove.Add(b); //Can't remove blocks from the current Array as we are interating on that collection
                                               //Add the current block to another array to be removed after the look finishes
                        if (!reflected) //Only reflect once but can break multiple blocks
                        {
                            ball.Direction.Y *= -1; //TODO side collision
                            this.reflected = true;
                        }
                    }
                }
            }

            //remove disabled blocks
            foreach (var block in blocksToRemove)
            {
                Blocks.Remove(block);
                ScoreManager.Score++;
            }
            blocksToRemove.Clear(); //empty broken blocks array

            base.Update(gameTime);
        }
    }
}

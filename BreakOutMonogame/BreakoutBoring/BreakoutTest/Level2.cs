using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakoutTest
{
    public class Level2 : BlockManager
    {
        public Level2(Game1 game, Ball b) : base(game, b)
        {

        }

        protected override void LoadLevel()
        {
            //Load something differnt
            base.LoadLevel();
        }
    }
}
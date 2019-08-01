using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{

    public enum BlockState {  Normal, Hit, Broken }

    public class Block
    {
        protected int hitCount; //Future use maybe should change state?
        protected uint blockID;

        protected static uint blockCount;

        public BlockState BlockState { get; set; }

        public Block()
        {
            this.BlockState = BlockState.Normal;
            blockCount++;
            this.blockID = blockCount;
        }

        public virtual void Hit()
        {
            this.hitCount++;
            this.UpdateBlockState();
        }

        public virtual void UpdateBlockState()
        {
            switch(this.hitCount)
            {
                case 0:
                    this.BlockState = BlockState.Normal;
                    break;
                case 1:
                    this.BlockState = BlockState.Hit;
                    break;
                case 2:
                    this.BlockState = BlockState.Broken;
                    break;
            }
            
        }
    }
}

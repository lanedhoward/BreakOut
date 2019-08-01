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
            this.updateBlockState();
        }

        protected virtual void updateBlockState()
        {
            if(this.hitCount > 0)
                this.BlockState = BlockState.Broken; //Maybe there should be more block states?
        }
    }
}

using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnityBlock : MonoBehaviour, IHitMessageTarget
{

    protected Block block;
    public Sprite NormalTexture, HitTexture;
    SpriteRenderer spriteRenderer;

    private BlockState blockstate;
    public BlockState BlockState
    {
        get { return this.block.BlockState = this.blockstate; } //encapulsate block.BlockState
        set { this.block.BlockState = this.blockstate = value; }
    }


    private void Awake()
    {
        SetupBlock();
        
    }

    protected virtual void SetupBlock()
    {
        block = new Block();
        NormalTexture = Resources.Load<Sprite>("block_blue");
        HitTexture = Resources.Load<Sprite>("block_bubble");
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        updateBlockTexture();
    }

    protected virtual void updateBlockTexture()
    {
        switch(block.BlockState)
        {
            case BlockState.Normal:
                this.gameObject.SetActive(true);
                this.spriteRenderer.sprite = NormalTexture;
                break;
            case BlockState.Hit:
                this.spriteRenderer.sprite = HitTexture;
                break;
            case BlockState.Broken:
                this.spriteRenderer.sprite = NormalTexture;
                //this.enabled = false;
                this.gameObject.SetActive(false); //don't show block
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityBlockStart();
    }

    protected virtual void UnityBlockStart()
    {
        //nothing
    }

    // Update is called once per frame
    void Update()
    {
        block.UpdateBlockState();
        UnityBlockUpdate();
        
    }

    protected virtual void UnityBlockUpdate()
    {
        updateBlockTexture();
    }

    public void Hit(BallCollision ballCollision)
    {
        this.block.Hit();
        

    }

    public void HitMessage()
    {
        this.Hit(null);
    }

    public void BrokenMessage()
    {
        throw new NotImplementedException();
    }
}

public interface IHitMessageTarget : IEventSystemHandler
{
    // functions that can be called via the messaging system
    void HitMessage();
    void BrokenMessage();
}

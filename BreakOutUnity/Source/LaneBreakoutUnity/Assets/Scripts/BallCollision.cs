using UnityEngine;
using System.Collections;
using System;

public class BallCollision : MonoBehaviour {

    
    private BallController controller;

    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;

    
    private ScoreManager sm;
    

    void Awake()
    {
        Util.GetComponentIfNull<SpriteRenderer>(this, ref spriteRenderer);
        rb2D = GetComponent<Rigidbody2D>();
        
    }

    // Use this for initialization
	void Start () {

        controller = this.GetComponent<BallController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<BallController>();
        }  
	}


    private bool reflected;
    // Update is called once per frame
    void Update () {
        reflected = false;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (controller.state == BallState.Playing)
        {
            switch (coll.gameObject.tag)
            {
                case "Player":
                    //Simple Reflection
                    //controller.RelfectY(); //reflect
                    //Better/Funner Reflections
                    controller.BounceOffPaddle(coll);
                    break;
                case "Block":
                    BlockCollision(coll);
                    break;
            }
        }
        
    }

    
    Vector3 collisionDirection;
    private void BlockCollision(Collision2D coll)
    {
        collisionDirection = (coll.gameObject.transform.position - gameObject.transform.position).normalized;

        if (Mathf.Abs(collisionDirection.y) < Mathf.Abs(collisionDirection.x))
        {
            if (collisionDirection.x > 0)
            {
                if (!reflected)
                {
                    Debug.Log("RIGHT");
                    this.controller.ReflectX();
                    Hit(coll.gameObject);
                    reflected = true;
                }

            }
            else if (collisionDirection.x < 0)
            {
                if (!reflected)
                {
                    Debug.Log("LEFT");
                    this.controller.ReflectX();
                    Hit(coll.gameObject);
                    reflected = true;

                }

            }
        }
        else
        {
            if (collisionDirection.y > 0)
            {
                if (!reflected)
                {
                    Debug.Log("TOP");
                    this.controller.ReflectY();
                    Hit(coll.gameObject);
                    reflected = true;

                }

            }
            else if (collisionDirection.y < 0)
            {
                if (!reflected)
                {
                    Debug.Log("BOTTOM");
                    this.controller.ReflectY();
                    Hit(coll.gameObject);
                    reflected = true;
                }
            }

        }
    }

    private void Hit(GameObject gameObject)
    {
        UnityBlock block = gameObject.GetComponent<UnityBlock>();
        if(block != null)
        {
            block.Hit(this);
        }
    }
}

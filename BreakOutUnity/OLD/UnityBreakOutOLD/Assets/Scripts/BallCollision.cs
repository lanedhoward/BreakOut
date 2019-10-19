using UnityEngine;
using System.Collections;

public class BallCollision : MonoBehaviour {

    
    private BallController moveBall;

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

        moveBall = this.GetComponent<BallController>();
        if (moveBall == null)
        {
            moveBall = gameObject.AddComponent<BallController>();
        }  
	}


    private bool reflected;
    // Update is called once per frame
    void Update () {
        reflected = false;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            moveBall.RelfectY();
        }

        if (coll.gameObject.tag == "Block")
        {

            Vector3 dir = (coll.gameObject.transform.position - gameObject.transform.position).normalized;

            if (Mathf.Abs(dir.y) < 0.01f)
            {
                if (dir.x > 0)
                {
                    if (!reflected)
                    {
                        Debug.Log("RIGHT");
                        this.moveBall.RelfectX();
                        Destroy(coll.gameObject);
                        reflected = true;
                    }

                }
                else if (dir.x < 0)
                {
                    if (!reflected)
                    {
                    Debug.Log("LEFT");
                    this.moveBall.RelfectX();
                        Destroy(coll.gameObject);
                        reflected = true;
                        
                    }

                }
            }
            else
            {
                if (dir.y > 0)
                {
                    if (!reflected)
                    {
                    Debug.Log("TOP");
                    this.moveBall.RelfectY();
                        Destroy(coll.gameObject);
                        reflected = true;
                        
                    }

                }
                else if (dir.y < 0)
                {
                    if (!reflected)
                    {
                        Debug.Log("BOTTOM");
                        this.moveBall.RelfectY();
                        Destroy(coll.gameObject);
                        reflected = true;
                        

                    }
                }

            }

        }
       
    }

}

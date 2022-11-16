using UnityEngine;
using System.Collections;

public enum BallState
{
    Start,
    Playing,
    Dead
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CircleCollider2D))]
public class BallController : MonoBehaviour
{
    public Vector2 Direction;
    public float Speed;

    public float paddleInfluence = 0.2f;
    public float maxPaddleInfluenceFactor = 3f;

    private Rigidbody2D rb2D;
    private CircleCollider2D cc2D;
    private SpriteRenderer spriteRenderer;

    public Vector2 paddleStartPositionOffset;

    public BallState state;

    public GameObject paddle;

    void Awake()
    {
        //defaults for Required components

        
        Util.GetComponentIfNull<SpriteRenderer>(this, ref spriteRenderer); 
        rb2D = GetComponent<Rigidbody2D>();
        cc2D = GetComponent<CircleCollider2D>();
        cc2D.radius = 0.125f;

    }

    // Use this for initialization
    void Start()
    {
        paddle = FindObjectOfType<Paddle>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// FixedUpdate uses physics
    /// </summary>
    Vector3 paddleFollow;
    bool pressedUp = false;
    void FixedUpdate()
    {
        switch (state)
        {
            case BallState.Start:
                this.spriteRenderer.forceRenderingOff = false;
                //this.transform.position.Set(paddle.transform.position.x + paddleStartPositionOffset.x, paddle.transform.position.y + paddleStartPositionOffset.y, this.transform.position.z);
                paddleFollow = Vector3.zero;
                paddleFollow.x = paddle.transform.position.x + paddleStartPositionOffset.x;
                paddleFollow.y = paddle.transform.position.y + paddleStartPositionOffset.y;
                paddleFollow.z = this.transform.position.z;
                this.transform.position = paddleFollow;

                if (Input.GetKey(KeyCode.UpArrow) && pressedUp == false)
                {
                    state = BallState.Playing;
                    pressedUp = true;
                }

                break;
            case BallState.Playing:
                if (rb2D != null)
                {
                    //Keep on screen
                    if (rb2D.position.y < Util.cameraRect.position.y)
                    {
                        Debug.Log("off the bottom");
                        ScoreManager.LoseLife();
                        state = BallState.Dead;
                    }

                    this.rb2D.position = Util.BounceOffWalls(this.transform.position,
                        spriteRenderer.bounds.size.x - 1,
                        spriteRenderer.bounds.size.y - 1, ref this.Direction);
                }

                rb2D.MovePosition(rb2D.position + Direction * Speed * Time.fixedDeltaTime);
                break;
            case BallState.Dead:
                this.spriteRenderer.forceRenderingOff = true;

                if (Input.GetKey(KeyCode.UpArrow) && pressedUp == false && ScoreManager.State == GameState.Playing)
                {
                    state = BallState.Start;
                    pressedUp = true;
                }

                break;
        }
        if (!Input.GetKey(KeyCode.UpArrow)) pressedUp = false;
    }

    /// <summary>
    /// Adds a bit of randomness to the ball bounce collision
    /// </summary>
    public void BounceOffPaddle(Collision2D coll)
    {
        if (this.Direction.y < 0)
        {
            // only reflect off paddle if going down

            if (coll.gameObject.transform.position.x < this.transform.position.x )
            {
                // bounce off right half of paddle
                Debug.Log("Right half of paddle bounce");
                this.Direction.x += paddleInfluence;
            }
            else
            {
                // bounce off left half of paddle
                Debug.Log("Left half of paddle bounce");
                this.Direction.x -= paddleInfluence;
            }

            this.Direction.x = Mathf.Clamp(this.Direction.x, -1 - (this.paddleInfluence * this.maxPaddleInfluenceFactor), 1 + (this.paddleInfluence * this.maxPaddleInfluenceFactor));

            this.ReflectY();


        }
    }


    private float GetReflectEntropy()
    {
        return -1 + ((Random.Range(0, 3) - 1) * 0.1f); //return -.9, -1 or -1.1
    }

    public void ReflectY()
    {
        this.Direction.y *= -1;
    }

    public void ReflectX()
    {
        this.Direction.x *= -1;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anima;
    private float xDir = 0f;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float jumpForce = 12f;

    private enum MovementState{ idle, running, jumping, falling };
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite=GetComponent<SpriteRenderer>();
        anima = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Horizontal movement
        //Joystick friendly
        xDir = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xDir*moveSpeed, rb.velocity.y);

        //Jumping movement
        if(Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
        
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        //If moving right
        if(xDir > 0f)
        {
            state = MovementState.running;
            sprite.flipX=false;
        }
        //If moving left
        else if (xDir < 0f)
        {
            state=MovementState.running;
            sprite.flipX=true;
        }
        //If not moving (idle)
        else
        {
            state=MovementState.idle;
        }

        if( rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if ( rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anima.SetInteger("state", (int)state);
    }

    
}

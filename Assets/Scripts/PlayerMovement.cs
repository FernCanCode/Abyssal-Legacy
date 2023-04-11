using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anima;

    [SerializeField] private LayerMask jumpableGround;

    private bool isDashing=false;
    private bool isDoubleJumping = false;
    private bool isAttacking = false;
    private float xDir = 0f;
    private int jumpCount = 0;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float dashSpeed = 35f;
    [SerializeField] private float dashDuration =0.4f;

    private float dashTimer = 0f;

    private enum MovementState{ idle, running, jumping, falling, dashing, attacking };
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite=GetComponent<SpriteRenderer>();
        anima = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!isDashing)
        {
            //Horizontal movement
            //Joystick friendly
            xDir = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(xDir*moveSpeed, rb.velocity.y);

            //Jumping movement
            if(Input.GetButtonDown("Jump") && (IsGrounded() || jumpCount<maxJumpCount))
            {
                if(IsGrounded())
                {
                    jumpCount=0;
                }
                jumpCount++;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        //Dash
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            dashTimer=0f;
            //reset vertical velocity
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        if(isDashing)
        {
            rb.velocity = new Vector2(xDir * dashSpeed, rb.velocity.y);

            dashTimer += Time.deltaTime;

            if(dashTimer >= dashDuration)
            {
                isDashing = false;
            }
        }
        if(Input.GetButtonDown("Attack") && !isAttacking)
        {
            isAttacking = true;
        }
        
        UpdateAnimationState();
        
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if(isDashing)
        {
            state=MovementState.dashing;
            
        }
         else if(isAttacking)
        {
            state = MovementState.attacking;
            isAttacking=false;
        }
        //If moving right
        else if(xDir > 0f)
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

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    private int amountOfJumpLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;

    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    private float dashTimeLeft;
    private float lastImageXPos;
    private float lastDash= -100f;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

    private bool isfacingright = true;
    private bool isWalking;
    private bool isgrounded;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJump;
    private bool isTouchingLedge;
    private bool canClimbLedge = false;
    private bool ledgeDetected;
    //on part 8(dash)
    private bool isdashing;
    private bool knockback;

    [SerializeField]
    private Vector2 knockbackSpeed;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJump = 1;

    public float movementspeed = 10;
    public float jumpforce = 16;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;

    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;

    public float dashTime;
    public float dastSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;
    

    public Transform groundcheck;
    public Transform wallcheck;
    public Transform ledgeCheck;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpLeft = amountOfJump;

        // normalize make this vector to have magnitude 1, and keep the same direction
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        checkMovementDirection();
        UpdateAnimation();
        checkIfCanJump();
        checkIfWallSliding();
        checkJump();
        checkLedgeClimb();

        checkDash();

        CheckKnockback();
    }

    // fixed update is used for physic related function on regular interval
    private void FixedUpdate()
    {
        ApplyMovement();
        checksurrounding();
    }

    // checking the player is wall sliding
    private void checkIfWallSliding()
    {
        if(isTouchingWall && movementInputDirection == facingDirection  && rb.velocity.y < 0 && !canClimbLedge)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    public bool getDashStatus()
    {
        return isdashing;
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void checkLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if (isfacingright)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);

            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }

            canMove = false;
            canFlip = false;

            anim.SetBool("canclimbledge", canClimbLedge);
        }

        if(canClimbLedge)
        {
            transform.position = ledgePos1;

       }
    }

    public void finishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;

        ledgeDetected = false;

        anim.SetBool("canclimbledge", canClimbLedge);
    }

    // checking collider around the player
    private void checksurrounding()
    {
        //overlaycircle: check if a collider within a circular area. (center of the circle, radius of the circle, filter to check object on specific layer) 
        isgrounded = Physics2D.OverlapCircle(groundcheck.position, groundCheckRadius, whatIsGround);

        //physics2d.raycast() like laser beam that is fired from the point in space along a particular direction and on which layer
        isTouchingWall = Physics2D.Raycast(wallcheck.position, transform.right, wallCheckDistance, whatIsGround);

        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if(isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallcheck.position;
        }
    }

    
    // check whether the player can jump or not
    private void checkIfCanJump()
    {
        if(isgrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpLeft = amountOfJump;
        }

        if (isTouchingWall)
        {
            canWallJump = true;
        }
         
        if(amountOfJumpLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }

    // checking the player will flip or not, will walk or not
    private void checkMovementDirection()
    {
        if (isfacingright && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isfacingright && movementInputDirection > 0)
        {
            Flip();
        }

        //rb.velocity.x != 0 && rb.velocity.y != 0
        if (Mathf .Abs (rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    // update animation according to the state or user input
    private void UpdateAnimation()
    {
        // used to set value of Animator parameter 
        anim.SetBool("isWalking",isWalking);
        anim.SetBool("isGrounded", isgrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("iswallsliding", isWallSliding);
    }

    // check user input 
    private  void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        //  getbuttondown = button press
        // check whether to normal jump or to set time for  second jump
        if (Input.GetButtonDown("Jump"))
        {
            if( isgrounded || (amountOfJumpLeft > 0 && isTouchingWall))
            {
                normalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if(Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if(!isgrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

       
        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if(Input.GetButtonDown("Dash"))
        {

            if(Time.time >= (lastDash + dashCoolDown ))
            AttemptToDash();
        }
    }

    private void AttemptToDash()
    {
        isdashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        playerAfterImagePool.Instance.GetFromPool();
        lastImageXPos = transform.position.x;

    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    private void checkDash()
    {
        if(isdashing)
        {
            if(dashTimeLeft > 0)
            {

                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dastSpeed * facingDirection, 0);

                dashTimeLeft -= Time.deltaTime;
                    
                if(Mathf.Abs(transform.position.x - lastImageXPos ) > distanceBetweenImages)
                {
                    playerAfterImagePool.Instance.GetFromPool();
                    lastImageXPos = transform.position.x;
                }
            }


            if(dashTimeLeft <=0 || isTouchingWall)
            {
                isdashing = false;
                canMove = true;
                canFlip = true;

            }
        }
    }

    // check which type of jump
    private void checkJump()
    {
      
        if(jumpTimer > 0)
        {
            // wall jump
            if(!isgrounded && isTouchingWall && movementInputDirection !=0 && movementInputDirection != facingDirection)
            {
                wallJump();
            }
            else if (isgrounded)
            {
                normalJump();
            }
        }
        if(isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0)
        {
            if( hasWallJump && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJump = false;
            }
            else if(wallJumpTimer <= 0)
            {
                hasWallJump = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    private void normalJump()
    {
        // normal jump
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            amountOfJumpLeft--;

            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;

        }
    }

    private void wallJump()
    {
         // to climb by jumping on a single wall
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);

            isWallSliding = false;
            amountOfJumpLeft = amountOfJump;
            amountOfJumpLeft--;

            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);

            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;

            turnTimer = 0;
            canMove = true;
            canFlip = true;

            hasWallJump = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }



    // apply the player to move horizontally whether it the player is in the ground or in the air
    private void ApplyMovement()
    {
        //to slow down the player when the player is in the air and no horizontal input is not apply
        if (!isgrounded && !isWallSliding && movementInputDirection == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }

        // normal/ground movement
        else if (canMove && !knockback)
        {
            rb.velocity = new Vector2(movementspeed * movementInputDirection, rb.velocity.y);
        }

        if (isWallSliding)
        {
        
            
            if(rb.velocity.y < -wallSlidingSpeed)

            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    // flip the the charcter accordingly
    private void Flip()
    {
        if (!isWallSliding && canFlip && !knockback)
        {
            facingDirection *= -1;
            isfacingright = !isfacingright;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    // to draw circle or line using for debugging
    private void OnDrawGizmos()
    {
        // draw wireframe sphere with  center and radius
        Gizmos.DrawWireSphere(groundcheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallcheck.position, new Vector3(wallcheck.position.x+ wallCheckDistance, wallcheck.position.y, wallcheck.position.z));
    }
}

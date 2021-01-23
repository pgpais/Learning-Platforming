using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpMaxTime = 0.35f;
    [SerializeField] float jumpForce = 10f;

    [Header("Wall Movement")]
    [SerializeField] float wallSlidingSpeed;
    [SerializeField] Vector2 wallJumpForce;
    [SerializeField] float wallJumpTime = 0.35f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] float groundCheckRadius = 0.5f;
    [SerializeField] LayerMask groundLayer;

    [Header("Wall Check")]
    [SerializeField] Transform frontCheck;

    Rigidbody2D rb;
    bool isGrounded = true;
    bool isTouchingFront;
    bool wallSliding;
    bool isJumping;
    bool isWallJumping;
    float jumpTimeLimit = 0;

    float defaultGravityScale;

    #region Input Fields
        [HideInInspector]
        public Vector2 movInput;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        CheckFront();
        HandleJumping();
        HandleMovement();
        HandleWallMovement();
        
    }

    private void HandleWallMovement()
    {
        if(isTouchingFront && !isGrounded && movInput.x != 0){
            wallSliding = true;
        } else{
            wallSliding = false;
        }

        if(wallSliding){
            float velY = Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue);
            // Debug.Log(velY);
            rb.velocity = new Vector2(rb.velocity.x, velY);
        }

        if(isWallJumping){
            rb.velocity = new Vector2(wallJumpForce.x * -movInput.x, wallJumpForce.y);
        }
    }

    void CheckFront(){
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, groundCheckRadius, groundLayer);
    }

    void CheckGround(){
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    void HandleMovement(){
        if(movInput.x > 0){
            // Look right
            transform.localScale = new Vector3(1, 1, 1);
        } else if(movInput.x < 0){
            transform.localScale = new Vector3(-1, 1, 1);
        }
        rb.velocity = new Vector2(movInput.x * speed, rb.velocity.y);
    }
    
    void HandleJumping(){
        if(isGrounded && isJumping){
            jumpTimeLimit = Time.time + jumpMaxTime;
            rb.velocity = Vector2.up * jumpForce;
        } else if(isJumping){
            if(Time.time < jumpTimeLimit){
                rb.velocity = Vector2.up * jumpForce;
            }
            else{
                StopJumping();
                // Debug.Log("Stopped Jumping");
            }
        }
    }

    public void StartJumping(){
        // rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        if(isGrounded){
            isJumping = true;
            // Debug.Log("Started Jumping");
        } else if(wallSliding){
            isWallJumping = true;
            Invoke(nameof(StopWallJumping), wallJumpTime);
        }
    }

    public void StopJumping()
    {
        isJumping = false;
        // Debug.Log("Stopped Jumping");
    }

    void StopWallJumping(){
        isWallJumping = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        Gizmos.DrawWireSphere(frontCheck.position, groundCheckRadius);
    }
}

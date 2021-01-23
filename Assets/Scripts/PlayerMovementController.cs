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

    [Header("Ground Check")]
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] float groundCheckRadius = 0.5f;
    [SerializeField] LayerMask groundLayer;

    Rigidbody2D rb;
    bool isGrounded = true;
    bool isJumping = false;
    float jumpTimeLimit = 0;

    #region Input Fields
        [HideInInspector]
        public Vector2 movInput;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        HandleJumping();
        HandleMovement();
    }

    void CheckGround(){
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    void HandleMovement(){
        if(movInput.x > 0){
            // Look right
            transform.localScale = new Vector3(1, 1, 1);
        } else{
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
                isJumping = false;
                Debug.Log("Stopped Jumping");
            }
        }
    }

    public void StartJumping(){
        // rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isJumping = true;
        Debug.Log("Started Jumping");
    }

    internal void StopJumping()
    {
        isJumping = false;
        Debug.Log("Stopped Jumping");
    }
}

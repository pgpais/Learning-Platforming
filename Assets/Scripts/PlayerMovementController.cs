using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float jumpMaxTime = 0.35f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float speed = 5f;

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
        HandleJumping();
        HandleMovement();
    }

    void HandleMovement(){
        rb.velocity = new Vector2(movInput.x * speed, rb.velocity.y);
    }
    
    void HandleJumping(){
        if(isGrounded && isJumping){
            jumpTimeLimit = Time.time + jumpTimeLimit;
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

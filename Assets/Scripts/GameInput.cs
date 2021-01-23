using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class GameInput : MonoBehaviour
{
    PlayerMovementController mov;

    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(CallbackContext ctx){
        mov.movInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(CallbackContext ctx){
        if(ctx.performed){
            mov.StartJumping();
        } else if(ctx.canceled){
            mov.StopJumping();
        }
    }
}

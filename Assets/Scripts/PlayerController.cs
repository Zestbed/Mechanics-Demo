using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float moveMult = 2f;
    [SerializeField]
    [Tooltip("Starting Velocity for the jump")]
    private float startVel = 3;
    public int flashJumpCount = 1;
    public float flashJumpSpeedX = 3;
    public float flashJumpSpeedY = 1;
    public float upJumpSpeed = 3f;
    private int flashJumps;
    public bool isCrouched;
    public bool facingRight;
    public bool isGrounded;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction flashJumpAction;
    private Rigidbody2D rigid;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        flashJumpAction = InputSystem.actions.FindAction("Flash Jump");
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.gameObject.tag == "Ground") {
            isGrounded = true;
            anim.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.collider.gameObject.tag == "Ground") {
            isGrounded = false;
            anim.SetBool("isJumping", true);
        }
    }



    // Update is called once per frame
    void Update()
    {
        // Read MoveValue from input action
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        
        // Determine which way the character should be facing
        bool oldFaceRight = facingRight;
        if (moveValue.x > 0) facingRight = true;
        if(moveValue.x < 0) facingRight = false;
        
        if (oldFaceRight != facingRight) {
            transform.Rotate(new Vector3(0, 180, 0));
        }

        // Actions that can only happen on ground
        if (isGrounded) {
            // Reset flash jump count
            flashJumps = flashJumpCount;
            // Horizontal Movement
            rigid.velocity = new Vector2(moveValue.x * moveMult, rigid.velocity.y);
            // Crouch check
            if (moveValue.y < 0) Crouch();
            else UnCrouch();
        }

        
        if (Mathf.Abs(rigid.velocity.x) > 0.01 || Mathf.Abs(rigid.velocity.y) > 0.01) {
            UnCrouch();
        }

        // If pressing up, try portal
        if (moveValue.y > 0) TryPortal();


        bool jumpedThisFrame = false;
        if (jumpAction.IsPressed() && isGrounded)  {
            Jump();
            jumpedThisFrame = true;
        }

        if (((flashJumpAction.IsPressed() && !isGrounded) || (jumpAction.WasPerformedThisFrame() && !isGrounded && !jumpedThisFrame)) && flashJumps > 0) {
            
            // Flash jump
            Debug.Log("Flash Jump Engaged!");
            Vector2 forceVec = new(3, flashJumpSpeedY);
            if (facingRight) forceVec.x = flashJumpSpeedX;
            else forceVec.x = -flashJumpSpeedX;

            // Check for up Jump
            if (moveValue.y > 0 && Mathf.Abs(rigid.velocityX) < 0.1) { 
                forceVec = new Vector2(0, upJumpSpeed);    
            }

            rigid.velocity += forceVec;
            flashJumps--;
        }

        // Change Animator Values
        if (Mathf.Abs(rigid.velocity.x) > 0.01f) {
            anim.SetBool("isMoving", true);
        } else {
            anim.SetBool("isMoving", false);
        }

        

    }

    void Jump() {
        if (rigid == null) {
            Debug.Log("No Rigidbody Reference!");
            return;
        }

        rigid.velocityY = startVel;

    }

    void TryPortal() {

    }

    void UnCrouch() {
        if (isCrouched) {
            isCrouched = false;
            //float newCollSizeY = coll.size.y * 2;
            //transform.position += new Vector3(0, newCollSizeY - coll.size.y, 0);
            //coll.size *= new Vector2(1, 2f);
            anim.SetBool("isCrouching", false);
        }

    }
    
    void Crouch() {
        if (!isCrouched) {
            isCrouched = true;
            //float oldCollSizeY = coll.size.y;
            //coll.size *= new Vector2(1, 0.5f);
            //transform.position += new Vector3(0, coll.size.y - oldCollSizeY, 0);
            anim.SetBool("isCrouching", true);
        }
    }
}

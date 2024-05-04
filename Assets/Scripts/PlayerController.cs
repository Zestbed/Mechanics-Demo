using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float moveMult = 2f;
    [SerializeField]
    private float jumpHeight = 2f;
    private bool isCrouched;
    private bool facingRight;

    private bool isGrounded;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody2D rigid;
    private Animator anim;
    private BoxCollider2D coll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }


    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.gameObject.tag == "Ground") {
            isGrounded = true;
            anim.SetBool("isJumping", false);
            Debug.Log("Landed");
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
        if (moveValue.x >= 0) facingRight = true;
        else facingRight = false;
        
        // Actions that can only happen on ground
        if (isGrounded) {
            // Horizontal Movement
            rigid.velocity = new Vector2(moveValue.x * moveMult, rigid.velocity.y);
            // Crouch check
            if (moveValue.y < 0) Crouch();
            else UnCrouch();
        }

        // If pressing up, try portal
        if (moveValue.y > 0) TryPortal();

        // If pressed jump, jump (lol)
        if (jumpAction.IsPressed()) Jump();

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

        if (isGrounded) {
            //transform.position += new Vector3(0, jumpHeight, 0);
            rigid.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
        }
            

        
    }

    void TryPortal() {

    }

    void UnCrouch() {
        if (isCrouched) {
            isCrouched = false;
            float newCollSizeY = coll.size.y * 2;
            transform.position += new Vector3(0, newCollSizeY - coll.size.y, 0);
            coll.size *= new Vector2(1, 2f);
            anim.SetBool("isCrouching", false);
        }

    }
    
    void Crouch() {
        if (!isCrouched) {
            isCrouched = true;
            float oldCollSizeY = coll.size.y;
            coll.size *= new Vector2(1, 0.5f);
            transform.position += new Vector3(0, coll.size.y - oldCollSizeY, 0);
            anim.SetBool("isCrouching", true);
        }
    }
}

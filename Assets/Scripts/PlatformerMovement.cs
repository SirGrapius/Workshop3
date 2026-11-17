using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

            /////////////// INFORMATION ///////////////
// This script automatically adds a Rigidbody2D, CapsuleCollider2D and CircleCollider2D component in the inspector.
// The Rigidbody2D component should (probably) have some constraints: Freeze Rotation Z
// The Circle Collider 2D should be set to "is trigger", resized and moved to a proper position.

// The following components are also needed: Player Input
// Gravity for the project is set in Unity at Edit -> Project Settings -> Physics2D -> Gravity Y

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(CapsuleCollider2D))]
public class PlatformerMovement : MonoBehaviour
{

    [Header("Stats")]
    public readonly int maxHealth = 3;
    [SerializeField] private int health;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Vector3 startPos;
    // [SerializeField] private float gravityMultiplier = 1;    //unused
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private ScreenFade fader;
    private Rigidbody2D rb;
    private CircleCollider2D groundCheckCollider;
    private Vector2 velocity;

    [Header("Checks")]

    public bool iframes;
    public UnityEvent onAction2;
    private LayerMask groundLayer = 3; // ~0 is referring to EVERY layer. Do you want a specific layer? Serialize the variable and assign the Layer of your choice.
    private Vector2 moveInput;
    public bool controlEnabled { get; set; } = true; // You can edit this variable from Unity Events
    public bool inDialog;
    private bool dialogCooldown;
    private NPC currentNPC = null;
    // Platformer specific variables
    private bool jumpInput;
    private bool jumpReleased;
    private bool wasGrounded;
    [SerializeField] bool isGrounded;
    
    void Awake()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        
        groundCheckCollider = GetComponent<CircleCollider2D>();
        groundCheckCollider.isTrigger = true;
        fader = GameObject.FindGameObjectWithTag("ScreenFader").GetComponent<ScreenFade>();
        
        // Set gravity scale to 0 so player won't "fall" 
        rb.gravityScale = 0;
        startPos = transform.position;
        animator = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        if (!inDialog)
        {
            velocity = TranslateInputToVelocity(moveInput);
        
            // Apply jump-input:
            if (jumpInput && wasGrounded)
            {
                velocity.y = jumpForce;
                jumpInput = false;
            }
        }
        
        if (currentNPC && Keyboard.current[Key.E].wasPressedThisFrame && !dialogCooldown)
        {
            currentNPC.Talk(this);
        }
        
        if (rb.linearVelocityY < 0)
        {
            animator.SetBool("_falling", true);
        }
        
        // Check if character lost contact with ground this frame
        if (wasGrounded == true && isGrounded == false)
        {
            if (rb.linearVelocityY < 0)
            {
                animator.SetBool("_falling", true);
            }
            else
            {
                animator.SetBool("_grounded", false);
                // Has jumped. Play jump sound and/or trigger jump animation etc
            }
        }
        // Check if character gained contact with ground this frame
        else if (wasGrounded == false && isGrounded == true)
        {
            jumpReleased = false;
            animator.SetBool("_falling", false);
        }
        wasGrounded = isGrounded;
        
        // Flip sprite according to direction (if a sprite renderer has been assigned)
        if (spriteRenderer)
        {
            if (moveInput.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (moveInput.x < -0.01f)
                spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        ApplyGravity();
        rb.linearVelocity = velocity;
        
        if (isGrounded)
        {
            animator.SetBool("_grounded", true);
            animator.SetBool("_falling", false);
        }
        else
        {
            animator.SetBool("_grounded", false);
        }

        if (velocity.x != 0 || rb.linearVelocity.x != 0)
        {
            animator.SetBool("_walking", true);
        }
        else
        {
            animator.SetBool("_walking", false);
        }
    }

    private bool IsGrounded()
    {
        // Is our groundCheckCollider touching the groundLayer? If so, return the value "true"
        if (groundCheckCollider.IsTouchingLayers(groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ApplyGravity()
    {
        // Applies a set gravity for when player is grounded
        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -1.0f;
        }
        // Updates fall speed with gravity if object isn't grounded
        else
        {
            // Is Jumping upwards
            if (velocity.y > 0)
            {
                float deceleration = 1;
                if (jumpReleased) // shorter jump height when releasing the jump-key
                {
                    deceleration = 5;
                }
                // you can add a gravity multiplier here... but how?
                velocity.y += Physics2D.gravity.y * deceleration * Time.deltaTime;
            }
            // Is Falling
            else
            {
                // you can add a gravity multiplier here... but how?
                velocity.y += Physics2D.gravity.y * Time.deltaTime;
            }
        }
    }
    
    Vector2 TranslateInputToVelocity(Vector2 input)
    {
        // Make the character move along the X-axis
        return new Vector2(input.x * maxSpeed, velocity.y);
    }

    // Handle Move-input
    // This method can be triggered through the UnityEvent in PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        if (controlEnabled)
        {
            moveInput = context.ReadValue<Vector2>().normalized;
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    // Handle Jump-input
    // This method can be triggered through the UnityEvent in PlayerInput
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && controlEnabled)
        {
            Debug.Log("Jump!");
            jumpInput = true;
            jumpReleased = false;
        }

        if (context.canceled && controlEnabled)
        {
            jumpReleased = true;
            jumpInput = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NPC")
        {
            currentNPC = other.gameObject.GetComponent<NPC>();
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NPC")
        {
            currentNPC = null;
        }
    }

    public IEnumerator TakeDamage(int amount)
    {
        Color originalColor = spriteRenderer.color;
        health -= amount;
        spriteRenderer.color = new Color(255, 0, 0);
        iframes = true;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = originalColor;
        iframes = false;
        if (health <= 0)
        {
            Die();
        }
        yield return null;
    }

    private IEnumerator Die()
    {
        health = maxHealth;
        transform.position = startPos;
        StartCoroutine(fader.FadeInAndOut(3));
        inDialog = true;
        yield return new WaitForSeconds(3);
        inDialog = false;
        yield return null;
    }

    public IEnumerator DialogCooldown()
    {
        dialogCooldown = true;
        yield return new WaitForSeconds(0.5f);
        dialogCooldown = false;
        yield return null;
    }

    public void Action2(InputAction.CallbackContext context)
    {
        if (context.started && controlEnabled)
        {
            Debug.Log("Action2!");
            onAction2.Invoke();
        }
    }
}

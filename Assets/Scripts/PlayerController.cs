using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpSpeed = 6f;
    [SerializeField] private bool isGrounded = true;
    private Rigidbody2D rb;
    private float direction_x;
    private Animator animator;
    private SpriteRenderer sprite;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();   
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Attack.performed += OnAttackPerformed;
    }
   private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ValidateComponent(rb, nameof(rb));

        animator = GetComponent<Animator>();
        ValidateComponent(animator, nameof(animator));

        sprite = GetComponent<SpriteRenderer>();
        ValidateComponent(sprite, nameof(sprite));
    }

    private void ValidateComponent<T>(T component, string name)
    {
        if (component == null) Debug.LogError($"{name} is null!");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction_x * speed, rb.linearVelocity.y);
    }
    private void Update()
    {
        HandleMovementAnimations();
    }

    private void HandleMovementAnimations()
    {
        if (animator)
            animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
        if (direction_x != 0 && sprite)
            sprite.flipX = direction_x < 0;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        direction_x = ctx.ReadValue<Vector2>().x;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        direction_x = 0f;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (!isGrounded) return;
        HandleJump();
    }
    private void HandleJump()
    {
        isGrounded = false;
        if (animator) animator.SetTrigger("isJumping");
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (animator) animator.SetTrigger("isAttacking");
        AttackHit();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
    public void AttackHit()
    {
        float radius = 2f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector2 directionToEnemy = hit.transform.position - transform.position;

                if (!sprite.flipX && directionToEnemy.x > 0)
                {
                    hit.GetComponent<EnemyController>().Die();
                }
                else if (sprite.flipX && directionToEnemy.x < 0)
                {
                    hit.GetComponent<EnemyController>().Die();
                }
            }
        }
    }
    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Disable();
    }
}

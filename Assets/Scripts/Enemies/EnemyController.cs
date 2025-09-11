using UnityEngine;
using System.Collections;


public class EnemyController : MonoBehaviour
{
    [SerializeField]  float speed = 3f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] float delayForDestroy = 1f;
    private Transform player;
    private Animator animator;
    SpriteRenderer sprite;
    float animationDelay = 0.3f;
    private bool isDead = false;
    private bool touchedGround = false;
    float facingDirectionX = 1f , originalScaleY = 1 , originalScaleZ = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        ValidateComponent(sprite, nameof(sprite));
        animator = GetComponent<Animator>();
        ValidateComponent(animator, nameof(animator));
    }

    private void ValidateComponent<T>(T component, string name)
    {
        if (component == null) Debug.LogError($"{name} is null!");
    }

    void Update()
    {
        if (player == null) return;
        bool isMoving = Vector2.Distance(transform.position, player.position) > chaseRange;
        Movement(isMoving);
    }

    private void Movement(bool isMoving)
    {
        if (animator != null && touchedGround)
        {
            animator.SetBool("IsRunning", isMoving);
        }

        
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            if (player.position.x < transform.position.x)
                transform.localScale = new Vector3(- facingDirectionX, originalScaleY, originalScaleZ);
            else
                transform.localScale = new Vector3(facingDirectionX, originalScaleY, originalScaleZ);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Ground"))
        {
            touchedGround = true;
            
        }
        if (other.gameObject.CompareTag("Player") && !isDead)
        {
            Events.PlayerHP?.Invoke(damage);
        }

    }

    public void Die()
    {
        isDead = true;
        Events.EnemyKilled?.Invoke();
        StartCoroutine(EnemyDeath());
    }

    IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(animationDelay);
        
            sprite.color = Color.red;
            animator.SetTrigger("IsHit");
            isDead = true;
        Destroy(gameObject , delayForDestroy);  
    }
}


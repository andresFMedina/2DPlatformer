using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;

    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    [Header("Pierce Info")]
    [SerializeField] private int pierceAmount;
    private int originalPierceAmount;


    [Header("Bounce Info")]
    [SerializeField] private float bounceSpeed = 20;
    private bool isBouncing;
    private bool isBouncingEnabled;
    private int bounceAmount;
    private int originalAmountOfBounce = 4;
    private List<Transform> enemyTargets = new();
    private int currentTargetIndex;

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void SetupBounce(bool isBouncing, int bounceAmount)
    {
        this.isBouncing = isBouncing;
        isBouncingEnabled = isBouncing;
        this.bounceAmount = bounceAmount;
    }

    public void SetupPierce(int pierceAmount)
    {
        this.pierceAmount = pierceAmount;
        originalPierceAmount = 4;
    }

    public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float hitCooldown)
    {
        this.isSpinning = isSpinning;
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;
    }

    public void SetupSword(Vector2 direction, float gravityScale, Player player)
    {
        rb.velocity = direction;
        rb.gravityScale = gravityScale;
        this.player = player;
        circleCollider.enabled = true;

        if (pierceAmount <= 0)
            animator.SetBool("IsRotating", true);
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                CatchSword();
            }
        }

        Bounce();
        Spin();
    }

    private void Spin()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhileSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        Enemy enemy = hit.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            enemy.Damage();
                        }
                    }
                }
            }
        }
    }

    private void StopWhileSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void CatchSword()
    {
        player.stateMachine.ChangeState(player.catchSwordState);
        gameObject.SetActive(false);
        player.sword = null;
        isReturning = false;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.None;
        bounceAmount = originalAmountOfBounce;
        pierceAmount = originalPierceAmount;
        isBouncing = isBouncingEnabled;
    }

    private void Bounce()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[currentTargetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTargets[currentTargetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTargets[currentTargetIndex].GetComponent<Enemy>());

                currentTargetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (currentTargetIndex >= enemyTargets.Count)
                {
                    currentTargetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            SwordSkillDamage(enemy);
        }
        SetupTargetsForBounce(enemy);

        StuckInto(collision);
    }

    private static void SwordSkillDamage(Enemy enemy)
    {
        enemy?.Damage();
        enemy.StartCoroutine("FreezeTimeFor", .7f);
    }

    private void SetupTargetsForBounce(Enemy enemy)
    {
        if (enemy != null)
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTargets.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhileSpinning();
            return;
        }

        canRotate = false;
        circleCollider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTargets.Count > 0)
            return;

        animator.SetBool("IsRotating", false);
        transform.parent = collision.transform;
    }
}

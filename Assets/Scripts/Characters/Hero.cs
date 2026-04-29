// 球之积分勇者 - 勇者角色控制器
// 玩家直接控制的角色，支持移动、跳跃、攻击

using UnityEngine;

namespace JingXing
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Hero : MonoBehaviour
    {
        [Header("属性")]
        public int health = 100;
        public int maxHealth = 100;
        public int attackDamage = 30;
        public float attackRange = 1.5f;
        public float attackCooldown = 0.3f;
        public float invincibleDuration = 1f;

        [Header("视觉")]
        public SpriteRenderer spriteRenderer;
        public Color heroColor = new(1f, 0.78f, 0.59f); // 肤色

        // 组件引用
        private Rigidbody2D rb;
        private BoxCollider2D boxCollider;

        // 状态
        private bool isGrounded = false;
        private bool facingRight = true;
        private float lastAttackTime = -1f;
        private float invincibleTimer = 0f;
        private bool isAlive = true;

        // 地面检测
        [Header("地面检测")]
        public Transform groundCheck;
        public float groundCheckRadius = 0.1f;
        public LayerMask groundLayer;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void Start()
        {
            // 设置勇者引用
            if (GameManager.Instance != null)
            {
                GameManager.Instance.hero = this;
            }
        }

        private void Update()
        {
            if (!isAlive) return;
            if (GameManager.Instance != null && GameManager.Instance.currentState != GameConstants.GameState.Playing)
                return;

            // 地面检测
            if (groundCheck != null)
            {
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            }

            // 无敌时间
            if (invincibleTimer > 0)
            {
                invincibleTimer -= Time.deltaTime;
                // 闪烁效果
                if (spriteRenderer != null)
                {
                    float alpha = (Mathf.FloorToInt(invincibleTimer * 10) % 2 == 0) ? 0.3f : 1f;
                    Color c = spriteRenderer.color;
                    c.a = alpha;
                    spriteRenderer.color = c;
                }
            }
            else if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = 1f;
                spriteRenderer.color = c;
            }

            // 移动输入
            HandleMovement();

            // 跳跃输入
            HandleJump();

            // 攻击输入
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
            {
                PerformAttack();
            }

            // 掉出世界检测
            if (transform.position.y < -10)
            {
                TakeDamage(100);
            }
        }

        private void HandleMovement()
        {
            float moveInput = 0f;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                moveInput = -1f;
                facingRight = false;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveInput = 1f;
                facingRight = true;
            }

            rb.velocity = new Vector2(moveInput * GameConstants.PlayerSpeed, rb.velocity.y);

            // 翻转精灵
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = !facingRight;
            }
        }

        private void HandleJump()
        {
            // 飞行模式
            if (GameManager.Instance != null && GameManager.Instance.flyingActive)
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
                {
                    rb.velocity = new Vector2(rb.velocity.x, GameConstants.PlayerJumpForce * 0.7f);
                }
                return;
            }

            // 普通跳跃
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, GameConstants.PlayerJumpForce);
                isGrounded = false;
            }
        }

        public void PerformAttack()
        {
            if (Time.time - lastAttackTime < attackCooldown) return;

            lastAttackTime = Time.time;

            // 攻击方向
            Vector2 attackDir = facingRight ? Vector2.right : Vector2.left;
            Vector2 attackPos = (Vector2)transform.position + attackDir * 0.5f;

            // 检测攻击范围内的敌人
            RaycastHit2D[] hits = Physics2D.BoxCastAll(attackPos, new Vector2(attackRange, 1f), 0f, attackDir, attackRange * 0.5f);

            foreach (var hit in hits)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null && enemy.IsAlive())
                {
                    enemy.TakeDamage(attackDamage);
                    // 粒子效果
                    if (ParticleManager.Instance != null)
                    {
                        ParticleManager.Instance.SpawnHitEffect(enemy.transform.position);
                    }
                    // 加分
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.AddScore(10);
                    }
                }
            }
        }

        public void TakeDamage(int amount)
        {
            if (invincibleTimer > 0) return;

            health -= amount;
            invincibleTimer = invincibleDuration;

            // 更新UI
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealth(health, maxHealth);
            }

            if (health <= 0)
            {
                health = 0;
                isAlive = false;
                Die();
            }
        }

        public void Heal(int amount)
        {
            health = Mathf.Min(health + amount, maxHealth);
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealth(health, maxHealth);
            }
        }

        private void Die()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }

        public bool IsFacingRight() => facingRight;
        public bool IsAlive() => isAlive;
        public bool IsGrounded() => isGrounded;

        // 可视化地面检测范围
        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
}

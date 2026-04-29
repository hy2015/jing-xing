// 球之积分勇者 - 敌人基类
// 所有敌人的基类，提供巡逻、受伤、状态效果等通用逻辑

using UnityEngine;

namespace JingXing
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Enemy : MonoBehaviour
    {
        [Header("敌人属性")]
        public int health = 60;
        public int maxHealth = 60;
        public int damage = 10;
        public float moveSpeed = 1.5f;
        public float patrolRange = 2f;

        [Header("视觉")]
        public SpriteRenderer spriteRenderer;
        public Color enemyColor = Color.red;

        // 组件引用
        protected Rigidbody2D rb;
        protected BoxCollider2D boxCollider;

        // 状态
        protected bool isAlive = true;
        protected bool facingRight = true;
        protected float startX;
        protected int direction = 1;

        // 状态效果
        protected float frozenTimer = 0f;
        protected float trappedTimer = 0f;
        protected float vineWrappedTimer = 0f;
        protected bool isControlled = false;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        protected virtual void Start()
        {
            startX = transform.position.x;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = enemyColor;
            }
        }

        protected virtual void Update()
        {
            if (!isAlive) return;

            // 状态效果计时
            UpdateStatusEffects();

            // 如果被控制/冻结/困住，不移动
            if (frozenTimer > 0 || trappedTimer > 0 || vineWrappedTimer > 0)
            {
                return;
            }

            // 巡逻移动
            Patrol();
        }

        // 更新状态效果
        protected virtual void UpdateStatusEffects()
        {
            if (frozenTimer > 0)
            {
                frozenTimer -= Time.deltaTime;
                // 冻结视觉效果
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.Lerp(enemyColor, Color.cyan, 0.5f);
                }
            }
            else if (trappedTimer > 0)
            {
                trappedTimer -= Time.deltaTime;
            }
            else if (vineWrappedTimer > 0)
            {
                vineWrappedTimer -= Time.deltaTime;
                if (vineWrappedTimer <= 0)
                {
                    isControlled = false;
                }
            }
            else
            {
                // 恢复正常颜色
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = enemyColor;
                }
            }
        }

        // 巡逻逻辑
        protected virtual void Patrol()
        {
            // 移动
            float moveX = moveSpeed * direction * Time.deltaTime;
            transform.position += new Vector3(moveX, 0, 0);

            // 检查巡逻边界
            if (Mathf.Abs(transform.position.x - startX) > patrolRange)
            {
                direction *= -1;
            }

            // 更新朝向
            facingRight = direction > 0;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = !facingRight;
            }
        }

        // 受伤
        public virtual void TakeDamage(int amount)
        {
            if (!isAlive) return;

            health -= amount;

            // 受伤闪烁效果
            if (spriteRenderer != null)
            {
                StartCoroutine(FlashDamage());
            }

            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        // 死亡
        protected virtual void Die()
        {
            isAlive = false;

            // 死亡粒子效果
            if (ParticleManager.Instance != null)
            {
                ParticleManager.Instance.SpawnDeathEffect(transform.position);
            }

            // 加分
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(20);
            }

            // 销毁对象
            Destroy(gameObject, 0.5f);
        }

        // 受伤闪烁协程
        private System.Collections.IEnumerator FlashDamage()
        {
            if (spriteRenderer == null) yield break;

            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }

        // 状态效果方法
        public void Freeze(float duration)
        {
            frozenTimer = duration;
        }

        public void Trap(float duration)
        {
            trappedTimer = duration;
        }

        public void VineWrap(float duration)
        {
            vineWrappedTimer = duration;
            isControlled = true;
        }

        // 碰撞检测 (伤害勇者)
        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            if (!isAlive) return;
            if (frozenTimer > 0 || vineWrappedTimer > 0) return;

            Hero hero = collision.gameObject.GetComponent<Hero>();
            if (hero != null)
            {
                hero.TakeDamage(damage);
            }
        }

        // 公共方法
        public bool IsAlive() => isAlive;
        public bool IsFrozen() => frozenTimer > 0;
        public bool IsTrapped() => trappedTimer > 0;
        public bool IsControlled() => isControlled;
        public bool IsVineWrapped() => vineWrappedTimer > 0;
    }
}

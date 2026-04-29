// 球之积分勇者 - 随从基类
// 所有随从的基类，提供跟随逻辑和能力接口

using UnityEngine;

namespace JingXing
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class FollowerBase : MonoBehaviour
    {
        [Header("随从设置")]
        public float followDistance = 2f;
        public float followSpeed = 5f;
        public float followHeight = 0.5f;

        [Header("视觉")]
        public SpriteRenderer spriteRenderer;

        // 组件引用
        protected Rigidbody2D rb;
        protected BoxCollider2D boxCollider;

        // 状态
        protected bool facingRight = true;
        protected bool isActive = false;

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
            // 注册到GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.followers.Add(this);
            }
        }

        protected virtual void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.hero == null) return;

            // 跟随勇者
            FollowHero();

            // 更新朝向
            UpdateFacing();
        }

        protected virtual void FollowHero()
        {
            Hero hero = GameManager.Instance.hero;
            if (hero == null) return;

            // 计算目标位置 (在勇者身后)
            int followerIndex = GameManager.Instance.followers.IndexOf(this);
            float targetX = hero.transform.position.x - (followerIndex + 1) * followDistance;
            float targetY = hero.transform.position.y + followHeight;

            // 平滑移动
            Vector2 targetPos = new(targetX, targetY);
            Vector2 currentPos = transform.position;
            float distance = Vector2.Distance(currentPos, targetPos);

            if (distance > 0.1f)
            {
                float speed = Mathf.Min(followSpeed, distance * 3f);
                transform.position = Vector2.Lerp(currentPos, targetPos, speed * Time.deltaTime);
            }
        }

        protected virtual void UpdateFacing()
        {
            if (GameManager.Instance.hero == null) return;

            float dirX = GameManager.Instance.hero.transform.position.x - transform.position.x;
            if (dirX > 0.1f) facingRight = true;
            else if (dirX < -0.1f) facingRight = false;

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = !facingRight;
            }
        }

        // 设置是否为当前激活的随从
        public void SetActive(bool active)
        {
            isActive = active;
            // 可以在这里添加高亮效果
            if (spriteRenderer != null)
            {
                // 简单的高亮：激活时增加亮度
                spriteRenderer.color = active ? Color.white : new Color(0.8f, 0.8f, 0.8f);
            }
        }

        // 获取显示名称
        public abstract string GetDisplayName();

        // 获取当前能力描述
        public abstract string GetAbilityDescription();

        // 使用能力
        public abstract void UseAbility();

        // 高亮当前随从 (由GameManager调用)
        public void Highlight(bool highlight)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = highlight ? Color.yellow : Color.white;
            }
        }
    }
}

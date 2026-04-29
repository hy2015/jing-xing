// 球之积分勇者 - 蜜蜂敌人
// 空中飞行型敌人

using UnityEngine;

namespace JingXing
{
    public class Bee : Enemy
    {
        [Header("蜜蜂设置")]
        public float flySpeed = 2f;
        public float floatAmplitude = 1f;
        public float floatFrequency = 1f;

        private float floatTimer = 0f;
        private Vector2 startPosition;

        protected override void Start()
        {
            base.Start();
            health = 30;
            maxHealth = 30;
            moveSpeed = flySpeed;
            damage = 8;
            patrolRange = 4f;
            enemyColor = new Color(1f, 0.78f, 0f); // 黄色
            startPosition = transform.position;

            // 蜜蜂不受重力影响
            if (rb != null)
            {
                rb.gravityScale = 0;
            }
        }

        protected override void Patrol()
        {
            // 上下浮动
            floatTimer += Time.deltaTime * floatFrequency;
            float yOffset = Mathf.Sin(floatTimer) * floatAmplitude;

            // 水平移动
            float moveX = moveSpeed * direction * Time.deltaTime;
            transform.position = new Vector3(
                transform.position.x + moveX,
                startPosition.y + yOffset,
                transform.position.z
            );

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
    }
}

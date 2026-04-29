// 球之积分勇者 - 甲虫敌人
// 地面巡逻型敌人

using UnityEngine;

namespace JingXing
{
    public class Beetle : Enemy
    {
        [Header("甲虫设置")]
        public float patrolSpeed = 1.2f;

        protected override void Start()
        {
            base.Start();
            health = 60;
            maxHealth = 60;
            moveSpeed = patrolSpeed;
            damage = 10;
            patrolRange = 3f;
            enemyColor = new Color(0.31f, 0.24f, 0.12f); // 棕色
        }

        // 甲虫特殊巡逻：地面行走，遇边缘折返
        protected override void Patrol()
        {
            // 检查前方是否有地面
            Vector2 checkPos = (Vector2)transform.position + (facingRight ? Vector2.right : Vector2.left) * 0.5f;
            RaycastHit2D groundHit = Physics2D.Raycast(checkPos, Vector2.down, 1f);

            // 如果前方没有地面，折返
            if (groundHit.collider == null)
            {
                direction *= -1;
            }

            base.Patrol();
        }
    }
}

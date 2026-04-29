// 球之积分勇者 - 蚂蚁敌人
// 群体行动型敌人

using UnityEngine;

namespace JingXing
{
    public class Ant : Enemy
    {
        [Header("蚂蚁设置")]
        public bool isLeader = false;
        public float groupFollowRange = 3f;

        private Ant leader;
        private System.Collections.Generic.List<Ant> group = new();

        protected override void Start()
        {
            base.Start();

            if (isLeader)
            {
                health = 40;
                maxHealth = 40;
                moveSpeed = 1.8f;
                damage = 12;
                enemyColor = new Color(0.47f, 0.24f, 0.10f); // 深棕色
            }
            else
            {
                health = 20;
                maxHealth = 20;
                moveSpeed = 2.5f;
                damage = 8;
                enemyColor = new Color(0.39f, 0.20f, 0.08f); // 浅棕色
            }

            patrolRange = 2f;

            // 寻找附近的领袖
            if (!isLeader)
            {
                FindLeader();
            }
        }

        private void FindLeader()
        {
            // 寻找附近的领袖蚂蚁
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, groupFollowRange);
            foreach (var collider in colliders)
            {
                Ant otherAnt = collider.GetComponent<Ant>();
                if (otherAnt != null && otherAnt != this && otherAnt.isLeader)
                {
                    leader = otherAnt;
                    leader.AddToGroup(this);
                    break;
                }
            }
        }

        public void AddToGroup(Ant ant)
        {
            if (!group.Contains(ant))
            {
                group.Add(ant);
            }
        }

        protected override void Patrol()
        {
            if (!isLeader && leader != null && leader.IsAlive())
            {
                // 跟随领袖
                Vector2 leaderPos = leader.transform.position;
                float followX = leaderPos.x + (group.IndexOf(this) + 1) * 0.8f * (leader.facingRight ? -1 : 1);

                Vector2 targetPos = new(followX, transform.position.y);
                transform.position = Vector2.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

                // 更新朝向
                facingRight = leader.facingRight;
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = !facingRight;
                }
            }
            else
            {
                // 独立巡逻
                base.Patrol();
            }
        }

        // 领袖死亡时，群体成员变为独立
        protected override void Die()
        {
            if (isLeader)
            {
                foreach (var ant in group)
                {
                    if (ant != null)
                    {
                        ant.leader = null;
                    }
                }
            }
            base.Die();
        }
    }
}

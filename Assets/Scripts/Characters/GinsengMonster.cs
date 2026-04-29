// 球之积分勇者 - 绿色人参怪
// 缠绕控制敌人 + 微积分挑战

using UnityEngine;

namespace JingXing
{
    public class GinsengMonster : FollowerBase
    {
        [Header("人参怪设置")]
        public float vineRange = 4f;
        public float vineDuration = 3f;
        public float abilityCooldown = 2f;

        [Header("缠绕效果")]
        public GameObject vineEffectPrefab;

        private float lastAbilityTime = -1f;
        private Enemy controlledEnemy = null;

        protected override void Start()
        {
            base.Start();
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(0.24f, 0.70f, 0.44f); // 绿色
            }
        }

        public override string GetDisplayName()
        {
            return "绿色人参怪";
        }

        public override string GetAbilityDescription()
        {
            return "[F] 缠绕控制+微积分挑战";
        }

        // 使用能力
        public override void UseAbility()
        {
            if (Time.time - lastAbilityTime < abilityCooldown) return;
            lastAbilityTime = Time.time;

            Hero hero = GameManager.Instance.hero;
            if (hero == null) return;

            // 寻找最近的敌人
            Enemy closestEnemy = FindClosestEnemy(hero.transform.position);

            if (closestEnemy != null)
            {
                // 缠绕控制敌人
                ControlEnemy(closestEnemy);
                GameManager.Instance.ShowMessage("缠绕控制敌人!");
            }

            // 触发微积分挑战
            TriggerCalculusChallenge();
        }

        // 寻找最近的敌人
        private Enemy FindClosestEnemy(Vector2 position)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, vineRange);
            Enemy closest = null;
            float closestDist = float.MaxValue;

            foreach (var collider in colliders)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null && enemy.IsAlive() && !enemy.IsControlled())
                {
                    float dist = Vector2.Distance(position, enemy.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = enemy;
                    }
                }
            }

            return closest;
        }

        // 缠绕控制敌人
        private void ControlEnemy(Enemy enemy)
        {
            enemy.VineWrap(vineDuration);
            controlledEnemy = enemy;

            // 生成缠绕效果
            if (vineEffectPrefab != null)
            {
                Instantiate(vineEffectPrefab, enemy.transform.position, Quaternion.identity);
            }

            if (ParticleManager.Instance != null)
            {
                ParticleManager.Instance.SpawnVineEffect(enemy.transform.position);
            }
        }

        // 触发微积分挑战
        private void TriggerCalculusChallenge()
        {
            if (CalculusChallenge.Instance != null)
            {
                CalculusChallenge.Instance.StartChallenge();
            }
        }

        // 被控制的敌人死亡时清理
        private void Update()
        {
            base.Update();

            if (controlledEnemy != null && !controlledEnemy.IsAlive())
            {
                controlledEnemy = null;
            }
        }
    }
}

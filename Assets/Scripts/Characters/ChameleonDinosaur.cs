// 球之积分勇者 - 变色恐龙
// 9种颜色，每种颜色有不同能力

using UnityEngine;
using System.Collections.Generic;

namespace JingXing
{
    public class ChameleonDinosaur : FollowerBase
    {
        [Header("变色恐龙设置")]
        public int currentColorIndex = 0;
        public float abilityRange = 2f;
        public float abilityCooldown = 1f;

        private float lastAbilityTime = -1f;

        protected override void Start()
        {
            base.Start();
            UpdateColorVisual();
        }

        public override string GetDisplayName()
        {
            return $"变色恐龙 ({GameConstants.ChameleonColorNames[currentColorIndex]})";
        }

        public override string GetAbilityDescription()
        {
            return GameConstants.ChameleonColorNames[currentColorIndex];
        }

        // 切换颜色
        public void CycleColor(bool forward)
        {
            if (forward)
            {
                currentColorIndex = (currentColorIndex + 1) % 9;
            }
            else
            {
                currentColorIndex = (currentColorIndex - 1 + 9) % 9;
            }
            UpdateColorVisual();
        }

        // 更新颜色视觉
        private void UpdateColorVisual()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = GameConstants.ChameleonColors[currentColorIndex];
            }
        }

        // 获取当前颜色名称
        public string GetColorName()
        {
            return GameConstants.ChameleonColorNames[currentColorIndex];
        }

        // 使用能力
        public override void UseAbility()
        {
            if (Time.time - lastAbilityTime < abilityCooldown) return;
            lastAbilityTime = Time.time;

            Hero hero = GameManager.Instance.hero;
            if (hero == null) return;

            GameConstants.ChameleonColor colorType = (GameConstants.ChameleonColor)currentColorIndex;

            switch (colorType)
            {
                case GameConstants.ChameleonColor.Red:
                    UseStrengthAbility(hero);
                    break;
                case GameConstants.ChameleonColor.Orange:
                    UseFireAbility(hero);
                    break;
                case GameConstants.ChameleonColor.Yellow:
                    UseLightAbility(hero);
                    break;
                case GameConstants.ChameleonColor.Green:
                    UseMagnetAbility();
                    break;
                case GameConstants.ChameleonColor.LightBlue:
                    UseLightningAbility(hero);
                    break;
                case GameConstants.ChameleonColor.DeepBlue:
                    UseDivingAbility(hero);
                    break;
                case GameConstants.ChameleonColor.Purple:
                    UseFlyingAbility();
                    break;
                case GameConstants.ChameleonColor.White:
                    UseIceAbility(hero);
                    break;
                case GameConstants.ChameleonColor.Black:
                    UseBlackHoleAbility(hero);
                    break;
            }
        }

        // 红色 - 巨力破墙
        private void UseStrengthAbility(Hero hero)
        {
            Vector2 attackDir = hero.IsFacingRight() ? Vector2.right : Vector2.left;
            Vector2 attackPos = (Vector2)hero.transform.position + attackDir * 1f;

            RaycastHit2D[] hits = Physics2D.BoxCastAll(attackPos, new Vector2(2f, 1f), 0f, attackDir, 1f);

            foreach (var hit in hits)
            {
                BreakableBlock block = hit.collider.GetComponent<BreakableBlock>();
                if (block != null)
                {
                    block.Destroy();
                    if (ParticleManager.Instance != null)
                    {
                        ParticleManager.Instance.SpawnBreakEffect(block.transform.position);
                    }
                }
            }

            GameManager.Instance.ShowMessage("巨力破墙!");
        }

        // 橙色 - 火焰烧毁障碍物
        private void UseFireAbility(Hero hero)
        {
            Vector2 attackDir = hero.IsFacingRight() ? Vector2.right : Vector2.left;
            Vector2 attackPos = (Vector2)hero.transform.position + attackDir * 1.2f;

            RaycastHit2D[] hits = Physics2D.BoxCastAll(attackPos, new Vector2(2.5f, 1f), 0f, attackDir, 1.2f);

            foreach (var hit in hits)
            {
                Obstacle obstacle = hit.collider.GetComponent<Obstacle>();
                if (obstacle != null && obstacle.obstacleType == Obstacle.ObstacleType.Thorns)
                {
                    obstacle.Destroy();
                    if (ParticleManager.Instance != null)
                    {
                        ParticleManager.Instance.SpawnFireEffect(obstacle.transform.position);
                    }
                }
            }

            GameManager.Instance.ShowMessage("火焰燃烧!");
        }

        // 黄色 - 光剑攻击
        private void UseLightAbility(Hero hero)
        {
            // 大范围光剑攻击
            Collider2D[] enemies = Physics2D.OverlapCircleAll(hero.transform.position, 3f);

            foreach (var collider in enemies)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null && enemy.IsAlive())
                {
                    enemy.TakeDamage(60);
                    if (ParticleManager.Instance != null)
                    {
                        ParticleManager.Instance.SpawnLightEffect(enemy.transform.position);
                    }
                }
            }

            GameManager.Instance.ShowMessage("光剑斩击! 照亮+攻击!");
        }

        // 绿色 - 磁力吸引
        private void UseMagnetAbility()
        {
            GameManager.Instance.ActivateMagnet(3f);
        }

        // 浅蓝 - 闪电
        private void UseLightningAbility(Hero hero)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(hero.transform.position, 4f);

            foreach (var collider in enemies)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null && enemy.IsAlive())
                {
                    enemy.TakeDamage(50);
                    if (ParticleManager.Instance != null)
                    {
                        ParticleManager.Instance.SpawnLightningEffect(enemy.transform.position);
                    }
                }
            }

            GameManager.Instance.ShowMessage("闪电链! 电击敌人!");
        }

        // 深海色 - 高压水炮
        private void UseDivingAbility(Hero hero)
        {
            Vector2 attackDir = hero.IsFacingRight() ? Vector2.right : Vector2.left;
            Vector2 attackPos = (Vector2)hero.transform.position + attackDir * 1.5f;

            RaycastHit2D[] hits = Physics2D.BoxCastAll(attackPos, new Vector2(4f, 0.5f), 0f, attackDir, 2f);

            foreach (var hit in hits)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null && enemy.IsAlive())
                {
                    enemy.TakeDamage(40);
                    // 击退效果
                    Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        enemyRb.AddForce(attackDir * 10f, ForceMode2D.Impulse);
                    }
                    if (ParticleManager.Instance != null)
                    {
                        ParticleManager.Instance.SpawnWaterEffect(enemy.transform.position);
                    }
                }
            }

            GameManager.Instance.ShowMessage("高压水炮! 击退敌人!");
        }

        // 紫色 - 飞翔
        private void UseFlyingAbility()
        {
            GameManager.Instance.ActivateFlying(3f);
        }

        // 白色 - 寒冰/绝对零度
        private void UseIceAbility(Hero hero)
        {
            bool absoluteZero = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float range = absoluteZero ? 6f : 3.5f;

            Collider2D[] enemies = Physics2D.OverlapCircleAll(hero.transform.position, range);

            foreach (var collider in enemies)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null && enemy.IsAlive())
                {
                    if (absoluteZero)
                    {
                        // 绝对零度 - 秒杀
                        enemy.TakeDamage(9999);
                        if (ParticleManager.Instance != null)
                        {
                            ParticleManager.Instance.SpawnAbsoluteZeroEffect(enemy.transform.position);
                        }
                    }
                    else
                    {
                        // 普通冻结
                        enemy.Freeze(3f);
                        if (ParticleManager.Instance != null)
                        {
                            ParticleManager.Instance.SpawnIceEffect(enemy.transform.position);
                        }
                    }
                }
            }

            GameManager.Instance.ShowMessage(absoluteZero ? "绝对零度! 万物冻结秒杀!" : "寒冰冻结!");
        }

        // 黑色 - 黑洞引力
        private void UseBlackHoleAbility(Hero hero)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(hero.transform.position, 5f);

            foreach (var collider in enemies)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null && enemy.IsAlive())
                {
                    // 向勇者方向吸引
                    Vector2 direction = (hero.transform.position - enemy.transform.position).normalized;
                    Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        enemyRb.AddForce(direction * 8f, ForceMode2D.Impulse);
                    }
                    enemy.TakeDamage(15);
                    if (ParticleManager.Instance != null)
                    {
                        ParticleManager.Instance.SpawnBlackHoleEffect(enemy.transform.position);
                    }
                }
            }

            GameManager.Instance.ShowMessage("黑洞引力! 吸引敌人!");
        }
    }
}

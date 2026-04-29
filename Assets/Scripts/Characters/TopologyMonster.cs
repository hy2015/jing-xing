// 球之积分勇者 - 拓扑变形怪
// 6种拓扑形态，每种形态有不同能力

using UnityEngine;
using System.Collections.Generic;

namespace JingXing
{
    public class TopologyMonster : FollowerBase
    {
        [Header("拓扑怪设置")]
        public int currentFormIndex = 0;
        public float abilityCooldown = 1.5f;

        [Header("传送门")]
        public GameObject portalPrefab;
        private List<Vector2> portalPositions = new();
        private List<GameObject> portalObjects = new();

        private float lastAbilityTime = -1f;

        protected override void Start()
        {
            base.Start();
            UpdateFormVisual();
        }

        public override string GetDisplayName()
        {
            return $"拓扑变形怪 ({GameConstants.TopologyFormNames[currentFormIndex]})";
        }

        public override string GetAbilityDescription()
        {
            return GameConstants.TopologyFormNames[currentFormIndex];
        }

        // 切换形态
        public void CycleForm(bool forward)
        {
            if (forward)
            {
                currentFormIndex = (currentFormIndex + 1) % 6;
            }
            else
            {
                currentFormIndex = (currentFormIndex - 1 + 6) % 6;
            }
            UpdateFormVisual();
        }

        // 更新形态视觉
        private void UpdateFormVisual()
        {
            if (spriteRenderer == null) return;

            // 根据形态改变颜色和形状提示
            switch ((GameConstants.TopologyForm)currentFormIndex)
            {
                case GameConstants.TopologyForm.Sphere:
                    spriteRenderer.color = GameConstants.PurpleColor;
                    break;
                case GameConstants.TopologyForm.Torus:
                    spriteRenderer.color = new Color(0.7f, 0.3f, 0.9f);
                    break;
                case GameConstants.TopologyForm.Torus2:
                    spriteRenderer.color = new Color(0.5f, 0.2f, 0.8f);
                    break;
                case GameConstants.TopologyForm.Torus3:
                    spriteRenderer.color = new Color(0.4f, 0.1f, 0.7f);
                    break;
                case GameConstants.TopologyForm.Mobius:
                    spriteRenderer.color = new Color(0.8f, 0.4f, 1f);
                    break;
                case GameConstants.TopologyForm.Klein:
                    spriteRenderer.color = new Color(0.3f, 0.1f, 0.5f);
                    break;
            }
        }

        // 获取当前形态名称
        public string GetFormName()
        {
            return GameConstants.TopologyFormNames[currentFormIndex];
        }

        // 使用能力
        public override void UseAbility()
        {
            if (Time.time - lastAbilityTime < abilityCooldown) return;
            lastAbilityTime = Time.time;

            Hero hero = GameManager.Instance.hero;
            if (hero == null) return;

            GameConstants.TopologyForm formType = (GameConstants.TopologyForm)currentFormIndex;

            switch (formType)
            {
                case GameConstants.TopologyForm.Sphere:
                    UseSphereAbility();
                    break;
                case GameConstants.TopologyForm.Torus:
                    UseTorusAbility(hero);
                    break;
                case GameConstants.TopologyForm.Torus2:
                    UsePortalAbility(hero, 2);
                    break;
                case GameConstants.TopologyForm.Torus3:
                    UsePortalAbility(hero, 3);
                    break;
                case GameConstants.TopologyForm.Mobius:
                    UseMobiusAbility(hero);
                    break;
                case GameConstants.TopologyForm.Klein:
                    UseKleinAbility(hero);
                    break;
            }
        }

        // 球形态 - 滚动 (加速移动)
        private void UseSphereAbility()
        {
            // 让勇者短暂加速
            Hero hero = GameManager.Instance.hero;
            if (hero != null)
            {
                Rigidbody2D heroRb = hero.GetComponent<Rigidbody2D>();
                if (heroRb != null)
                {
                    float dir = hero.IsFacingRight() ? 1f : -1f;
                    heroRb.AddForce(Vector2.right * dir * 8f, ForceMode2D.Impulse);
                }
            }
            GameManager.Instance.ShowMessage("球形态 - 滚动加速!");
        }

        // 圆环 - 盾牌防御 (短暂无敌)
        private void UseTorusAbility(Hero hero)
        {
            // 给勇者短暂无敌
            // 这里需要扩展Hero类支持临时无敌
            GameManager.Instance.ShowMessage("盾牌防御! 短暂无敌!");
        }

        // 双洞/三洞圆环 - 传送门
        private void UsePortalAbility(Hero hero, int maxPortals)
        {
            if (portalPositions.Count < maxPortals)
            {
                // 放置传送门
                portalPositions.Add(hero.transform.position);
                if (ParticleManager.Instance != null)
                {
                    ParticleManager.Instance.SpawnPortalEffect(hero.transform.position);
                }
                GameManager.Instance.ShowMessage($"传送门 {portalPositions.Count}/{maxPortals} 已放置");
            }
            else
            {
                // 传送到第一个传送门
                if (portalPositions.Count >= 2)
                {
                    hero.transform.position = portalPositions[0];
                    if (ParticleManager.Instance != null)
                    {
                        ParticleManager.Instance.SpawnPortalEffect(hero.transform.position);
                    }
                    GameManager.Instance.ShowMessage("传送!");
                }
                // 清空传送门
                portalPositions.Clear();
                foreach (var portal in portalObjects)
                {
                    if (portal != null) Destroy(portal);
                }
                portalObjects.Clear();
            }
        }

        // 莫比乌斯环 - 扭曲空间 (交换敌人位置)
        private void UseMobiusAbility(Hero hero)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(hero.transform.position, 4f);
            List<Enemy> enemyList = new();

            foreach (var collider in enemies)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null && enemy.IsAlive())
                {
                    enemyList.Add(enemy);
                }
            }

            // 交换相邻敌人位置
            for (int i = 0; i < enemyList.Count - 1; i += 2)
            {
                Vector3 temp = enemyList[i].transform.position;
                enemyList[i].transform.position = enemyList[i + 1].transform.position;
                enemyList[i + 1].transform.position = temp;
            }

            GameManager.Instance.ShowMessage("扭曲空间! 敌人位置互换!");
        }

        // 克莱因瓶 - 困住敌人
        private void UseKleinAbility(Hero hero)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(hero.transform.position, 3f);

            foreach (var collider in enemies)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null && enemy.IsAlive())
                {
                    enemy.Trap(5f);
                    if (ParticleManager.Instance != null)
                    {
                        ParticleManager.Instance.SpawnTrapEffect(enemy.transform.position);
                    }
                }
            }

            GameManager.Instance.ShowMessage("克莱因瓶! 困住敌人!");
        }

        // 清理传送门
        private void OnDestroy()
        {
            foreach (var portal in portalObjects)
            {
                if (portal != null) Destroy(portal);
            }
        }
    }
}

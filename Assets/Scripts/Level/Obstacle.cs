// 球之积分勇者 - 障碍物
// 荆棘等障碍物，可被火焰烧毁

using UnityEngine;

namespace JingXing
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Obstacle : MonoBehaviour
    {
        public enum ObstacleType
        {
            Thorns,  // 荆棘
            Fire,    // 火焰
            Water    // 水域
        }

        [Header("障碍物设置")]
        public ObstacleType obstacleType = ObstacleType.Thorns;

        [Header("视觉")]
        public SpriteRenderer spriteRenderer;

        [Header("伤害")]
        public int damage = 15;

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void Start()
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (spriteRenderer == null) return;

            switch (obstacleType)
            {
                case ObstacleType.Thorns:
                    spriteRenderer.color = new Color(0f, 0.39f, 0f); // 绿色
                    break;
                case ObstacleType.Fire:
                    spriteRenderer.color = new Color(1f, 0.4f, 0f); // 橙红色
                    break;
                case ObstacleType.Water:
                    spriteRenderer.color = new Color(0f, 0.4f, 0.8f); // 蓝色
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Hero hero = other.GetComponent<Hero>();
            if (hero != null)
            {
                hero.TakeDamage(damage);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Hero hero = other.GetComponent<Hero>();
            if (hero != null)
            {
                hero.TakeDamage(damage);
            }
        }

        // 被烧毁
        public void Destroy()
        {
            if (obstacleType == ObstacleType.Thorns)
            {
                // 粒子效果
                if (ParticleManager.Instance != null)
                {
                    ParticleManager.Instance.SpawnFireEffect(transform.position);
                }

                Destroy(gameObject);
            }
        }
    }
}

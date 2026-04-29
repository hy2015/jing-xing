// 球之积分勇者 - 收集品
// 宝石、心心、钥匙

using UnityEngine;

namespace JingXing
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Collectible : MonoBehaviour
    {
        [Header("收集品设置")]
        public GameConstants.CollectibleType collectibleType = GameConstants.CollectibleType.Gem;

        [Header("视觉")]
        public SpriteRenderer spriteRenderer;
        public float floatAmplitude = 0.2f;
        public float floatFrequency = 1f;

        private Vector2 startPosition;
        private float floatTimer = 0f;
        private bool collected = false;

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            // 确保碰撞器是触发器
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }

        private void Start()
        {
            startPosition = transform.position;
            UpdateVisual();
        }

        private void Update()
        {
            if (collected) return;

            // 上下浮动效果
            floatTimer += Time.deltaTime * floatFrequency;
            float yOffset = Mathf.Sin(floatTimer) * floatAmplitude;
            transform.position = new Vector3(startPosition.x, startPosition.y + yOffset, startPosition.z);
        }

        private void UpdateVisual()
        {
            if (spriteRenderer == null) return;

            switch (collectibleType)
            {
                case GameConstants.CollectibleType.Gem:
                    spriteRenderer.color = new Color(1f, 0.84f, 0f); // 金色
                    break;
                case GameConstants.CollectibleType.Heart:
                    spriteRenderer.color = Color.red;
                    break;
                case GameConstants.CollectibleType.Key:
                    spriteRenderer.color = new Color(1f, 0.84f, 0f); // 金色
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (collected) return;

            Hero hero = other.GetComponent<Hero>();
            if (hero != null)
            {
                Collect(hero);
            }
        }

        private void Collect(Hero hero)
        {
            collected = true;

            switch (collectibleType)
            {
                case GameConstants.CollectibleType.Gem:
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.CollectGem();
                    }
                    break;
                case GameConstants.CollectibleType.Heart:
                    hero.Heal(30);
                    break;
                case GameConstants.CollectibleType.Key:
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.AddScore(100);
                        GameManager.Instance.ShowMessage("获得钥匙!");
                    }
                    break;
            }

            // 粒子效果
            if (ParticleManager.Instance != null)
            {
                ParticleManager.Instance.SpawnCollectEffect(transform.position);
            }

            // 销毁对象
            Destroy(gameObject);
        }
    }
}

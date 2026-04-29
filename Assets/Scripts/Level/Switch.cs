// 球之积分勇者 - 开关
// 触发机关，激活对应的门

using UnityEngine;

namespace JingXing
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Switch : MonoBehaviour
    {
        [Header("开关设置")]
        public int switchId = 0;
        public bool isActivated = false;

        [Header("视觉")]
        public SpriteRenderer spriteRenderer;
        public Color inactiveColor = Color.red;
        public Color activeColor = Color.green;

        [Header("关联")]
        public Gate linkedGate;

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
            UpdateVisual();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActivated) return;

            Hero hero = other.GetComponent<Hero>();
            if (hero != null)
            {
                Activate();
            }
        }

        public void Activate()
        {
            if (isActivated) return;

            isActivated = true;
            UpdateVisual();

            // 激活关联的门
            if (linkedGate != null)
            {
                linkedGate.Open();
            }

            // 加分
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(200);
                GameManager.Instance.ShowMessage("开关激活! 门已打开");
            }

            // 粒子效果
            if (ParticleManager.Instance != null)
            {
                ParticleManager.Instance.SpawnSwitchEffect(transform.position);
            }
        }

        private void UpdateVisual()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = isActivated ? activeColor : inactiveColor;
            }
        }
    }
}

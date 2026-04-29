// 球之积分勇者 - 门/闸门
// 被开关激活后打开

using UnityEngine;

namespace JingXing
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Gate : MonoBehaviour
    {
        [Header("门设置")]
        public int gateId = 0;
        public bool isOpened = false;

        [Header("视觉")]
        public SpriteRenderer spriteRenderer;
        public Color closedColor = new(0.31f, 0.31f, 0.31f);
        public Color openedColor = new(0.31f, 0.31f, 0.31f, 0f);

        [Header("动画")]
        public float openSpeed = 2f;
        public float openHeight = 3f;

        private BoxCollider2D boxCollider;
        private Vector2 closedPosition;
        private Vector2 openedPosition;
        private bool isOpening = false;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void Start()
        {
            closedPosition = transform.position;
            openedPosition = closedPosition + Vector2.up * openHeight;
            UpdateVisual();
        }

        private void Update()
        {
            if (isOpening && !isOpened)
            {
                // 向上移动打开
                transform.position = Vector2.Lerp(transform.position, openedPosition, openSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, openedPosition) < 0.1f)
                {
                    isOpened = true;
                    isOpening = false;
                    // 禁用碰撞器
                    if (boxCollider != null)
                    {
                        boxCollider.enabled = false;
                    }
                }
            }
        }

        public void Open()
        {
            if (isOpened || isOpening) return;

            isOpening = true;

            // 粒子效果
            if (ParticleManager.Instance != null)
            {
                ParticleManager.Instance.SpawnGateEffect(transform.position);
            }
        }

        private void UpdateVisual()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = isOpened ? openedColor : closedColor;
            }
        }
    }
}

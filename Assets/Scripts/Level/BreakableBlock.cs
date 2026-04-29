// 球之积分勇者 - 可破坏方块
// 被红色巨力能力破坏

using UnityEngine;

namespace JingXing
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BreakableBlock : MonoBehaviour
    {
        [Header("视觉")]
        public SpriteRenderer spriteRenderer;

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void Start()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(0.59f, 0.47f, 0.31f); // 棕色
            }
        }

        // 被破坏
        public void Destroy()
        {
            // 粒子效果
            if (ParticleManager.Instance != null)
            {
                ParticleManager.Instance.SpawnBreakEffect(transform.position);
            }

            // 销毁对象
            Destroy(gameObject);
        }
    }
}

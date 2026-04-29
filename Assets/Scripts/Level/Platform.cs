// 球之积分勇者 - 平台
// 支持多种类型：地面、石台、泥土

using UnityEngine;

namespace JingXing
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Platform : MonoBehaviour
    {
        public enum PlatformType
        {
            Ground,  // 地面 (棕色+草地顶部)
            Stone,   // 石台 (灰色)
            Dirt     // 泥土 (深棕色)
        }

        [Header("平台设置")]
        public PlatformType platformType = PlatformType.Ground;

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
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (spriteRenderer == null) return;

            switch (platformType)
            {
                case PlatformType.Ground:
                    spriteRenderer.color = new Color(0.55f, 0.27f, 0.07f); // 棕色
                    break;
                case PlatformType.Stone:
                    spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f); // 灰色
                    break;
                case PlatformType.Dirt:
                    spriteRenderer.color = new Color(0.31f, 0.16f, 0.04f); // 深棕色
                    break;
            }
        }
    }
}

// 球之积分勇者 - 蜘蛛敌人
// 蛛网摆动型敌人

using UnityEngine;

namespace JingXing
{
    public class Spider : Enemy
    {
        [Header("蜘蛛设置")]
        public float webLength = 3f;
        public float swingSpeed = 1f;
        public float swingAmplitude = 1.5f;

        [Header("蛛网")]
        public LineRenderer webLine;
        public Transform anchorPoint;

        private float swingTimer = 0f;
        private Vector2 anchorPosition;

        protected override void Start()
        {
            base.Start();
            health = 50;
            maxHealth = 50;
            moveSpeed = 0; // 蜘蛛不水平移动
            damage = 15;
            patrolRange = 0;
            enemyColor = new Color(0.24f, 0.24f, 0.24f); // 深灰色

            // 设置锚点
            if (anchorPoint != null)
            {
                anchorPosition = anchorPoint.position;
            }
            else
            {
                // 默认锚点在上方
                anchorPosition = transform.position + Vector3.up * webLength;
            }

            // 初始化蛛网线条
            if (webLine == null)
            {
                webLine = gameObject.AddComponent<LineRenderer>();
                webLine.startWidth = 0.05f;
                webLine.endWidth = 0.05f;
                webLine.material = new Material(Shader.Find("Sprites/Default"));
                webLine.startColor = Color.white;
                webLine.endColor = Color.white;
            }

            // 蜘蛛不受重力影响
            if (rb != null)
            {
                rb.gravityScale = 0;
            }
        }

        protected override void Patrol()
        {
            // 摆动运动
            swingTimer += Time.deltaTime * swingSpeed;

            float swingX = Mathf.Sin(swingTimer) * swingAmplitude;
            float swingY = Mathf.Cos(swingTimer * 0.5f) * webLength * 0.5f;

            transform.position = new Vector3(
                anchorPosition.x + swingX,
                anchorPosition.y - webLength + swingY,
                transform.position.z
            );

            // 更新朝向
            facingRight = Mathf.Sin(swingTimer) > 0;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = !facingRight;
            }

            // 更新蛛网线条
            UpdateWebLine();
        }

        private void UpdateWebLine()
        {
            if (webLine != null)
            {
                webLine.SetPosition(0, anchorPosition);
                webLine.SetPosition(1, transform.position);
            }
        }

        // 绘制蛛网
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(anchorPosition, transform.position);
            }
            else
            {
                Gizmos.color = Color.gray;
                Vector3 anchor = transform.position + Vector3.up * webLength;
                Gizmos.DrawLine(transform.position, anchor);
            }
        }
    }
}

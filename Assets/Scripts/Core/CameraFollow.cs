// 球之积分勇者 - 相机跟随系统

using UnityEngine;

namespace JingXing
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("跟随目标")]
        public Transform target;

        [Header("相机设置")]
        public float smoothSpeed = 5f;
        public Vector3 offset = new(0, 1, -10);

        [Header("边界限制")]
        public bool useBounds = true;
        public float minX = -1f;
        public float maxX = 50f;
        public float minY = -1f;
        public float maxY = 15f;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;

            // 平滑跟随
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // 边界限制
            if (useBounds)
            {
                smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
                smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
            }

            transform.position = smoothedPosition;
        }

        /// <summary>
        /// 设置相机边界 (由关卡构建器调用)
        /// </summary>
        public void SetBounds(float xMin, float xMax, float yMin, float yMax)
        {
            minX = xMin;
            maxX = xMax;
            minY = yMin;
            maxY = yMax;
        }
    }
}

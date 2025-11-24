using UnityEngine;

namespace MyAssets.Framework
{
    [ExecuteAlways]
    public class ItemRevolution : MonoBehaviour
    {
        [Header("公转中心位置")] 
        public Transform center;
        [Header("轨道半径")]
        public float orbitRadius = 2f;
        [Header("公转速度")]
        public float orbitSpeed = 90f;
        [Header("公转方向(默认逆时针)")]
        public bool counterClockwise = true;

        [Header("可视化轨道")] 
        public Color orbitColor = Color.cyan;
        [Header("轨道分段数")]
        public int orbitSegments = 64;
    
        // 记录当前角度
        private float currentAngle;

        void Start()
        {
            //判断中心点是否为空，若为空则设为父物体
            if (center == null)
            {
                center = transform.parent;
            }else if(center != null)
            {
                Vector2 offset = transform.position - center.position;
                currentAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                orbitRadius = offset.magnitude;
            }
        }

        void Update()
        {
            //只有在运行游戏时才移动
            if (!Application.isPlaying)
            {
                return;
            }

            if (!center)
            {
                return;
            }

            float direction = counterClockwise ? 1f : -1f;
            currentAngle += orbitSpeed * direction * Time.deltaTime;

            float rad = currentAngle * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;
            transform.position = (Vector2)center.position + offset;
        }

        void OnDrawGizmos()
        {
            if (center == null) return;

            Gizmos.color = orbitColor;

            Vector3 prevPos = center.position + new Vector3(orbitRadius, 0, 0);
            for (int i = 1; i <= orbitSegments; i++)
            {
                float angle = i * (360f / orbitSegments) * Mathf.Deg2Rad;
                Vector3 nextPos = center.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * orbitRadius;
                Gizmos.DrawLine(prevPos, nextPos);
                prevPos = nextPos;
            }
        }
    }
}

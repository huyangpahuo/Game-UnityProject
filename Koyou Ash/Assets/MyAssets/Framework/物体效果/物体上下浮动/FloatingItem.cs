using UnityEngine;

namespace MyAssets.Framework
{
    public class FloatingItem : MonoBehaviour
    {
        [Header("浮动参数")]
        public float floatAmplitude = 0.5f; // 上下浮动的幅度
        public float floatSpeed = 1f;       // 浮动速度

        private Vector3 startPos;

        void Start()
        {
            startPos = transform.position;
        }

        void Update()
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }
    }
}




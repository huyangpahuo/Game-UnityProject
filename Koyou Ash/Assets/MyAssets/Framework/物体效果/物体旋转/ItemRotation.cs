using UnityEngine;

namespace MyAssets.Framework
{
    public class ItemRotation : MonoBehaviour
    {
        
        [Header("自转参数")]
        [Tooltip("每秒旋转角度（正数）")]
        public float rotationSpeed = 90f;

        [Tooltip("是否逆时针旋转")]
        public bool counterClockwise = true;

        void Update()
        {
            float direction = counterClockwise ? 1f : -1f;
            transform.Rotate(0, 0, rotationSpeed * direction * Time.deltaTime);
        }
    }

}

using UnityEngine;

namespace MyAssets.Codes.Framework
{
    public class ButtonSlide : MonoBehaviour
    {
        public enum MoveDirection
        {
            Right,
            Left,
            Up,
            Down
        }

        [Header("移动模式选择")] [Tooltip("是否使用自定义终点位置，如果勾选则禁用距离选项")]
        public bool useCustomTarget = false;

        [Header("自定义终点（当 useCustomTarget = true 时启用）")]
        public Vector2 targetPosition; // 终点坐标（相对 RectTransform 父级的 anchoredPosition）

        [Header("按方向与距离移动（当 useCustomTarget = false 时启用）")]
        public MoveDirection direction = MoveDirection.Right;

        public float distance = 500f;

        [Header("运动参数")] [Tooltip("初速度（像素/秒）")]
        public float initialSpeed = 800f;

        [Tooltip("加速度（默认为0时自动计算，使末速度为0）")] public float acceleration = 0f;

        [Header("控制选项")] public bool autoPlay = true;
        public bool loop = false;

        private RectTransform rect;
        private Vector2 startPos;
        private Vector2 endPos;
        private float currentSpeed;
        private float usedAcceleration;
        private bool isPlaying = false;
        private float traveled = 0f;
        private float totalDistance = 0f;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            startPos = rect.anchoredPosition;

            // 确定终点位置
            if (useCustomTarget)
            {
                endPos = targetPosition;
                totalDistance = Vector2.Distance(startPos, endPos);
            }
            else
            {
                endPos = startPos + DirectionVector() * distance;
                totalDistance = distance;
            }

            // 自动计算加速度（使终点速度=0）
            if (acceleration == 0f)
            {
                acceleration = -(initialSpeed * initialSpeed) / (2 * totalDistance);
            }

            usedAcceleration = acceleration;
        }

        private void Start()
        {
            if (autoPlay) Play();
        }

        public void Play()
        {
            isPlaying = true;
            traveled = 0f;
            currentSpeed = initialSpeed;
            startPos = rect.anchoredPosition;

            // 每次播放都重新计算终点（防止运行时更改）
            if (useCustomTarget)
            {
                endPos = targetPosition;
                totalDistance = Vector2.Distance(startPos, endPos);
            }
            else
            {
                endPos = startPos + DirectionVector() * distance;
                totalDistance = distance;
            }

            if (acceleration == 0f)
            {
                acceleration = -(initialSpeed * initialSpeed) / (2 * totalDistance);
            }

            usedAcceleration = acceleration;
        }

        private void Update()
        {
            if (!isPlaying) return;

            float delta = currentSpeed * Time.deltaTime;
            traveled += Mathf.Abs(delta);

            if (traveled >= totalDistance)
            {
                rect.anchoredPosition = endPos;
                currentSpeed = 0f;
                isPlaying = false;

                if (loop)
                {
                    // 返回原位重新开始
                    (startPos, endPos) = (endPos, startPos);
                    Play();
                }

                return;
            }

            // 更新位置（根据移动模式计算方向）
            Vector2 dir = (endPos - startPos).normalized;
            rect.anchoredPosition += dir * delta;

            // 减速
            currentSpeed += usedAcceleration * Time.deltaTime;
            if (currentSpeed < 0) currentSpeed = 0;
        }

        private Vector2 DirectionVector()
        {
            switch (direction)
            {
                case MoveDirection.Left: return Vector2.left;
                case MoveDirection.Right: return Vector2.right;
                case MoveDirection.Up: return Vector2.up;
                case MoveDirection.Down: return Vector2.down;
                default: return Vector2.right;
            }
        }

        public void Stop() => isPlaying = false;

        // 为了在编辑器中更清晰显示当前禁用选项
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (useCustomTarget)
            {
                // 如果使用终点，就锁定距离和方向
                distance = Mathf.Max(0.01f, distance);
            }
        }
#endif
    }
}
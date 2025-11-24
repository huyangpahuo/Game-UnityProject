using UnityEngine;

namespace MyAssets.Framework
{
    //通用按钮逃跑逻辑组件，实现 IButtonEscape。
    //可与其他按钮脚本组合使用。

    [RequireComponent(typeof(RectTransform))]
    public class ButtonEscapeLogic : MonoBehaviour, IButtonEscape
    {
        [Header("功能开关")] public bool enableEscape = true;

        public bool EnableEscape
        {
            get => enableEscape;
            set => enableEscape = value;
        }

        [Header("检测范围")] public Vector2 detectBoxSize = new Vector2(300f, 150f);

        public Vector2 DetectBoxSize
        {
            get => detectBoxSize;
            set => detectBoxSize = value;
        }

        [Header("逃跑参数")] public float escapeDistance = 200f;

        public float EscapeDistance
        {
            get => escapeDistance;
            set => escapeDistance = value;
        }

        public float moveSpeed = 6f;

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

        [Header("可视化设置")] public Color gizmoColor = new Color(0f, 191f, 255f, 0.3f);

        private RectTransform rect;
        private Canvas canvas;
        private Vector2 originalPos;
        private Vector2 targetPos;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            originalPos = rect.anchoredPosition;
            targetPos = originalPos;
        }

        void Update()
        {
            HandleEscape(rect, canvas);
        }

        public void HandleEscape(RectTransform rect, Canvas canvas)
        {
            if (!enableEscape || !rect || !canvas)
            {
                return;
            }

            Vector2 mousePos = Input.mousePosition;
            Vector2 buttonCenter = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rect.position);

            bool inside =
                mousePos.x >= buttonCenter.x - detectBoxSize.x / 2 &&
                mousePos.x <= buttonCenter.x + detectBoxSize.x / 2 &&
                mousePos.y >= buttonCenter.y - detectBoxSize.y / 2 &&
                mousePos.y <= buttonCenter.y + detectBoxSize.y / 2;

            if (inside)
            {
                Vector2 dir = GetEscapeDirection(mousePos, buttonCenter);
                targetPos = originalPos + dir * escapeDistance;
            }
            else
            {
                targetPos = originalPos;
            }

            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPos, moveSpeed * Time.deltaTime);
        }

        private Vector2 GetEscapeDirection(Vector2 mouse, Vector2 center)
        {
            float dx = mouse.x - center.x;
            float dy = mouse.y - center.y;

            Vector2 dir = Vector2.zero;

            if (dx >= 0 && dy >= 0)
                dir = new Vector2(-1, -1); // 第一象限 -> 第三象限
            else if (dx < 0 && dy >= 0)
                dir = new Vector2(1, -1); // 第二象限 -> 第四象限
            else if (dx < 0 && dy < 0)
                dir = new Vector2(1, 1); // 第三象限 -> 第一象限
            else if (dx >= 0 && dy < 0)
                dir = new Vector2(-1, 1); // 第四象限 -> 第二象限

            // 若在坐标轴上，仅沿垂直方向逃跑
            if (Mathf.Approximately(dx, 0))
                dir.x = 0;
            if (Mathf.Approximately(dy, 0))
                dir.y = -Mathf.Sign(dy);

            return dir.normalized;
        }

        void OnDrawGizmos()
        {
            if (rect == null) rect = GetComponent<RectTransform>();
            if (rect == null) return;

            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);
            Vector3 center = (corners[0] + corners[2]) / 2;

            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(center, new Vector3(detectBoxSize.x, detectBoxSize.y, 0));
        }
    }
}
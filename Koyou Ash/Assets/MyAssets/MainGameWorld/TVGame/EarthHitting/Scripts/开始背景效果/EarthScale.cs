using UnityEngine;
using UnityEngine.EventSystems;

namespace EarthHitting
{
    public class EarthScale : MonoBehaviour
    {
        [Header("放大设置")] public float scaleMultiplier = 1.15f; // 放大比例
        public float animSpeed = 8f; // 动画速度

        private Vector3 originalScale;
        private bool isHovering;
        private bool isUI;

        void Awake()
        {
            originalScale = transform.localScale;

            // 判断是否是 UI 元素
            isUI = GetComponent<RectTransform>() != null && GetComponentInParent<Canvas>() != null;

            if (isUI)
            {
                // 如果是 UI 按钮或 UI 元素，则添加 EventTrigger
                var trigger = gameObject.GetComponent<EventTrigger>();
                if (trigger == null)
                    trigger = gameObject.AddComponent<EventTrigger>();

                trigger.triggers.Clear();

                // 鼠标进入
                var entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
                entryEnter.callback.AddListener(_ => isHovering = true);
                trigger.triggers.Add(entryEnter);

                // 鼠标离开
                var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
                entryExit.callback.AddListener(_ => isHovering = false);
                trigger.triggers.Add(entryExit);
            }
            else
            {
                // 对于非 UI 物体，确保有 Collider
                if (GetComponent<Collider>() == null && GetComponent<Collider2D>() == null)
                {
                    Debug.LogWarning($"{name} 缺少 Collider 或 Collider2D，鼠标悬停检测将无法工作！");
                }
            }
        }

        void Update()
        {
            Vector3 target = isHovering ? originalScale * scaleMultiplier : originalScale;
            transform.localScale = Vector3.Lerp(transform.localScale, target, animSpeed * Time.deltaTime);
        }

        // 非 UI 物体使用这些事件
        void OnMouseEnter()
        {
            if (!isUI)
                isHovering = true;
        }

        void OnMouseExit()
        {
            if (!isUI)
                isHovering = false;
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyAssets.Framework
{
    public class ButtonScale : MonoBehaviour
    {
        [Header("按钮")] public Button btn; // 拖入开始按钮

        [Header("放大设置")] public  float scaleMultiplier = 1.15f; // 放大多少
        public float animSpeed = 8f; // 越大越快

        // 记录原始缩放
        protected Vector3 originalScale;

        // 记录鼠标是否悬停
        protected bool isHovering;

        protected virtual void Awake()
        {
            originalScale = transform.localScale;

            btn = GetComponent<Button>();

            if (btn == null)
            {
                Debug.LogError("HoverScale 需要挂在 Button 物体上");
            }
        }

        protected virtual void OnEnable()
        {
            // 监听鼠标进出
            btn.onClick.AddListener(() => { }); // 防止按钮无响应，可忽略

            var trigger = btn.gameObject.GetComponent<EventTrigger>();

            if (trigger == null)
            {
                trigger = btn.gameObject.AddComponent<EventTrigger>();
            }

            // 进入
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };

            entry.callback.AddListener(_ => isHovering = true);

            trigger.triggers.Add(entry);

            // 离开
            var exit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };

            exit.callback.AddListener(_ => isHovering = false);

            trigger.triggers.Add(exit);
        }

        protected virtual void Update()
        {
            Vector3 target = isHovering ? originalScale * scaleMultiplier : originalScale;
            transform.localScale = Vector3.Lerp(transform.localScale, target, animSpeed * Time.deltaTime);
        }
    }
}
using UnityEngine;

namespace MyAssets.Framework
{
    //按钮逃跑逻辑接口，用于定义统一的行为。
    //任何按钮类或组件都可以实现它。

    public interface IButtonEscape
    {
        bool EnableEscape { get; set; }
        Vector2 DetectBoxSize { get; set; }
        float EscapeDistance { get; set; }
        float MoveSpeed { get; set; }


        //检查鼠标并执行逃跑逻辑。

        void HandleEscape(RectTransform rect, Canvas canvas);
    }
}
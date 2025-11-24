using UnityEngine;
using UnityEngine.EventSystems;

public class MoveBag : MonoBehaviour, IDragHandler
{
    //当前背包的RectTransform组件
    RectTransform currentRect;

    void Awake()
    {
        currentRect = GetComponent<RectTransform>();
    }


    //移动背包的中心点的位置
    public void OnDrag(PointerEventData eventData)
    {
        //根据鼠标的移动增量来更新背包中心点的位置
        currentRect.anchoredPosition += eventData.delta;
    }
}
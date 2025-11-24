using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    public Inventory myBag;
    private int currentItemID; //当前物体的ID


    //调用IBeginDragHandler接口的方法,表示开始拖拽执行的操作
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; //获取Item的父对象Slot的Transform
        currentItemID = originalParent.GetComponent<Slot>().slotID; //获取Item的ID,等于Slot的ID
        transform.SetParent(transform.parent.parent); //在拖着Item时将Item的父对象设置为Slot的父对象Grid,这样可以拖着Item满屏幕拖动
        transform.position = eventData.position; //将Item的位置设置为鼠标位置


        /*CanvasGroup 是 Unity 中专门用来“打包”管理一组 UI 元素的组件,这里用它来控制图标的射线检测属性
        在实现Item可以满屏幕拖动之后,还要获取Item下面的物体的信息,比如就要检测鼠标下面的物体是否为Item Image物体
        而如果不做处理的话,鼠标下面永远检测到是被拖动的Item图标本身,因为它在最上层,会阻挡射线检测到下面的物体

        可以类比为现实生活中你用手拿着一个物品,你的手挡住了你眼睛对物品下面的东西的观察,所以你看不到下面的东西
        这里鼠标发出的射线相当于目光,拖动的行为相当于用手抽动纸片,而纸片挡住了目光,看不到纸片后面的东西

        而在开始拖拽时把 blocksRaycasts = false就相当于让纸片变成透明的纸片,既可以实现拖动纸片的效果,又不会挡住目光

        让被拖动的图标不再阻挡射线,这样鼠标才能“穿透”它去检测下面的格子,背包槽等目标 UI*/
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    //调用IDragHandler接口的方法,表示拖拽中执行的操作
    public void OnDrag(PointerEventData eventData)
    {
        //拖拽中持续更新Item的位置为鼠标位置
        transform.position = eventData.position;
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name); //输出当前鼠标下面检测到的UI物体的名字
    }

    //调用IEndDragHandler接口的方法,表示结束拖拽执行的操作
    public void OnEndDrag(PointerEventData eventData)
    {
        //经过检测,当鼠标拖动物体到背包槽上时,鼠标下面检测到的物体名字是Item Image
        //如果鼠标下面检测到的物体名字是Item Image,说明是拖拽到另一个物品图标上了
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "Item Image")
            {
                /*Item Image的父对象是Item,而Item的父对象才是Slot,所以这里要获取到Slot的Transform
                transform 在脚本里指当前脚本挂载的那个被拖拽物体(GameObject)自身的 Transform。
                eventData.pointerCurrentRaycast.gameObject.transform 是鼠标当前射线命中的 UI 物体的 Transform

                这里是表示将Item的父物体slot设为鼠标下面检测到的Item Image的父物体Item的父物体Slot
                即将Item的归属关系设为鼠标下面检测到的物体所属的那个抽屉Slot
                */
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                /*将Item的位置设置为鼠标下面检测到的Item Image的父物体Item的父物体Slot的位置
                就相当于slot是抽屉,Item是抽屉里的物品,每个抽屉的位置不同,而抽屉里的物体和本身抽屉的位置是一样的
                这里就是第一步表明Item将要放到另一个鼠标指定的抽屉slot里去
                第二步是吧这个Item从抽屉里抽出来放到这个指定的抽屉里
                即将物体的position设置为鼠标下面检测到的抽屉Slot的position
                */
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

                //由于之前的那个是只是交换了UI中的物体的位置,但是没有改变背包中数据库中的物品的位置
                //所以需要再次交换背包数据库列表itemList中物品的位置
                var temp = myBag.itemList[currentItemID];
                myBag.itemList[currentItemID] =
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];


                /*同样的,在一开始的时候我们就获取了要拖拽的放Item的原来的抽屉Slot的位置originalParent.position
                 *然后就把鼠标下面的另一个Item的位置设为原来Item的抽屉Slot的位置
                 *就相当于把鼠标下的另一个Item从他的抽屉Slot里抽出来放到我们拖动的Item的原来抽屉Slot里去
                 */
                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
                /*  上一步虽然改了鼠标下的这个物体Item的位置,但还没有指定归属关系
                因为假如我们把要移动的物体称之为钱钱,装它的抽屉Slot称之为买水果的篮子
                把鼠标下面的物体称之为香蕉,装它的抽屉Slot称之为水果摊
                那么上一步只是把香蕉放到了买水果的篮子里,但是香蕉的归属关系还是水果摊(因为你没付钱)
                不过如果我们在前几步已经改变了Item的归属关系为鼠标指定的物体所属那个抽屉,即把钱钱放到了水果摊(老板手)里
                所以这里香蕉已经归我们了,我们只需要把香蕉的归属关系改为买水果的篮子就行了
                即将鼠标指定物体Item的归属关系改为原来Item的抽屉Slot
                 */
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);

                //拖拽结束后再把 blocksRaycasts = true,就相当于把纸片又变回不透明的纸片了,鼠标才能检测到纸片本身而不是下面的东西
                //恢复图标正常接收射线,保证以后还能再次拖拽或点击
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }

            if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)")
            {
                //如果没有拖拽到Item Image上,就将Item设置为到鼠标射线指向的位置
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

                //itemList的物品存储位置改变
                myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] =
                    myBag.itemList[currentItemID];
                //处理把物品放回原位的物体消失的情况
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID != currentItemID)
                {
                    myBag.itemList[currentItemID] = null;
                }

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
        }


        //Item拖到其他任何位置就回到原位
        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
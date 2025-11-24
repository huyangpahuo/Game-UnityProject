using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 这个slot本质上是将物体的所有信息转换为UI显示在背包中的一个载体,本身不包含任何逻辑
/// </summary>
public class Slot : MonoBehaviour
{
    
    
    public int slotID;//背包空格ID等于物品的ID
    
    //public Item slotItem;   //背包槽位本身
    public Image slotImage;//背包槽位显示物体图标
    public Text slotNum;//背包槽位显示物体数量
    public string slotInfo;//背包槽位显示物体信息

    public GameObject itemInSlot;//背包槽位中物体对象
    
    //点击槽位时更新物品信息显示
    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotInfo);
    }
    
    public void SetupSlot(Item item)
    {
        if (item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }
        
        //将物体信息赋值给槽位
        slotImage.sprite= item.itemImage;
        slotNum.text = item.itemHeld.ToString();
        slotInfo=item.itemInfo; 
    }
}

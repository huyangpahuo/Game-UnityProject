using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    [Header("物品对象")]
    public Item thisItem;
    [Header("玩家背包对象")]
    public Inventory playerInventory;

    
    //检测玩家与物品的碰撞
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    //添加物品到背包
    public void AddNewItem()
    {
        //先判断玩家的背包列表中是否已经有该物品
        //如果没有就先循环插入物品
        if(!playerInventory.itemList.Contains(thisItem))
        {
            //playerInventory.itemList.Add(thisItem);
            //InventoryManager.CreateNewItem(thisItem);
            
            for (int i = 0; i < playerInventory.itemList.Count; i++)
            {
                if (playerInventory.itemList[i] == null)
                {
                    playerInventory.itemList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            //有的话直接数量itemHeld++就可以了
            thisItem.itemHeld += 1;
        }
        //完成后刷新背包UI
        InventoryManager.RefeshItem();
    }
}

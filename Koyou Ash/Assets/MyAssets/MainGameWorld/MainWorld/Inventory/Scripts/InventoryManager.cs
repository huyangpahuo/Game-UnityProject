using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    //背包管理单例
    static InventoryManager instance;

    
    public Inventory myBag;
    public GameObject slotGrid;
    
    //public Slot slotPrefab;
    public GameObject emptySlot;
    public Text itemInformation;
    
    public List<GameObject> slots = new List<GameObject>();

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    //将物品信息显示在UI上
    private void OnEnable()
    {
        RefeshItem();

        instance.itemInformation.text = "";
    }

    //更新物品信息显示
    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInformation.text = itemDescription;
    }


    //在背包中创建新物品,也就是角色捡起物品时调用用来在背包中生成物品图标
    /*public static void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);

        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHeld.ToString();
    }*/

    
    //刷新背包物品数量显示
    public static void RefeshItem()
    {
        //循环删除slotGrid下的所有子物体
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            if (instance.slotGrid.transform.childCount == 0)
                break;

            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }
        //重新生成对应myBag里面的物品
        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            //CreateNewItem(instance.myBag.itemList[i]);
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<Slot>().slotID = i;
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.myBag.itemList[i]);
        }
    }
}
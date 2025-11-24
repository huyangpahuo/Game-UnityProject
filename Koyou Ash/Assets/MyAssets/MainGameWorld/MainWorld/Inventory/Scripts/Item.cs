using UnityEngine;

//这个脚本用于右键创建物品数据资产
[CreateAssetMenu(fileName = "New Item",menuName = "Inventory/New Item")]
public class Item :ScriptableObject
{
    public string itemName; //物品名称
    public Sprite itemImage; //物品图标
    public int itemHeld;//物品数量
    
    [TextArea]//多行文本框
    public string itemInfo; //物品描述

    public bool equip;
}

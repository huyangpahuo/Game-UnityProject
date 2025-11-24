using System.Collections.Generic;
using UnityEngine;

//这个脚本用于右键创建背包数据资产
[CreateAssetMenu(fileName = "New Inventory",menuName = "Inventory/New Inventory")]
public class Inventory :ScriptableObject
{
    //这个脚本用于创建背包数据资产
    public List<Item> itemList=new List<Item>();
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameSaveManager : MonoBehaviour
{
    public Inventory myInventory;

    //保存游戏数据到文件
    public void SaveGame()
    {
        //输出当前游戏存储的路径
        Debug.Log(Application.persistentDataPath);
        
        //判断存档文件夹是否存在，不存在则创建
        if(!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
        }
        
        //创建二进制格式化器
        BinaryFormatter formatter = new BinaryFormatter();
        
        //创建背包存档文件
        FileStream file = File.Create(Application.persistentDataPath + "/game_SaveData/inventory.txt");
        
        //将背包数据转换为JSON格式字符串
        var json=JsonUtility.ToJson(myInventory);
        
        //使用Serializer对json字符串进行序列化并保存到文件
        formatter.Serialize(file,json);
        
        //关闭文件
        file.Close();
        
    }

    //从文件加载游戏数据
    public void LoadGame()
    {
        BinaryFormatter bf=new BinaryFormatter();

        //首先判断存档文件是否存在
        if (File.Exists(Application.persistentDataPath + "/game_SaveData/inventory.txt"))
        {
            //打开存档文件
            FileStream file = File.Open(Application.persistentDataPath + "/game_SaveData/inventory.txt", FileMode.Open);
            
            //反序列化读取文件中的json字符串，并将其覆盖到myInventory对象中
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file),myInventory);
            
            //关闭文件
            file.Close();
        }
    }

}

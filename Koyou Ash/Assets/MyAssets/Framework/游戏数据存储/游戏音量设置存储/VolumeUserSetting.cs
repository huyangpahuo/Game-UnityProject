using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public class VolumeUserSetting : MonoBehaviour
{
    [SerializeField] Text uiText;

    [SerializeField] private PlayerData data;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // PlayerPrefs.SetString("name", data.name);
            // PlayerPrefs.SetInt("level",data.level);


            // FileStream fs =new FileStream(Application.dataPath+"/Save.txt", FileMode.Create);
            // StreamWriter sw = new StreamWriter(fs);
            // sw.WriteLine(data.name);
            // sw.WriteLine(data.level);
            // sw.Close();
            // fs.Close();


            // BinaryFormatter bf = new BinaryFormatter();//这个文件格式可以自己定义
            // Stream s=File.Open(Application.dataPath + "/SaveData.ept", FileMode.Create);
            // bf.Serialize(s, data);
            // s.Close();


            // XmlSerializer xml=new XmlSerializer(data.GetType());
            // Stream s=File.Open(Application.dataPath + "/SaveData.xml", FileMode.Create);
            // xml.Serialize(s, data); 
            // s.Close();
            
            
            
            PlayerPrefs.SetString("jsondata", JsonUtility.ToJson(data));
            uiText.text = "存储完成";
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //uiText.text = PlayerPrefs.GetString("name");
            // data.name = PlayerPrefs.GetString("name");
            // data.level = PlayerPrefs.GetInt("level");

            // FileStream fs =new FileStream(Application.dataPath+"/Save.txt", FileMode.Open);
            // StreamReader sr = new StreamReader(fs);
            // data.name=sr.ReadLine();
            // data.level=int.Parse( sr.ReadLine());

            // BinaryFormatter bf = new BinaryFormatter(); 
            // Stream s=File.Open(Application.dataPath + "/SaveData.ept", FileMode.Open);
            // data=(PlayerData)bf.Deserialize(s);


            // XmlSerializer xml=new XmlSerializer(data.GetType());
            // Stream s=File.Open(Application.dataPath + "/SaveData.xml", FileMode.Open);
            // data=(PlayerData)xml.Deserialize(s);
            
            data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("jsondata"));
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public int level;
    }
}
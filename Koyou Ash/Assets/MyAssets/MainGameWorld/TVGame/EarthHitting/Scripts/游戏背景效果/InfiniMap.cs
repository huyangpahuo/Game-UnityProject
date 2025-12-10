using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniMap : MonoBehaviour
{
    [Header("无限地图")] 
    public GameObject mainCamera;//主摄像机对象
    public float mapWidth;//地图宽度
    public int mapMums;//地图重复的次数
    public float totalwidth;//总地图宽度
    
    
    void Start()
    {
        //查找标签为MainCamera的游戏对象并赋值给mainCamera变量
        mainCamera=GameObject.FindGameObjectWithTag("MainCamera");
        //通过SpriteRenderer组件获取图片的宽度
        mapWidth=GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        //计算总地图的宽度
        totalwidth=mapWidth*mapMums;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempPosition=transform.position;//获取当前位置
        //如果摄像机的位置与玩家的位置超过地图宽度的一半，则将背景位置向右移动一个地图宽度
        
        if (mainCamera.transform.position.x > transform.position.x + totalwidth / 2)
        {
            tempPosition.x += totalwidth;
            transform.position = tempPosition;
        }
        else if (mainCamera.transform.position.x < transform.position.x - totalwidth / 2)
        {
            tempPosition.x-= totalwidth;//将地图向左移动一个地图宽度
            transform.position = tempPosition;//更新位置
        }
    }
}

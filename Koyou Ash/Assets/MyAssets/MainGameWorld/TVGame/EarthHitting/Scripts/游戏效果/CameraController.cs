using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //玩家的位置
    public Transform target;
    //远的背景图和中间的背景图的位置
    public Transform farBackground, middleBackground;
    //最后一次相机的位置
    private Vector2 lastCameraPosition;
    
    void Start()
    {
        //记录相机的初始位置
        lastCameraPosition = transform.position;
    }

    private void Update()
    {
        //将相机的位置设置为玩家的位置,但限制在一定的垂直范围
        transform.position=new Vector3(target.position.x,target.position.y,target.position.z);
        
        //计算相机在上一帧与当前帧移动的距离
        Vector2 amounToMove = new Vector2(transform.position.x - lastCameraPosition.x,transform.position.y - lastCameraPosition.y);
        
        //根据相机移动的距离,移动远背景和中间背景的位置
        farBackground.position+= new Vector3(amounToMove.x,amounToMove.y,0f);
        middleBackground.position+= new Vector3(amounToMove.x*0.5f,amounToMove.y*0.5f,0f);
        
        lastCameraPosition = transform.position;
    }
}

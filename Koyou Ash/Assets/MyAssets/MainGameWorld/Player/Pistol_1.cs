using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_1 : MonoBehaviour
{

    private float interval;//射击间隔
    private GameObject bulletPrefab;//子弹预制体
    private GameObject shellPrefab;//弹壳预制体
    private Transform muzzlePos;//枪口位置
    private Transform shellPos;//弹壳位置
    private Vector2 mousePos;//鼠标位置
    private Vector2 direction;//枪口朝向
    private float timer;//射击计时器
    private float flipY;//枪的翻转状态
    private Animator animator;//动画组件

    void Start()
    {
        animator = GetComponent<Animator>(); //获取动画组件
        muzzlePos = transform.Find("Muzzle");//找到枪口位置
        shellPos = transform.Find("BulletShell");//找到弹壳位置
        flipY = transform.localScale.y;
    }


    void Update()
    {

    }
}

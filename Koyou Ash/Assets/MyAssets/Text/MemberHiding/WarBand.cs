using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBand : MonoBehaviour
{
    void Start () 
    {
        Humanoid human = new Humanoid();
        Humanoid enemy = new Enemy();
        Humanoid orc = new Orc();

        //注意每个 Humanoid 变量如何包含
        //对继承层级视图中
        //不同类的引用，但每个变量都
        //调用 Humanoid Yell() 方法。
        
        //可以看到结果全是 Humanoid 版本的 Yell() 方法被调用。
        //这是因为变量的类型决定了调用哪个版本的方法。
        //即使变量引用的是派生类的实例。
        //这种行为称为“成员隐藏”
        human.Yell();
        enemy.Yell();
        orc.Yell();
    }
}

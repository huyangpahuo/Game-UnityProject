using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Humanoid
{
    //这会隐藏 Humanoid 版本。
    new public void Yell()
    {
        Debug.Log("Enemy version of the Yell() method");
    }
}
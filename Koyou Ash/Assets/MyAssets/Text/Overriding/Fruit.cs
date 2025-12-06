using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Fruit()
    {
        Debug.Log("1st Fruit Constructor Called");
    }

    //这些方法是虚方法，因此可以在子类中
    //将它们覆盖
    public virtual void Chop()
    {
        Debug.Log("The fruit has been chopped.");
    }

    public virtual void SayHello()
    {
        Debug.Log("Hello, I am a fruit.");
    }
}
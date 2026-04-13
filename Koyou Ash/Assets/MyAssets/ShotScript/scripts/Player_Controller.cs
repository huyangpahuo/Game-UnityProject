using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // 玩家移动相关变量
    public float playerSpeed = 5f;//玩家速度
    private Rigidbody2D playerRigidbody;//玩家刚体
    private Animator playerAnimator;//玩家动画
    private Vector2 movementInput;//玩家输入,用于移动

    //武器相关变量
    public GameObject[] weapons;//武器数组
    private int currentWeaponIndex;//当前武器索引
    private Vector2 mousePosition;//鼠标位置


    void Start()
    {
        GetComponents();//获取组件引用

        weapons[0].SetActive(true);//默认激活第一把武器
    }

    void Update()
    {
        MovementLogic();//玩家控制行为
        SwitchWeapon();//切换武器
    }

    //获取组件引用
    void GetComponents()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    //玩家控制行为
    void MovementLogic()
    {
        //获取玩家输入
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        //移动玩家的刚体而非直接移动Transform
        playerRigidbody.velocity = movementInput.normalized * playerSpeed;

        //获取鼠标在世界坐标中的位置
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //根据鼠标位置调整玩家朝向
        if (mousePosition.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        //根据角色移动状态切换动画
        if (movementInput != Vector2.zero)
        {
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }
    }

    //切换武器
    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapons[currentWeaponIndex].SetActive(false);
            if (--currentWeaponIndex < 0)
            {
                currentWeaponIndex = weapons.Length - 1;
            }
            weapons[currentWeaponIndex].SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            weapons[currentWeaponIndex].SetActive(false);
            if (++currentWeaponIndex > weapons.Length - 1)
            {
                currentWeaponIndex = 0;
            }
            weapons[currentWeaponIndex].SetActive(true);
        }
    }
}





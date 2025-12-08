using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //角色刚体
    private Rigidbody2D mRig;

    //每秒移动速度
    public float moveSpeed = 5f;

    //动画
    private Animator animator;

    // 上一次有效输入方向（用于角色静止时保持朝向）
    private Vector2 lastMoveDir = new Vector2(0, -1);

    //背包
    public GameObject myBag;
    bool isBagOpen = false;


    void Start()
    {
        //获取组件
        GetComponentAccess();
        //设置帧率
        FrameSetup();
    }

    void Update()
    {
        InputProcess();

        OpenMyBag();
    }

    void FrameSetup()
    {
        //设置帧率为60
        //这里垂直同步设置为1，表示每一帧都等待显示器刷新一次
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    //访问游戏对象组件
    void GetComponentAccess()
    {
        //刚体Rigidbody
        mRig = GetComponent<Rigidbody2D>();
        //动画Animator
        animator = GetComponent<Animator>();
    }


    //检测玩家输入和处理移动
    void InputProcess()
    {
        //检测玩家输入
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 归一化输入向量,角色各个方向移动速度相同
        Vector2 input = new Vector2(horizontal, vertical).normalized;


        Vector2 position = mRig.position;
        position += input * (moveSpeed * Time.deltaTime); // 匀速任意方向
        transform.position = position;


        // 如果有输入（非零），则更新朝向
        if (input != Vector2.zero)
        {
            lastMoveDir = input;
        }

        animator.SetFloat("Move X", lastMoveDir.x);
        animator.SetFloat("Move Y", lastMoveDir.y);
        animator.SetFloat("Speed", input.magnitude);
    }

    
    //按B键打开背包
    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBagOpen = !isBagOpen;
            myBag.SetActive(isBagOpen);
            //每次打开背包时刷新物品栏
            InventoryManager.RefeshItem();
        }
    }
}
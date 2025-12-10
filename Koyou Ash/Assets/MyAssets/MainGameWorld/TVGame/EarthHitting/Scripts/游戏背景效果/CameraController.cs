using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("要跟随的目标")]
    public Transform target;

    [Header("视差背景")]
    public Transform farBackground;
    public Transform middleBackground;

    [Tooltip("中景视差系数（0~1，数值越小，移动越慢）")]
    [Range(0f, 1f)] public float middleParallax = 0.5f;

    [Tooltip("远景视差系数（0~1，远景通常更小）")]
    [Range(0f, 1f)] public float farParallax = 0.25f;

    [Header("相机设置")]
    public float zPosition = -10f;

    // 可选：限制垂直范围
    public bool clampY = false;
    public float minY = -10f;
    public float maxY = 10f;

    private Vector3 lastCameraPosition;
    private float farZ, middleZ;

    void Start()
    {
        if (target != null)
        {
            // 初始化时先把相机放到正确的 Z
            var y = target.position.y;
            if (clampY) y = Mathf.Clamp(y, minY, maxY);
            transform.position = new Vector3(target.position.x, y, zPosition);
        }

        lastCameraPosition = transform.position;

        // 记录背景原始 Z，防止 Z 漂移
        if (farBackground != null) farZ = farBackground.position.z;
        if (middleBackground != null) middleZ = middleBackground.position.z;
    }

    void LateUpdate()
    {
        if (!target) return;

        // 相机跟随（固定 Z）
        float y = target.position.y;
        if (clampY) y = Mathf.Clamp(y, minY, maxY);
        Vector3 newCamPos = new Vector3(target.position.x, y, zPosition);
        transform.position = newCamPos;

        // 计算相机位移（与上一帧相比）
        Vector3 amountToMove = transform.position - lastCameraPosition;

        // 视差：只改 X/Y，不改 Z
        if (farBackground)
        {
            farBackground.position += new Vector3(amountToMove.x * farParallax, amountToMove.y * farParallax, 0f);
            // 强制恢复 Z，避免累计误差
            farBackground.position = new Vector3(farBackground.position.x, farBackground.position.y, farZ);
        }

        if (middleBackground)
        {
            middleBackground.position += new Vector3(amountToMove.x * middleParallax, amountToMove.y * middleParallax, 0f);
            middleBackground.position = new Vector3(middleBackground.position.x, middleBackground.position.y, middleZ);
        }

        lastCameraPosition = transform.position;
    }
}
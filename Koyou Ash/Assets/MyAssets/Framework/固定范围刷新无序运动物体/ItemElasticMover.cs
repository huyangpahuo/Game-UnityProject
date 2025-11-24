using UnityEngine;

namespace MyAssets.Framework
{
    public class ItemElasticMover : MonoBehaviour
    {
        private Rigidbody2D rb;
        private BackgroundItemController controller;

        private float moveSpeed;
        private float rotateSpeed;
        private int rotateDir;

        [Header("检测与避让参数")] [Tooltip("检测范围半径（单位：世界空间单位）")]
        public float detectRadius = 1.5f;

        [Tooltip("两次方向重置之间的冷却时间（秒）")] public float resetCooldown = 0.5f;

        [Tooltip("检测的层（默认全部）")] public LayerMask detectionLayer = ~0;

        private float lastResetTime = -999f;
        private float minSpeedThreshold = 0.05f;

        [Header("检测范围可视化")] public bool showDetectRange = true;
        public Color detectRangeColor = new Color(1f, 0.5f, 0f, 0.3f);

        public void Init(BackgroundItemController ctrl)
        {
            controller = ctrl;
            rb = GetComponent<Rigidbody2D>();

            // === 刚体设置 ===
            rb.gravityScale = 0;
            rb.drag = 0.3f;
            rb.angularDrag = 0.3f;
            rb.mass = 1f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // === 设置碰撞材质 ===
            var col = GetComponent<Collider2D>();
            var mat = new PhysicsMaterial2D("ElasticMat");
            mat.bounciness = 0.5f;
            mat.friction = 0f;
            col.sharedMaterial = mat;

            if (detectionLayer == 0)
                detectionLayer = LayerMask.GetMask("Default");

            ResetDirection();
        }

        public void Move()
        {
            DetectAndReset();
            MaintainMotion();
        }

        private void DetectAndReset()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRadius, detectionLayer);
            Vector2 separationForce = Vector2.zero;
            int nearbyCount = 0;

            foreach (var hit in hits)
            {
                if (hit.gameObject == gameObject) continue;
                Vector2 diff = (Vector2)(transform.position - hit.transform.position);
                float dist = diff.magnitude;
                if (dist > 0.001f)
                {
                    // 距离越近，推开力度越大
                    separationForce += diff.normalized / dist;
                    nearbyCount++;
                }
            }

            if (nearbyCount > 0)
            {
                separationForce /= nearbyCount;
                separationForce.Normalize();

                // 添加微量随机扰动
                separationForce += Random.insideUnitCircle * 0.3f;
                separationForce.Normalize();

                // 用力而非瞬间设置速度
                rb.AddForce(separationForce * (moveSpeed * 1.2f), ForceMode2D.Impulse);

                // 限制最大速度
                if (rb.velocity.magnitude > controller.moveSpeedMax)
                    rb.velocity = rb.velocity.normalized * controller.moveSpeedMax;
            }

            // 保持转动
            rb.angularVelocity = rotateSpeed * rotateDir;
        }

        private void ResetDirection()
        {
            if (!rb) return;

            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            // 新方向：随机+轻微偏向中心
            Vector2 dir = (controller.transform.position - transform.position).normalized;
            dir += Random.insideUnitCircle * 0.6f;
            dir.Normalize();

            moveSpeed = Random.Range(controller.moveSpeedMin, controller.moveSpeedMax);
            rotateSpeed = Random.Range(controller.rotateSpeedMin, controller.rotateSpeedMax);
            rotateDir = Random.value > 0.5f ? 1 : -1;

            rb.velocity = dir * moveSpeed;
            rb.angularVelocity = rotateSpeed * rotateDir;
        }

        private void MaintainMotion()
        {
            if (rb.velocity.magnitude < minSpeedThreshold && Time.time - lastResetTime > 0.3f)
            {
                ResetDirection();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ResetDirection();
        }

        private void Update()
        {
            Move();
        }

        // === Gizmo 可视化 ===
        private void OnDrawGizmosSelected()
        {
            if (!showDetectRange) return;

            Gizmos.color = detectRangeColor;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}
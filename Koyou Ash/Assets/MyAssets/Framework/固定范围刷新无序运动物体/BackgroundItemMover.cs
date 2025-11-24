using UnityEngine;

namespace MyAssets.Framework
{
    public class BackgroundItemMover : MonoBehaviour
    {
        private BackgroundItemController controller;
        private Vector3 direction;
        private float moveSpeed;
        private float rotateSpeed;
        private int rotateDir;

        public void Init(BackgroundItemController ctrl)
        {
            controller = ctrl;
            // 随机方向指向中心区域
            direction = (ctrl.transform.position - transform.position).normalized + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0);
            moveSpeed = Random.Range(ctrl.moveSpeedMin, ctrl.moveSpeedMax);
            rotateSpeed = Random.Range(ctrl.rotateSpeedMin, ctrl.rotateSpeedMax);
            rotateDir = Random.value > 0.5f ? 1 : -1;
        }

        void Update()
        {
            // 移动
            transform.position += direction * (moveSpeed * Time.deltaTime);
            // 自转
            transform.Rotate(Vector3.forward, rotateDir * rotateSpeed * Time.deltaTime);
        }
    }
}


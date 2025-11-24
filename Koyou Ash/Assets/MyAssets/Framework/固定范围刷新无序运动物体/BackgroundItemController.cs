using System.Collections.Generic;
using UnityEngine;

namespace MyAssets.Framework
{
    public class BackgroundItemController : MonoBehaviour
    {
        // XY范围
        [Header("生成范围设置")] public Vector2 areaSize = new Vector2(10f, 5f);

        [Header("是否显示检测范围")] public bool showArea = true;

        [Header("范围颜色设置")] public Color areaColor = new Color(0f, 1f, 0f, 0.2f);

        //可以在Inspector中加减
        [Header("预制体设置")] public GameObject[] prefabs;

        //预制体同时存在的最大数量
        [Range(0, 100)] public int maxObjects = 10;

        [Header("移动速度随机范围")] public float moveSpeedMin = 0.5f;
        public float moveSpeedMax = 2f;

        [Header("旋转速度随机范围")] public float rotateSpeedMin = 10f;
        public float rotateSpeedMax = 50f;

        // 避免预制体重叠的最小距离
        public float minDistanceBetweenObjects = 0.5f;

        // 当前活动的预制体列表
        private List<GameObject> activeObjects = new List<GameObject>();

        void Start()
        {
            // 初始化时生成目标数量
            while (activeObjects.Count < maxObjects)
            {
                SpawnRandomObject();
            }
        }

        void Update()
        {
            // 检查并补充被销毁的对象
            for (int i = activeObjects.Count - 1; i >= 0; i--)
            {
                if (!activeObjects[i])
                {
                    activeObjects.RemoveAt(i);
                }
                else
                {
                    // 检查是否超出范围
                    Vector3 pos = activeObjects[i].transform.position;

                    if (!IsInsideArea(pos))
                    {
                        Destroy(activeObjects[i]);
                        activeObjects.RemoveAt(i);
                    }
                }
            }

            // 如果数量不足，则生成新对象
            while (activeObjects.Count < maxObjects)
            {
                SpawnRandomObject();
            }
        }

        void SpawnRandomObject()
        {
            if (prefabs == null || prefabs.Length == 0)
            {
                return;
            }

            // 随机选择一个预制体
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

            // 在边界随机选一个出生点
            Vector2 spawnPos = GetRandomEdgePosition();
            Vector3 worldPos = transform.position + new Vector3(spawnPos.x, spawnPos.y, 0);

            // 检查与现有对象的距离，避免重叠
            foreach (var obj in activeObjects)
            {
                if (!obj)
                {
                    continue;
                }

                if (Vector3.Distance(obj.transform.position, worldPos) < minDistanceBetweenObjects)
                {
                    return; // 太近就放弃这次生成
                }
            }

            GameObject newObj = Instantiate(prefab, worldPos, Quaternion.identity, transform);

            // 随机旋转角度与旋转方向
            float randomAngle = Random.Range(0f, 360f);
            newObj.transform.rotation = Quaternion.Euler(0, 0, randomAngle);

            // 添加控制组件,来设置移动和旋转速度
            BackgroundItemMover mover = newObj.AddComponent<BackgroundItemMover>();
            mover.Init(this);

            activeObjects.Add(newObj);
        }


        //判断预制体是否在生成范围内
        bool IsInsideArea(Vector3 pos)
        {
            Vector3 localPos = pos - transform.position;

            return (Mathf.Abs(localPos.x) <= areaSize.x / 2f) && (Mathf.Abs(localPos.y) <= areaSize.y / 2f);
        }


        //获取预制体初始位置,设定在长方形生成范围边界上随机位置
        Vector2 GetRandomEdgePosition()
        {
            // 随机选择边界：上下左右之一
            int edge = Random.Range(0, 4);
            float x = 0, y = 0;
            switch (edge)
            {
                case 0: // 上边
                    x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
                    y = areaSize.y / 2f;
                    break;
                case 1: // 下边
                    x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
                    y = -areaSize.y / 2f;
                    break;
                case 2: // 左边
                    x = -areaSize.x / 2f;
                    y = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
                    break;
                case 3: // 右边
                    x = areaSize.x / 2f;
                    y = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
                    break;
            }

            return new Vector2(x, y);
        }

        void OnDrawGizmos()
        {
            //判断是否显示范围
            if (!showArea)
            {
                return;
            }

            //指定范围颜色并绘制
            Gizmos.color = areaColor;
            Gizmos.DrawCube(transform.position, new Vector3(areaSize.x, areaSize.y, 0.01f));
        }
    }
}
using System.Collections.Generic;
using UnityEngine;


namespace EarthHitting
{
    public class BlueBarRolling : MonoBehaviour
    {
        [Header("地图预制体")]
        public GameObject mapPrefab;

        [Header("初始生成数量")]
        public int initialCount = 2;

        [Header("每个地图的宽度")]
        public float mapWidth = 10f;

        [Header("滚动速度")]
        public float scrollSpeed = 2f;

        private List<GameObject> activeMaps = new List<GameObject>();

        void Start()
        {
            for (int i = 0; i < initialCount; i++)
            {
                SpawnMap(i);
            }
        }

        void Update()
        {
            // 所有地图块左移
            foreach (var map in activeMaps)
            {
                map.transform.Translate(Vector3.left * (scrollSpeed * Time.deltaTime));
            }

            // 检查第一个地图是否完全移出视野
            GameObject firstMap = activeMaps[0];
            if (firstMap.transform.position.x <= -mapWidth)
            {
                // 销毁最旧的地图
                Destroy(firstMap);
                activeMaps.RemoveAt(0);

                // 在最右边生成新的地图
                float newX = activeMaps[^1].transform.position.x + mapWidth;
                SpawnMapAt(newX);
            }
        }

        void SpawnMap(int index)
        {
            float xPos = index * mapWidth;
            GameObject map = Instantiate(mapPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            activeMaps.Add(map);
        }

        void SpawnMapAt(float xPos)
        {
            GameObject map = Instantiate(mapPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            activeMaps.Add(map);
        }
    }
}



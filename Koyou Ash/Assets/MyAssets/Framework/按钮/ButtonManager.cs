using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MyAssets.Framework
{
    public class ButtonManager : MonoBehaviour
    {
        public static ButtonManager Instance;

        [Header("按钮组")] 
        public Button[] buttons; // 存放所有需要统一管理的按钮

        private void Awake()
        {
            // 单例模式
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);

            // 检查是否绑定
            if (buttons == null || buttons.Length == 0)
            {
                Debug.LogWarning("ButtonManager: 未绑定任何按钮！");
            }
        }

      
        //禁用除点击按钮外的其他所有按钮
        public void DisableOther(Button clickedButton)
        {
            if (buttons == null || buttons.Length == 0)
            {
                return;
            }

            foreach (Button btn in buttons)
            {
                if (btn == null)
                {
                    continue;
                }

                if (btn != clickedButton)
                {
                    btn.interactable = false;
                }
            }
        }

      
        /// 加载场景（可延时）
        public void LoadScene(string sceneName, float delay = 0f)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName, delay));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, float delay)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadSceneAsync(sceneName);
            }
            else
            {
                Debug.LogWarning("ButtonManager: 未设置目标场景名！");
            }
        }
    }
}
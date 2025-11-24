using UnityEngine;
using UnityEngine.UI;

namespace MyAssets.Framework
{
    public class StartButton : ButtonScale, ISceneLoader
    {
        public string targetSceneName = "EarthHittingGameScene";

        protected override void Awake()
        {
            base.Awake();
            scaleMultiplier = 1.15f;

            if (btn == null) btn = GetComponent<Button>();

            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OnClick);
            }
            else
            {
                Debug.LogError($"{name}: StartButton 缺少 Button 组件！");
            }
        }

        private void OnClick()
        {
            btn.interactable = false;
            ButtonManager.Instance.DisableOther(btn);
            SwitchScene(1f);
        }

        public void SwitchScene(float delay = 0f)
        {
            ButtonManager.Instance.LoadScene(targetSceneName, delay);
        }
    }
}
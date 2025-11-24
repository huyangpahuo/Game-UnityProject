using UnityEngine;
using UnityEngine.UI;

namespace MyAssets.Framework
{
    public class ExitButton : ButtonScale, ISceneLoader
    {
        public string targetSceneName = "TVSelectionScene";
        private IButtonEscape escapeLogic;

        protected override void Awake()
        {
            base.Awake();
            scaleMultiplier = 0.85f;

            if (btn == null) btn = GetComponent<Button>();
            btn?.onClick.RemoveAllListeners();
            btn?.onClick.AddListener(OnClick);

            escapeLogic = GetComponent<IButtonEscape>();
        }

        void Update()
        {
            base.Update();
            escapeLogic?.HandleEscape(transform as RectTransform, GetComponentInParent<Canvas>());
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
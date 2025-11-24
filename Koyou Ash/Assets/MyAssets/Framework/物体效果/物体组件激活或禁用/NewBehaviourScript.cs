using UnityEngine;
using UnityEngine.UI;

namespace MyAssets.Framework
{
    public class NewBehaviourScript : MonoBehaviour
    {
        private Button btn;
        public 

        void Start()
        {
            btn = GetComponent<Button>();
            // 确保在 Inspector 中拖入了按钮
            if (btn != null)
            {
                btn.onClick.AddListener(OnButtonClick);
            }
        }


        private void OnButtonClick()
        {
            
        }
    }
}
using UnityEngine;


namespace  MyAssets.Framework
{
    public class SetActiveObject : MonoBehaviour
    {
        public GameObject myObject;
        bool isActive = false;
        
        void Update()
        {
            OpenObject();
        }
        
        void OpenObject()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isActive=!isActive;
                myObject.SetActive(isActive);
            }
        }
    }

}

using UnityEngine.UI;
using UnityEngine;


namespace MyAssets.Framework
{
    public class VolumeController : MonoBehaviour
    {
        //需要控制的声音
        private AudioSource MenuAudio;
        
        //获取到滑动条
        public Slider VolumeSlider;
        void Start()
        {
            MenuAudio = GameObject.FindGameObjectWithTag("Menu").transform.GetComponent<AudioSource>();
            VolumeSlider=GameObject.FindGameObjectWithTag("GameSetting").transform.GetComponent<Slider>();
        }
        
        void Update()
        {
            VolumeControl();
        }
        
        //控制声音音效
        public void VolumeControl()
        {
            MenuAudio.volume = VolumeSlider.value;
        }
        
    }
}




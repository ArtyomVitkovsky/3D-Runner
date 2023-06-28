using Modules.MainModule.Scripts.Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Content.Scripts.Sound
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(AudioClip))]
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField] private AudioDefault category;
        
        
        private SoundControl soundControl;
        [Inject]
        private void Construct(SoundControl soundControl)
        {
            this.soundControl = soundControl;
        }

        

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }

            if (category == AudioDefault.Custom)
            {
                category = AudioDefault.ClickButton;
            }
        }
#endif

        private void OnEnable()
        {
            button.onClick.AddListener(HandlePlaySound);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(HandlePlaySound);
        }

        private void HandlePlaySound()
        {
            if (soundControl != null)
            {
                soundControl.PlayEffect(category);
            }
        }
    }
}
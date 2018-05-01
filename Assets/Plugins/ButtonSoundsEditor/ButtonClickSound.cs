using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Plugins.ButtonSoundsEditor
{
    public class ButtonClickSound : MonoBehaviour, IPointerClickHandler
    {
        public AudioSource AudioSource;
        public AudioClip ClickSound;

		void Awake(){
			AudioSource = GameObject.FindGameObjectWithTag ("ClickSound").GetComponent<AudioSource> ();
		}

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayClickSound();
        }

        private void PlayClickSound()
        {
            AudioSource.PlayOneShot(ClickSound);
        }
    }

}

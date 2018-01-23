using UnityEngine;
using UnityEngine.UI;
public class UpdateDescriptionText : MonoBehaviour {
    public static Text descriptionText;
  
    void Start () {
        descriptionText = GetComponent<Text>();
        descriptionText.text = "";
    }

	
}

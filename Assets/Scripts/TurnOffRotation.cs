using UnityEngine;

public class TurnOffRotation : MonoBehaviour {

     Animation walkAnim;
    public GameObject enemy;

    void Awake() {
        walkAnim = enemy.GetComponent<Animation>();

    }

    public void AnimSwitch() {
        walkAnim.enabled = !walkAnim.enabled;
    }
}

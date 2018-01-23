using UnityEngine;

public class EnemyLightFollow : MonoBehaviour {
    public Transform torch,torchAnim;
    public GameObject enemy;

	void Start () {
	
	}
	

	void Update () {
        if(enemy.activeSelf==true)
        transform.position = torchAnim.position;
        else
            transform.position = torch.position;
    }
}

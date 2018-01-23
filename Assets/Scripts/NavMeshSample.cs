using UnityEngine;

public class NavMeshSample : MonoBehaviour {
   
   
    Transform farEnd;
    private Vector3 fromThe;
    private Vector3 unToThe;
    public float secondsForOneLength = 20f;
    Quaternion a,b;
    public Animation anim;
   
    bool isturned = false;
   
    void Start()
    {

        unToThe = transform.GetChild(0).position;

        fromThe = transform.position;

        
        a = Quaternion.Euler(new Vector3(0, transform.rotation.y + 180 , 0));
        b = Quaternion.Euler(new Vector3(0, transform.rotation.y , 0));
    }

    void Update()
    {
        transform.position = Vector3.Lerp(fromThe, unToThe,
         Mathf.SmoothStep(0f, 1f,
          Mathf.PingPong(Time.time / secondsForOneLength, 1f)
        ));
      
        float distance = Vector3.Distance(transform.position, unToThe);
        float distance2 = Vector3.Distance(fromThe, transform.position);

        if (distance < 0.4f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, a, Time.deltaTime * 150f);
            isturned = true;
        }
          
        if (distance2 < 0.4f && isturned)
        {
            anim["Take 001"].speed = 0.2f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, b, Time.deltaTime * 150f);
        }
        if (distance2 < 0.5f || (distance < 0.5f))
        {
            if (anim["Take 001"].speed != 0)
            anim["Take 001"].speed = 0;
        }
        else
        {
            if (anim["Take 001"].speed != 0.825f)
            anim["Take 001"].speed = 0.825f;
        }
    }
  
}

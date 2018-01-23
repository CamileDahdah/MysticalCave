var speed:float =1f ;
function Update() {
  
    transform.Rotate(Vector3.up * Time.deltaTime*speed,Space.World);
}
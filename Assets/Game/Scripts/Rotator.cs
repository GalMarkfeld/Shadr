    using UnityEngine;

public class Rotator : MonoBehaviour {

	public float speed = 20f;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
        //transform.position -= (new Vector3(speed, speed, 0) * Time.deltaTime);
        //transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}

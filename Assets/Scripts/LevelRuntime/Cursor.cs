using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    Transform target;
    public Camera cam;
    public float speed = 2.5f;

	void Start () {
        target = GetComponent<Transform>();
	}
	

	void Update () {

        //target.position += Vector3.right * 0.01F;
        //Debug.Log(Screen.height);
        //Debug.Log(Screen.width);
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        //Debug.Log("target is " + screenPos.x + " pixels from the left");
    }

    private void LateUpdate()
    {
        Vector3 pos = cam.WorldToScreenPoint(target.position);
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
        target.position = cam.ScreenToWorldPoint(pos);
    }

    public void Move(float x, float y)
    {
        target.position += new Vector3(x*-1*speed, y*speed, 0);
    }
}

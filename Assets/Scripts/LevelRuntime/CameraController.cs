using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Vector3 offset = new Vector3(0, 0, -20);
    private static CameraController cam;
    // Update is called once per frame

    private void Awake()
    {
        if (cam == null)
        {
            cam = this;
            Debug.Log("Dont destroy the camera");
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this.gameObject);
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

        /*
        const int orthographicSizeMin = 1;
        const int orthographicSizeMax = 50;


        if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
 {
            Camera.main.orthographicSize++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
 {
            Camera.main.orthographicSize--;
        }
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);*/
    }

}

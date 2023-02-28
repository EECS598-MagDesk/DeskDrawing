using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{

    public Vector3 position;
    public Vector3 rotation;
    private float screenSizeHalf = 5f;
    private Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        cam.fieldOfView = 2f * Mathf.Atan(screenSizeHalf / position.z) / Mathf.PI * 180;
        transform.position = new Vector3(position.x, position.y, 0.1f);
        transform.localEulerAngles = new Vector3(Mathf.Atan(position.y / position.z) / Mathf.PI * 180, Mathf.Atan(position.x / position.z) / Mathf.PI * 180, 0);
    }
}

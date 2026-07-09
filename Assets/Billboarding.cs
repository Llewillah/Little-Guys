using UnityEngine;

public class Billboarding : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    private void LateUpdate()
    {
        transform.forward = cam.transform.forward;


        //Other version
        // transform.LookAt(cam.transform.position, Vector3.up);
    }
}

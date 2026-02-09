using UnityEngine;

public class Magnify : MonoBehaviour
{
    public Transform magnefying;
    public Transform lens;
    public Camera mag_camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        magnefying.LookAt(lens);

        Vector3 newRotation = lens.eulerAngles;
        newRotation.z = 0;
        lens.eulerAngles = newRotation;

        mag_camera.nearClipPlane = Vector3.Distance(magnefying.position, lens.position);
    }
}

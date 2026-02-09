using UnityEngine;

public class Magnify : MonoBehaviour
{
    public Transform magnefying;
    public Transform lens;
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
    }
}

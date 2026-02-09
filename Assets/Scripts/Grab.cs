using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grab : MonoBehaviour
{
    Grab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    bool grabbing = false;
    Vector3 lastPosition;
    Quaternion lastRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action.action.Enable();

            foreach (Grab c in transform.parent.GetComponentsInChildren<Grab>())
            {
                if (c != this)
                {
                    otherHand = this;
                }
            }

            lastPosition = transform.position;
            lastRotation = transform.rotation;
    }


    // Update is called once per frame
    void Update()
    {
        grabbing = action.action.IsPressed();

        if (grabbing)
        {
            if (!grabbedObject)
            {
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;
            }

            if (grabbedObject)
            {
                Vector3 deltaPos = transform.position - lastPosition;
                Quaternion deltaRot = transform.rotation * Quaternion.Inverse(lastRotation);

                grabbedObject.position += deltaPos;
                grabbedObject.rotation = deltaRot * grabbedObject.rotation;
            }
        }

        else if (grabbedObject)
        {
            grabbedObject = null;
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform t = other.transform;

        if (t && t.tag.ToLower() == "grabbable")
        {
            nearObjects.Add(t);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;

        if (t && t.tag.ToLower() == "grabbable")
        {
            nearObjects.Remove(t);
        }
    }
}

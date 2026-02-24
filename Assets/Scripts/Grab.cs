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
    public InputActionReference other_action;
    bool grabbing = false;
    bool other_grabbing = false;
    Vector3 lastPosition;
    Quaternion lastRotation;
    public Transform controller;
    public Transform other_controller;
    Vector3 position;
    Vector3 velocity;
    private Collider[] handColliders;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action.action.Enable();
        other_action.action.Enable();

        handColliders = GetComponentsInChildren<Collider>();

            foreach (Grab c in transform.parent.GetComponentsInChildren<Grab>())
            {
                if (c != this)
                {
                    otherHand = c;
                }
            }

            lastPosition = transform.position;
            lastRotation = transform.rotation;
    }


    // Update is called once per frame
    void Update()
    {
        grabbing = action.action.IsPressed();
        other_grabbing = other_action.action.IsPressed();
        bool bothHands = grabbing && other_grabbing;

        if (bothHands || grabbing)
        {
            if (!grabbedObject)
            {
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;
            }

            if (grabbedObject)
            {
                foreach (Collider col in handColliders)
                {
                    col.enabled = false;
                }
                
                Collider[] objectColliders = grabbedObject.GetComponentsInChildren<Collider>();
                if (objectColliders != null)
                {
                    foreach (Collider objCol in objectColliders)
                    {
                        foreach (Collider handCol in handColliders)
                        {
                            Physics.IgnoreCollision(handCol, objCol, true);
                        }
                    }
                }

                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.isKinematic = true;
                }

                Vector3 deltaPos = transform.position - lastPosition;
                Quaternion deltaRot = transform.rotation * Quaternion.Inverse(lastRotation);

                grabbedObject.position += deltaPos;
                grabbedObject.rotation = deltaRot * grabbedObject.rotation;
                
                velocity = (transform.position - lastPosition) / Time.deltaTime;

                if (bothHands)
                {
                    position.x = (controller.position.x + other_controller.position.x) / 2;
                    position.y = (controller.position.y + other_controller.position.y) / 2;
                    position.z = (controller.position.z + other_controller.position.z) / 2;
                } 
                else
                {
                    position.x = controller.position.x;
                    position.y = controller.position.y;
                    position.z = controller.position.z;
                }

                grabbedObject.position = position;

            }
        } 

        else if (grabbedObject)
        {

            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = false;
                rb.linearVelocity = velocity;
                rb.angularVelocity = (transform.rotation * Quaternion.Inverse(lastRotation)).eulerAngles / Time.deltaTime;
            }

            foreach (Collider col in handColliders)
            {
                col.enabled = true;
            }

            Collider[] objectColliders = grabbedObject.GetComponentsInChildren<Collider>();
            if (objectColliders != null)
            {
                foreach (Collider objCol in objectColliders)
                {
                    foreach (Collider handCol in handColliders)
                    {
                        Physics.IgnoreCollision(handCol, objCol, false);
                    }
                }
            }

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

using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private GameObject heldObject;

    void Grab(GameObject obj)
    {
        heldObject = obj;
        obj.transform.SetParent(transform);
    }

    void Release()
    {
        heldObject.transform.SetParent(null);
        heldObject = null;
    }
}

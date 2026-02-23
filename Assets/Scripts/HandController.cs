using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputActionReference grip;
    public InputActionReference trigger;
    public Hand hand;
    void Start()
    {
        grip.action.Enable();
        trigger.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
       hand.SetGrip(grip.action.ReadValue<float>());
       hand.SetTrigger(trigger.action.ReadValue<float>());
    }
}

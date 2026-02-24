using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    public InputActionReference action;

    void Start()
    {

    }

    void Update()
    {
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            #if UNITY_EDITOR
            SceneManager.LoadScene(0);
            #else
            Application.Quit();
            #endif
        };
    }
}

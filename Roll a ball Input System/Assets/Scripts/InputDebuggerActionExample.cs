using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebuggerActionExample : MonoBehaviour
{
    public InputAction exampleAction;
    
    void OnEnable()
    {
       exampleAction.Enable();
    }
}
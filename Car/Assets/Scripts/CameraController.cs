using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour, InputActions.ICameraControlsActions
{
    private InputActions controls;
    [SerializeField] private Camera perspectiveCamera;
    [SerializeField] private Camera topCamera;

    public void OnFovDown(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            this.perspectiveCamera.fieldOfView -= 10;
        }
    }

    public void OnFovUp(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            this.perspectiveCamera.fieldOfView += 10;
        }
    }

    public void OnSwitchCameraOrtographic(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            this.perspectiveCamera.enabled = false;
            this.topCamera.enabled = true;
        }
    }

    public void OnSwitchCameraPerspective(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            this.perspectiveCamera.enabled = true;
            this.topCamera.enabled = false;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        this.controls = new InputActions();
        this.controls.CameraControls.SetCallbacks(this);
        this.controls.CameraControls.Enable();

        this.topCamera.enabled = false;
        this.perspectiveCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }


}

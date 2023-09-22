using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputActions.IPlayerActions
{

    
    private InputActions controls;
    private Vector2 movementInput;
    private Rigidbody rb;
    [SerializeField] private float maxSpeed = 20;

    [SerializeField] private float forwardForceMultiplyer = 1000;
    [SerializeField] private float turnMultiplyer = 2000;


    [SerializeField] private GameObject[] frontWheels;
    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.controls = new InputActions();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        TurnWheels();
    }

    void TurnWheels() {
        // if turning, turn wheels 10degrees to left or right, no more
        if (this.movementInput.x != 0) {
            foreach (GameObject wheel in frontWheels) {
                wheel.transform.localRotation = Quaternion.Euler(0, 20*this.movementInput.x, 90);
            }
        } else {
            foreach (GameObject wheel in frontWheels) {
                wheel.transform.localRotation = Quaternion.Euler(0, 0,  90);
            }
        }
    }
    void FixedUpdate()
    {
        ApplyAcceleration();
        ApplyTurn();
    }

    private void ApplyTurn()
    {
        Vector3 force = this.turnMultiplyer * this.movementInput.x * Time.deltaTime * this.transform.up;
        this.rb.AddTorque(force);
    }

    private void ApplyAcceleration()
    { //delta time is time between frames, must multiply by it to make sure it is framerate independent
        Vector3 force = forwardForceMultiplyer * this.movementInput.y * Time.deltaTime * this.transform.forward;
        this.rb.AddForce(force, ForceMode.Acceleration);
        this.rb.velocity = Vector3.ClampMagnitude(this.rb.velocity, this.maxSpeed);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
       this.movementInput = context.ReadValue<Vector2>();
    }

}

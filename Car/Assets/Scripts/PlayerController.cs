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

    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider backLeft;
    [SerializeField] private WheelCollider backRight;

    [SerializeField] private GameObject[] frontWheels;

    private float currentAcceleration = 0;
    private float currentBreak = 0;
    private float currentTurnAngle = 0;
    [SerializeField] private float brakingForce = 1000;
    [SerializeField] private float maxTurnAngle = 35;

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
        // Vector3 force = this.turnMultiplyer * this.movementInput.x * Time.deltaTime * this.transform.up;
        // this.rb.AddTorque(force);
        // if (this.movementInput.y != 0) {
        //     this.frontLeft.steerAngle = this.movementInput.x * 45;
        //     this.frontRight.steerAngle = this.movementInput.x * 45;
        // }
        currentTurnAngle = maxTurnAngle * this.movementInput.x;
        this.frontLeft.steerAngle = currentTurnAngle;
        this.frontRight.steerAngle = currentTurnAngle;
        



    }

    private void ApplyAcceleration()
    { //delta time is time between frames, must multiply by it to make sure it is framerate independent
        // Vector3 force = forwardForceMultiplyer * this.movementInput.y * Time.deltaTime * this.transform.forward;
        // this.rb.AddForce(force, ForceMode.Acceleration);
        // this.rb.velocity = Vector3.ClampMagnitude(this.rb.velocity, this.maxSpeed);

        // this.frontLeft.motorTorque = this.movementInput.y * this.forwardForceMultiplyer;
        // this.frontRight.motorTorque = this.movementInput.y * this.forwardForceMultiplyer;
        // this.backLeft.motorTorque = this.movementInput.y * this.forwardForceMultiplyer;
        // this.backRight.motorTorque = this.movementInput.y * this.forwardForceMultiplyer;

        if (this.movementInput.y < 0 && this.rb.velocity.magnitude > 0.1) {
            this.currentBreak = this.brakingForce;
        } else {
            this.currentBreak = 0;
        }

        this.currentAcceleration = this.forwardForceMultiplyer * this.movementInput.y;

        this.frontLeft.motorTorque = this.currentAcceleration * this.forwardForceMultiplyer;
        this.frontRight.motorTorque = this.currentAcceleration * this.forwardForceMultiplyer;
        this.backLeft.motorTorque = this.currentAcceleration * this.forwardForceMultiplyer;
        this.backRight.motorTorque = this.currentAcceleration * this.forwardForceMultiplyer;

        this.frontLeft.brakeTorque = this.currentBreak * this.forwardForceMultiplyer;
        this.frontRight.brakeTorque = this.currentBreak * this.forwardForceMultiplyer;
        this.backLeft.brakeTorque = this.currentBreak * this.forwardForceMultiplyer;
        this.backRight.brakeTorque = this.currentBreak * this.forwardForceMultiplyer;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
       this.movementInput = context.ReadValue<Vector2>();
    }

}

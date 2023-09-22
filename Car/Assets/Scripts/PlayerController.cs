using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputActions.IPlayerActions
{

    
    private InputActions controls;
    private Vector2 movement;
    private Rigidbody rb;

    [SerializeField] private frontOfCar GameObject;
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
    }


    void FixedUpdate()
    {
        applyAcceleration();
        applyTurn();

    }

    private void applyTurn()
    {
        Vector3 force = this.turnMultiplyer * this.movement.x * Time.deltaTime * this.transform.up;
        this.rb.AddTorque(force);
    }

    private void applyAcceleration()
    { //delta time is time between frames, must multiply by it to make sure it is framerate independent
        Vector3 force = forwardForceMultiplyer * this.movement.y * Time.deltaTime * this.transform.forward;
        this.rb.AddForce(force, ForceMode.Acceleration);
        this.rb.velocity = Vector3.ClampMagnitude(this.rb.velocity, this.maxSpeed);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
       this.movement = context.ReadValue<Vector2>();
    }
}

internal class frontOfCar
{
}
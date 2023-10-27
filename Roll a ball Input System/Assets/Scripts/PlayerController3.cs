using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerController3 : MonoBehaviour
{

    PlayerControls controls;
    Vector2 move;
    public float speed = 10;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Buttons.performed += context => SendMessage("Button Pressed");
        controls.Player.Move.performed += ctx => SendMessage(ctx.ReadValue<Vector2>().ToString());
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
    }

    private void SendMessage(String str)
    {
        Debug.Log(str);
    }



    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }




    void FixedUpdate()
    {
        Vector3 movement = speed * Time.deltaTime * new Vector3(move.x, 0f, move.y);
        transform.Translate(movement,  Space.World);
    }
}

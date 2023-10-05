using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class handles the movement of the player with given input from the input manager
/// </summary>
public class PlayerController : MonoBehaviour
{

    [Header("Settings")]
    [Tooltip("The speed at which the player moves")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("The speed at which the player rotates(degrees)")]
    [SerializeField] private float lookSpeed = 60f;
    [Tooltip("The speed at which the player jumps")]
    [SerializeField] private float jumpPower = 8f;
    [Tooltip("The strength of gravity")]
    [SerializeField] private float gravity = 9.81f;

    [Header("Jump Timing")]
    [SerializeField] private float jumpTimeLeniency = 0.1f;
    private float timeToStopBeingLenient = 0f;


    [Header("Required References")]
    [Tooltip("The playershooter script that fires projectiles")]
    public Shooter playerShooter;
    public Health playerHealth;
    public List<GameObject> disableWhileDead;

    private bool doubleJumpAvailable = false;

    // The character controller component on the player
    private CharacterController controller;
    private InputManager inputManager;

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first Update call
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Start()
    {
        SetUpCharacterController();
        SetUpInputManager();
    }

    private void SetUpCharacterController()
    {
        this.controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("PlayerController: No CharacterController component found on this object");
        }
    }

    private void SetUpInputManager()
    {
        this.inputManager = InputManager.instance;
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once every frame
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Update()
    {


        if (playerHealth.currentHealth <= 0)
        {
            foreach (GameObject inGameObject in this.disableWhileDead)
            {
                inGameObject.SetActive(false);
            }
            return;
        }
        else
        {
            foreach (GameObject inGameObject in this.disableWhileDead)
            {
                inGameObject.SetActive(true);
            }
        }
        ProcessMovement();
        ProcessRotation();

    }

    private void FixedUpdate()
    {
    }

    Vector3 moveDirection;
    private void ProcessMovement()
    {
        float leftRightInput = this.inputManager.horizontalMoveAxis;
        float forwardBackInput = this.inputManager.verticalMoveAxis;
        // Debug.Log("LeftRightInput: " + leftRightInput + " ForwardBackInput: " + forwardBackInput);
        bool jumpPressed = this.inputManager.jumpPressed;
        // Debug.Log("JumpPressed: " + jumpPressed);

        if (controller.isGrounded)
        {
            this.doubleJumpAvailable = true;
            this.timeToStopBeingLenient = Time.time + jumpTimeLeniency;

            moveDirection = new Vector3(leftRightInput, 0, forwardBackInput);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;
            if (jumpPressed)
            {
                // Debug.Log("Jumping");
                moveDirection.y = jumpPower;
            }
        }
        else
        {
            moveDirection = new Vector3(leftRightInput * moveSpeed, moveDirection.y, forwardBackInput * moveSpeed);
            moveDirection = transform.TransformDirection(moveDirection);

            if (jumpPressed && this.timeToStopBeingLenient > Time.time)
            {
                moveDirection.y = jumpPower;
            }
            else if (jumpPressed && doubleJumpAvailable)
            {
                // Debug.Log("Double Jumping");
                moveDirection.y = jumpPower;
                doubleJumpAvailable = false;
            }

        }
        moveDirection.y -= gravity * Time.deltaTime;
        if (controller.isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -0.3f; // prevent gravity from accumulating...
        }

        this.controller.Move(moveDirection * Time.deltaTime);
    }

    private void ProcessRotation()
    {
        float horizontalLookInput = inputManager.horizontalLookAxis;
        Vector3 playerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(playerRotation.x, playerRotation.y + horizontalLookInput * lookSpeed * Time.deltaTime, playerRotation.z);
    }
}

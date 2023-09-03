using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject pickUpsParent; // parent object of all pick ups
    private int totalPickUps;

    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int collectableCount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collectableCount = 0;
        totalPickUps = pickUpsParent.transform.childCount;
        winTextObject.SetActive(false);
        SetCountText();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = $"Count: {collectableCount} / {totalPickUps}";
        if (collectableCount >= totalPickUps )
        {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            collectableCount++;
            SetCountText();
        }

    }
}

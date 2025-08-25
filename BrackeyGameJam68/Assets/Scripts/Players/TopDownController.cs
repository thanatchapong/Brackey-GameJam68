using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Maximum movement speed")]
    public float moveSpeed = 5f;

    [Tooltip("Acceleration for smooth movement")]
    public float acceleration = 10f;

    [Tooltip("Deceleration when no input is given")]
    public float deceleration = 10f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontalInput, verticalInput).normalized;
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            currentVelocity = Vector2.Lerp(currentVelocity, movement * moveSpeed, acceleration * Time.fixedDeltaTime);
            rb.velocity = currentVelocity;
        }
        else
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rb.velocity = currentVelocity;
        }
    }
}


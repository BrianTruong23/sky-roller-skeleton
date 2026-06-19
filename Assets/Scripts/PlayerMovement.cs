using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float forwardSpeed = 5f;

    float sideSpeed = 6f;

    Rigidbody rb;

    Vector2 moveInput; 

    float currentSideInput;

    float sideVelocity; 

    float originalForwardSpeed;



    void Start()
    {
        rb = GetComponent<Rigidbody>();

        originalForwardSpeed = forwardSpeed;

        
    }

    public void ActivateSpeedBost(float boostSpeed, float duration)
    {
        StartCoroutine(SpeedBoostRountine(boostSpeed, duration));
        
    }

    private IEnumerator SpeedBoostRountine(float boostSpeed, float duration)
    {
        forwardSpeed = boostSpeed;
        yield return new WaitForSeconds(duration);
        forwardSpeed = originalForwardSpeed;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        currentSideInput = Mathf.SmoothDamp(
            currentSideInput,
            moveInput.x,
            ref sideVelocity,
            0.1f
        );

        Vector3 movement = new Vector3(currentSideInput * sideSpeed, rb.linearVelocity.y, forwardSpeed);
        rb.linearVelocity = movement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

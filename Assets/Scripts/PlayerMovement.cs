using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float forwardSpeed = 5f;
    float sideSpeed = 6f;

    Rigidbody rb;
    Vector2 moveInput;
    float currentSideInput;
    float sideVelocity;

    float originalForwardSpeed;
    float originalSideSpeed;

    float forwardMultiplier = 1f;
    float sideMultiplier = 1f;

    bool controlsLocked;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalForwardSpeed = forwardSpeed;
        originalSideSpeed = sideSpeed;
    }

    public void ActivateSpeedBost(float boostSpeed, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(boostSpeed, duration));
    }

    IEnumerator SpeedBoostRoutine(float boostSpeed, float duration)
    {
        forwardSpeed = boostSpeed;
        yield return new WaitForSeconds(duration);
        forwardSpeed = originalForwardSpeed;
    }

    public void SetMovementMultipliers(float forwardMult, float sideMult)
    {
        forwardMultiplier = forwardMult;
        sideMultiplier = sideMult;
    }

    public void ResetMovementMultipliers()
    {
        forwardMultiplier = 1f;
        sideMultiplier = 1f;
    }

    public void StopBriefly(float stopDuration)
    {
        StartCoroutine(StopBrieflyRoutine(stopDuration));
    }

    IEnumerator StopBrieflyRoutine(float stopDuration)
    {
        controlsLocked = true;
        rb.linearVelocity = Vector3.zero;

        yield return new WaitForSeconds(stopDuration);

        controlsLocked = false;
    }

    public void LaunchUp(float upwardSpeed, float forwardBoost, float duration)
    {
        StartCoroutine(LaunchRoutine(upwardSpeed, forwardBoost, duration));
    }

    IEnumerator LaunchRoutine(float upwardSpeed, float forwardBoost, float duration)
    {
        float boostedForward = originalForwardSpeed + forwardBoost;
        float endTime = Time.time + duration;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, upwardSpeed, boostedForward);

        while (Time.time < endTime)
        {
            forwardSpeed = boostedForward;
            yield return null;
        }

        forwardSpeed = originalForwardSpeed;
    }

    void OnMove(InputValue value)
    {
        if (!controlsLocked)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    void FixedUpdate()
    {
        if (controlsLocked)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        currentSideInput = Mathf.SmoothDamp(
            currentSideInput,
            moveInput.x,
            ref sideVelocity,
            0.1f
        );

        float currentForward = forwardSpeed * forwardMultiplier;
        float currentSide = sideSpeed * sideMultiplier;

        Vector3 movement = new Vector3(
            currentSideInput * currentSide,
            rb.linearVelocity.y,
            currentForward
        );

        rb.linearVelocity = movement;
    }
}

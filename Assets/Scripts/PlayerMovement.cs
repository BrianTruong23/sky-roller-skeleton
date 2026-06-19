using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float forwardSpeed = 5f;
    float sideSpeed = 6f;
    [SerializeField] float jumpSpeed = 7f;
    [SerializeField] float groundCheckDistance = 0.65f;
    [SerializeField] LayerMask groundLayers = ~0;

    Rigidbody rb;
    Vector2 moveInput;
    float currentSideInput;
    float sideVelocity;

    float originalForwardSpeed;
    float originalSideSpeed;

    float forwardMultiplier = 1f;
    float sideMultiplier = 1f;

    int slowZoneCount;
    int forwardBlockCount;

    const float SlowZoneMinForward = 2.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalForwardSpeed = forwardSpeed;
        originalSideSpeed = sideSpeed;
        ControlHintUI.ShowDefaultControls();
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

    public void EnterSlowZone(float forwardMult, float sideMult)
    {
        slowZoneCount++;
        ApplySlowEffect(forwardMult, sideMult);
    }

    public void ApplySlowEffect(float forwardMult, float sideMult)
    {
        forwardMultiplier = forwardMult;
        sideMultiplier = sideMult;
    }

    public void ExitSlowZone()
    {
        slowZoneCount = Mathf.Max(0, slowZoneCount - 1);
        if (slowZoneCount == 0)
        {
            ResetMovementMultipliers();
        }
    }

    public bool IsInSlowZone => slowZoneCount > 0;

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

    public void AddForwardBlock()
    {
        forwardBlockCount++;
    }

    public void RemoveForwardBlock()
    {
        forwardBlockCount = Mathf.Max(0, forwardBlockCount - 1);
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
        moveInput = value.Get<Vector2>();
    }

    void OnJump()
    {
        TryJump();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryJump();
        }
    }

    void TryJump()
    {
        if (!IsGrounded())
        {
            return;
        }

        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(velocity.x, jumpSpeed, velocity.z);
    }

    void FixedUpdate()
    {
        currentSideInput = Mathf.SmoothDamp(
            currentSideInput,
            moveInput.x,
            ref sideVelocity,
            0.1f
        );

        float currentForward = forwardSpeed * forwardMultiplier;
        if (IsInSlowZone)
        {
            currentForward = Mathf.Max(currentForward, SlowZoneMinForward);
        }

        if (forwardBlockCount > 0)
        {
            if (IsTouchingTreeBlocker())
            {
                currentForward = 0f;
            }
            else
            {
                forwardBlockCount = 0;
            }
        }

        float currentSide = sideSpeed * sideMultiplier;

        Vector3 movement = new Vector3(
            currentSideInput * currentSide,
            rb.linearVelocity.y,
            currentForward
        );

        rb.linearVelocity = movement;
    }

    bool IsTouchingTreeBlocker()
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, 0.6f);
        foreach (Collider overlap in overlaps)
        {
            if (overlap.GetComponent<TreeBlocker>() != null)
            {
                return true;
            }
        }

        return false;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore);
    }
}

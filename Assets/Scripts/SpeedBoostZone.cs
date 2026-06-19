using UnityEngine;

public class SpeedBoostZone : MonoBehaviour
{

    float boostSpeed = 15f; 
    float boostDuration = 2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("Player"))
        {
            return;
        }

        Rigidbody rb = collider.attachedRigidbody;
        if (rb == null)
        {
            return;
        }

        PlayerMovement playerMovement = rb.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            EffectNotificationUI.Show("Speed boosted!");
            playerMovement.ActivateSpeedBost(boostSpeed, boostDuration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

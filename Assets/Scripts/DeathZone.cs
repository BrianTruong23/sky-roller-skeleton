using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] GameOverUI gameOverUI;
    [SerializeField] Transform player;
    [SerializeField] bool followPlayer = true;
    [SerializeField] float zOffset = 0f;

    bool hasLost;
    float fixedX;
    float fixedY;

    void Awake()
    {
        fixedX = transform.position.x;
        fixedY = transform.position.y;

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (gameOverUI == null)
        {
            gameOverUI = FindAnyObjectByType<GameOverUI>();
        }
    }

    void LateUpdate()
    {
        if (!followPlayer || player == null)
        {
            return;
        }

        transform.position = new Vector3(fixedX, fixedY, player.position.z + zOffset);
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (hasLost || !collider.CompareTag("Player"))
        {
            return;
        }

        hasLost = true;
        EffectNotificationUI.Show("You fell off the path!");
        if (gameOverUI != null)
        {
            gameOverUI.showGameOver();
        }
        Time.timeScale = 0f;
    }
}

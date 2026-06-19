using TMPro;
using UnityEngine;

public class SurvivalScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    bool isRunning = true;
    public void StopTimer()
    {
        isRunning = false;
    }

    float elapsedTime;

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            int score = Mathf.FloorToInt(elapsedTime);

            scoreText.text = $"Score: {score}";
        }

    }
}

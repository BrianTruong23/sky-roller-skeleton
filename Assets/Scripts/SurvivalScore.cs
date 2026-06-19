using TMPro;
using UnityEngine;

public class SurvivalScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    float elapsedTime;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int score = Mathf.FloorToInt(elapsedTime);

        scoreText.text = $"Score: {score}";

    }
}

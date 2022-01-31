using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public static ScoreTracker S { get; private set; }
    [SerializeField] TMPro.TMP_Text scoreText;
    [SerializeField] TMPro.TMP_Text highScore;
    float _currentScore;

    void Awake() => S = this;

    void Start()
    {
        scoreText.text = $"Score: {0}";
        if (PlayerPrefs.HasKey("HighScore"))
            highScore.text = $"High Score: {PlayerPrefs.GetFloat("HighScore")}";
        else
            PlayerPrefs.SetFloat("HighScore", 0);
        highScore.text = $"High Score: {PlayerPrefs.GetFloat("HighScore")}";
    }

    public void UpdateScore(float amount)
    {
        if (scoreText == null) return;
        var total = _currentScore += amount;
        scoreText.text = $"Score: {total}";
        var high = PlayerPrefs.GetFloat("HighScore");
        if (high < _currentScore)
        {
            PlayerPrefs.SetFloat("HighScore", _currentScore);
            highScore.text = $"High Score: {PlayerPrefs.GetFloat("HighScore")}";
        }
    }
}

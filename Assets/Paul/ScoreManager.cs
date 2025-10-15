using UnityEngine;
using TMPro; // pour l�affichage du texte

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton d�acc�s global

    [Header("R�glages du score")]
    public int currentScore = 0;

    [Header("R�f�rence UI")]
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        // Singleton simple pour �viter les doublons
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
    }

    /// <summary>
    /// Ajoute un certain nombre de points et met � jour l�UI
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI();
    }

    /// <summary>
    /// R�initialise le score
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score : " + currentScore;
    }
}

using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;  // Singleton instance

    public TMP_Text scoreText;       // Toplam skoru g�steren text
    public TMP_Text pointsText;      // Anl�k al�nan puan� g�steren text
    public TMP_Text highScoreText;   // En y�ksek skoru g�steren text (GameOver ekran�nda)
    public TMP_Text finalScoreText;  // Game Over ekran�nda toplam skoru g�stermek i�in
    public AudioClip scoreSound;     // Oynat�lacak ses dosyas�

    private int totalScore = 0;
    private int highScore = 0;       // En y�ksek skor

    private void Awake()
    {
        // Singleton instance atamas�
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // PlayerPrefs'ten en y�ksek skoru y�kle
        highScore = PlayerPrefs.GetInt("HighScore", 0);  // Varsay�lan de�er 0
    }

    // Skoru artt�r ve UI'yi g�ncelle
    public void AddScore(int scoreToAdd)
    {
        // Skoru hemen g�ncelle
        totalScore += scoreToAdd;
        UpdateScoreText();  // Toplam skoru UI'de hemen g�ncelle
        ShowPoints(scoreToAdd);  // Anl�k puan� hemen g�ster

        // Skor artt���nda ses �al
        SoundManager.Instance.PlaySound(scoreSound);

        // E�er toplam skor en y�ksek skoru ge�tiyse kaydet
        if (totalScore > highScore)
        {
            highScore = totalScore;
            PlayerPrefs.SetInt("HighScore", highScore);  // Yeni y�ksek skoru kaydet
        }
    }

    // Toplam skoru UI'de g�ncelle
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + totalScore.ToString();
    }

    // Anl�k puan� g�ster ve coroutine ile kaybolmas�n� sa�la
    private void ShowPoints(int points)
    {
        pointsText.text = "+" + points.ToString();
        pointsText.gameObject.SetActive(true);

        // Coroutine ile daha kesin bir zamanlama kullanarak 1 saniye sonra gizle
        StartCoroutine(HidePointsTextAfterDelay(1.0f));
    }

    // 1 saniye sonra anl�k puan yaz�s�n� gizle
    private System.Collections.IEnumerator HidePointsTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pointsText.gameObject.SetActive(false);
    }

    // GameOver ekran�nda en y�ksek skoru ve toplam skoru g�ster
    public void ShowGameOverScores()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        finalScoreText.text = "Your Score: " + totalScore.ToString();  // Oyunda ald��� puan
    }
}

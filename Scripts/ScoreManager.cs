using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;  // Singleton instance

    public TMP_Text scoreText;       // Toplam skoru gösteren text
    public TMP_Text pointsText;      // Anlýk alýnan puaný gösteren text
    public TMP_Text highScoreText;   // En yüksek skoru gösteren text (GameOver ekranýnda)
    public TMP_Text finalScoreText;  // Game Over ekranýnda toplam skoru göstermek için
    public AudioClip scoreSound;     // Oynatýlacak ses dosyasý

    private int totalScore = 0;
    private int highScore = 0;       // En yüksek skor

    private void Awake()
    {
        // Singleton instance atamasý
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // PlayerPrefs'ten en yüksek skoru yükle
        highScore = PlayerPrefs.GetInt("HighScore", 0);  // Varsayýlan deðer 0
    }

    // Skoru arttýr ve UI'yi güncelle
    public void AddScore(int scoreToAdd)
    {
        // Skoru hemen güncelle
        totalScore += scoreToAdd;
        UpdateScoreText();  // Toplam skoru UI'de hemen güncelle
        ShowPoints(scoreToAdd);  // Anlýk puaný hemen göster

        // Skor arttýðýnda ses çal
        SoundManager.Instance.PlaySound(scoreSound);

        // Eðer toplam skor en yüksek skoru geçtiyse kaydet
        if (totalScore > highScore)
        {
            highScore = totalScore;
            PlayerPrefs.SetInt("HighScore", highScore);  // Yeni yüksek skoru kaydet
        }
    }

    // Toplam skoru UI'de güncelle
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + totalScore.ToString();
    }

    // Anlýk puaný göster ve coroutine ile kaybolmasýný saðla
    private void ShowPoints(int points)
    {
        pointsText.text = "+" + points.ToString();
        pointsText.gameObject.SetActive(true);

        // Coroutine ile daha kesin bir zamanlama kullanarak 1 saniye sonra gizle
        StartCoroutine(HidePointsTextAfterDelay(1.0f));
    }

    // 1 saniye sonra anlýk puan yazýsýný gizle
    private System.Collections.IEnumerator HidePointsTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pointsText.gameObject.SetActive(false);
    }

    // GameOver ekranýnda en yüksek skoru ve toplam skoru göster
    public void ShowGameOverScores()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        finalScoreText.text = "Your Score: " + totalScore.ToString();  // Oyunda aldýðý puan
    }
}

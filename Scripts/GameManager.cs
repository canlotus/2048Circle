using UnityEngine;
using UnityEngine.SceneManagement;  // Sahne y�netimi i�in gerekli
using UnityEngine.UI;               // UI elemanlar� i�in gerekli

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject pausePanel;    // Pause panel referans�
    public GameObject gameOverPanel; // Game over panel referans�
    public Button continueButton;    // Oyuna devam butonu
    public Button mainMenuButton;    // Ana men� butonu
    public Button restartButton;     // Oyunu yeniden ba�lat butonu
    public Button gameOverMainMenuButton;  // GameOver panelindeki ana men� butonu

    private bool isPaused = false;   // Oyunun durdurulup durdurulmad���n� takip eder

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Paneller ba�lang��ta kapal�
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Buton t�klama eventlerini atama
        continueButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        restartButton.onClick.AddListener(RestartGame);
        gameOverMainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void Update()
    {
        // Escape tu�una bas�l�rsa oyunu durdur veya devam ettir
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    // Oyunu durdur ve paneli g�ster
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;  // Oyunu durdur
        pausePanel.SetActive(true);  // Pause panelini g�ster
    }

    // Oyunu devam ettir ve paneli gizle
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;  // Oyunu devam ettir
        pausePanel.SetActive(false);  // Paneli gizle
    }

    // Game Over oldu�unda GameOver panelini g�ster
    public void GameOver()
    {
        Time.timeScale = 0;  // Oyun durduruluyor
        gameOverPanel.SetActive(true);  // GameOver panelini a�

        // En y�ksek skoru ve o oyunda al�nan puan� g�ster
        ScoreManager.Instance.ShowGameOverScores();

    }

    // Ana men�ye d�n
    public void GoToMainMenu()
    {
        Time.timeScale = 1;  // Oyun durdurulduysa devam ettir
        SceneManager.LoadScene("MainScene");  // Ana men� sahnesini y�kle
    }

    // Oyunu yeniden ba�lat
    public void RestartGame()
    {
        Time.timeScale = 1;  // Oyun tekrar devam etmeli
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Mevcut sahneyi yeniden y�kle
    }
}

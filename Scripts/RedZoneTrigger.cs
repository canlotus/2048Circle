using UnityEngine;
using System.Collections;

public class RedZoneTrigger : MonoBehaviour
{
    private bool gameOver = false;  // Oyunun bittiðini kontrol eder

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eðer Circle tag'ine sahip bir obje RedZone'a girerse
        if (collision.CompareTag("Circle") && !gameOver)
        {
            Debug.Log($"Circle entered RedZone: {collision.gameObject.name}");

            // Coroutine ile 1.5 saniye boyunca tetiklenme kontrolü yapalým
            StartCoroutine(CheckForGameOver(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Circle RedZone'dan çýkarsa, oyunu iptal edelim
        if (collision.CompareTag("Circle"))
        {
            Debug.Log($"Circle exited RedZone: {collision.gameObject.name}");
            StopAllCoroutines();  // Circle RedZone'dan çýkarsa, tüm coroutine'leri durdur
        }
    }

    private IEnumerator CheckForGameOver(GameObject circle)
    {
        // 1.5 saniye boyunca Circle RedZone'da kalýyor mu diye kontrol edelim
        yield return new WaitForSeconds(1.5f);

        // Eðer Circle hala RedZone'da ise oyun biter
        if (circle != null && !gameOver)
        {
            Debug.Log("Game Over");
            gameOver = true;

            // Oyun bittiðinde GameManager'daki GameOver fonksiyonunu çaðýrýyoruz
            GameManager.Instance.GameOver();
        }
    }
}

using UnityEngine;

public class GecisliOyunLoad : MonoBehaviour
{
    [SerializeField] private int loadAdAfterSpawns = 20;  // Ka� circle spawnland���nda reklam y�klenecek
    [SerializeField] private int showAdAfterSpawns = 50;  // Ka� circle spawnland���nda reklam g�sterilecek
    private int spawnCount = 0;  // �u ana kadar spawnlanan circle say�s�

    private GecisliOyun gecisliOyun;  // GecisliOyun scriptine referans

    private void Start()
    {
        gecisliOyun = GetComponent<GecisliOyun>();  // Ayn� GameObject'te bulunan GecisliOyun scriptine referans al

        Spawner.OnCircleSpawned += OnCircleSpawned;  // Spawner scriptinden tetiklenen event'e abone ol
    }

    private void OnDestroy()
    {
        Spawner.OnCircleSpawned -= OnCircleSpawned;  // Eventten ��k
    }

    // Her circle spawnland���nda tetiklenecek fonksiyon
    private void OnCircleSpawned()
    {
        spawnCount++;  // Spawn say�s�n� artt�r

        if (spawnCount == loadAdAfterSpawns)  // E�er yeterli say�da circle spawnland�ysa
        {
            gecisliOyun.LoadInterstitialAd();  // Reklam� y�kle
        }

        if (spawnCount == showAdAfterSpawns)  // E�er reklam g�sterme zaman� geldiyse
        {
            gecisliOyun.ShowInterstitialAd();  // Reklam� g�ster
            spawnCount = 0;  // Spawn say�s�n� s�f�rla
        }
    }
}

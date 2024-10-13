using UnityEngine;

public class GecisliOyunLoad : MonoBehaviour
{
    [SerializeField] private int loadAdAfterSpawns = 20;  // Kaç circle spawnlandýðýnda reklam yüklenecek
    [SerializeField] private int showAdAfterSpawns = 50;  // Kaç circle spawnlandýðýnda reklam gösterilecek
    private int spawnCount = 0;  // Þu ana kadar spawnlanan circle sayýsý

    private GecisliOyun gecisliOyun;  // GecisliOyun scriptine referans

    private void Start()
    {
        gecisliOyun = GetComponent<GecisliOyun>();  // Ayný GameObject'te bulunan GecisliOyun scriptine referans al

        Spawner.OnCircleSpawned += OnCircleSpawned;  // Spawner scriptinden tetiklenen event'e abone ol
    }

    private void OnDestroy()
    {
        Spawner.OnCircleSpawned -= OnCircleSpawned;  // Eventten çýk
    }

    // Her circle spawnlandýðýnda tetiklenecek fonksiyon
    private void OnCircleSpawned()
    {
        spawnCount++;  // Spawn sayýsýný arttýr

        if (spawnCount == loadAdAfterSpawns)  // Eðer yeterli sayýda circle spawnlandýysa
        {
            gecisliOyun.LoadInterstitialAd();  // Reklamý yükle
        }

        if (spawnCount == showAdAfterSpawns)  // Eðer reklam gösterme zamaný geldiyse
        {
            gecisliOyun.ShowInterstitialAd();  // Reklamý göster
            spawnCount = 0;  // Spawn sayýsýný sýfýrla
        }
    }
}

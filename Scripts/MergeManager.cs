using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public float pushRadius = 0.2f;  // Yak�ndaki circle'lar� tespit etmek i�in yar��ap
    public float pushForce = 2.0f;   // Uygulanacak itme kuvveti

    // �ki circle'� birle�tir
    public void MergeCircles(Circle circle1, Circle circle2)
    {
        // Ayn� numaraya sahiplerse ve daha �nce birle�tirilmediyse birle�tir
        if (circle1.CircleNumber == circle2.CircleNumber && !circle1.IsMerging && !circle2.IsMerging)
        {
            int newNumber = circle1.CircleNumber * 2;  // Bir �st seviye

            // Circle'lar�n birle�tirildi�ini i�aretle
            circle1.IsMerging = true;
            circle2.IsMerging = true;

            // Yeni circle'� olu�tur
            Circle newCircle = Spawner.Instance.Spawn(newNumber, GetMergePosition(circle1, circle2));

            // Yeni circle'�n scale ve rengini ayarla
            newCircle.SetScale(newNumber);
            newCircle.SetColor(Spawner.Instance.GetColor(newNumber));

            // Yeni circle'�n fiziksel hareketini etkinle�tir
            newCircle.CircleRigidbody.gravityScale = 1;
            newCircle.CircleRigidbody.velocity = Vector2.zero; // Olas� hatal� h�zlar� s�f�rl�yoruz

            // �evredeki circle'lara hafif itme kuvveti uygulayal�m
            ApplyPushToNearbyCircles(newCircle.transform.position, newCircle);

            // Skoru g�ncelle: birle�en circle'lar�n yeni de�eri kadar puan ekle
            ScoreManager.Instance.AddScore(newNumber);

            // Eski circle'lar� yok et
            Destroy(circle1.gameObject);
            Destroy(circle2.gameObject);
        }
    }

    // �ki circle birle�ti�inde yeni circle'�n pozisyonunu ayarlamak i�in
    private Vector3 GetMergePosition(Circle circle1, Circle circle2)
    {
        return (circle1.transform.position + circle2.transform.position) / 2;
    }

    // �evredeki circle'lara itme kuvveti uygula
    private void ApplyPushToNearbyCircles(Vector3 mergePosition, Circle mergedCircle)
    {
        // �evredeki circle'lar� bul
        Collider2D[] nearbyCircles = Physics2D.OverlapCircleAll(mergePosition, pushRadius);

        foreach (Collider2D collider in nearbyCircles)
        {
            Circle nearbyCircle = collider.GetComponent<Circle>();

            // E�er nearbyCircle mevcutsa ve bu nearbyCircle bir mainCircle de�ilse kuvvet uygula
            if (nearbyCircle != null && nearbyCircle.CircleRigidbody != null && !nearbyCircle.IsMainCircle)
            {
                // Birle�me pozisyonuna g�re itme kuvvetini belirle
                Vector2 pushDirection = nearbyCircle.transform.position - mergePosition;
                nearbyCircle.CircleRigidbody.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}

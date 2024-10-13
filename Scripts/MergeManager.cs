using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public float pushRadius = 0.2f;  // Yakýndaki circle'larý tespit etmek için yarýçap
    public float pushForce = 2.0f;   // Uygulanacak itme kuvveti

    // Ýki circle'ý birleþtir
    public void MergeCircles(Circle circle1, Circle circle2)
    {
        // Ayný numaraya sahiplerse ve daha önce birleþtirilmediyse birleþtir
        if (circle1.CircleNumber == circle2.CircleNumber && !circle1.IsMerging && !circle2.IsMerging)
        {
            int newNumber = circle1.CircleNumber * 2;  // Bir üst seviye

            // Circle'larýn birleþtirildiðini iþaretle
            circle1.IsMerging = true;
            circle2.IsMerging = true;

            // Yeni circle'ý oluþtur
            Circle newCircle = Spawner.Instance.Spawn(newNumber, GetMergePosition(circle1, circle2));

            // Yeni circle'ýn scale ve rengini ayarla
            newCircle.SetScale(newNumber);
            newCircle.SetColor(Spawner.Instance.GetColor(newNumber));

            // Yeni circle'ýn fiziksel hareketini etkinleþtir
            newCircle.CircleRigidbody.gravityScale = 1;
            newCircle.CircleRigidbody.velocity = Vector2.zero; // Olasý hatalý hýzlarý sýfýrlýyoruz

            // Çevredeki circle'lara hafif itme kuvveti uygulayalým
            ApplyPushToNearbyCircles(newCircle.transform.position, newCircle);

            // Skoru güncelle: birleþen circle'larýn yeni deðeri kadar puan ekle
            ScoreManager.Instance.AddScore(newNumber);

            // Eski circle'larý yok et
            Destroy(circle1.gameObject);
            Destroy(circle2.gameObject);
        }
    }

    // Ýki circle birleþtiðinde yeni circle'ýn pozisyonunu ayarlamak için
    private Vector3 GetMergePosition(Circle circle1, Circle circle2)
    {
        return (circle1.transform.position + circle2.transform.position) / 2;
    }

    // Çevredeki circle'lara itme kuvveti uygula
    private void ApplyPushToNearbyCircles(Vector3 mergePosition, Circle mergedCircle)
    {
        // Çevredeki circle'larý bul
        Collider2D[] nearbyCircles = Physics2D.OverlapCircleAll(mergePosition, pushRadius);

        foreach (Collider2D collider in nearbyCircles)
        {
            Circle nearbyCircle = collider.GetComponent<Circle>();

            // Eðer nearbyCircle mevcutsa ve bu nearbyCircle bir mainCircle deðilse kuvvet uygula
            if (nearbyCircle != null && nearbyCircle.CircleRigidbody != null && !nearbyCircle.IsMainCircle)
            {
                // Birleþme pozisyonuna göre itme kuvvetini belirle
                Vector2 pushDirection = nearbyCircle.transform.position - mergePosition;
                nearbyCircle.CircleRigidbody.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}

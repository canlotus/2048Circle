using UnityEngine;

public class CircleCollision : MonoBehaviour
{
    public GameObject explosionEffectPrefab;  // Patlama efekt prefab'ý için referans

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Circle otherCircle = collision.gameObject.GetComponent<Circle>();

        if (otherCircle != null)
        {
            Circle thisCircle = GetComponent<Circle>();

            // Eðer iki circle'ýn numaralarý aynýysa, onlarý birleþtir
            if (thisCircle.CircleNumber == otherCircle.CircleNumber)
            {
                MergeManager mergeManager = FindObjectOfType<MergeManager>();
                mergeManager.MergeCircles(thisCircle, otherCircle);

                // Birleþme sýrasýnda patlama efekti oluþtur
                Vector3 spawnPosition = (thisCircle.transform.position + otherCircle.transform.position) / 2;
                GameObject explosionEffect = Instantiate(explosionEffectPrefab, spawnPosition, Quaternion.identity);

                // 2 saniye sonra patlama efektini yok et
                Destroy(explosionEffect, 2.0f);

                // Circle üzerindeki AudioSource'u kullanarak sesi çal
                AudioSource thisAudioSource = thisCircle.GetComponent<AudioSource>();
                if (thisAudioSource != null && thisAudioSource.clip != null)
                {
                    thisAudioSource.Play();  // Sesi çal
                }

                // Birleþme sýrasýnda fiziksel kuvvet ekle (isteðe baðlý)
                Rigidbody2D rb = thisCircle.CircleRigidbody;
                rb.AddForce(new Vector2(0.5f, 0.5f), ForceMode2D.Impulse);  // Circle'ý hafifçe yukarý doðru ittir
            }
        }
    }
}

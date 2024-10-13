using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static event System.Action OnCircleSpawned;

    // Singleton instance
    public static Spawner Instance { get; private set; }

    public GameObject circlePrefab;
    public Color[] circleColors;  // Renkler için dizi ekliyoruz

    private Vector3 spawnPosition;

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

        spawnPosition = transform.position;
    }

    public Circle SpawnRandom()
    {
        int randomNumber = GenerateRandomNumber();
        Circle newCircle = Spawn(randomNumber, spawnPosition);

        OnCircleSpawned?.Invoke();  // Yeni bir circle spawn edildiðinde event tetiklenir
        return newCircle;
    }

    // Yeni circle spawn etmek için bir sayý ve pozisyon alýr
    public Circle Spawn(int number, Vector3 position)
    {
        GameObject circleObject = Instantiate(circlePrefab, position, Quaternion.identity);
        Circle newCircle = circleObject.GetComponent<Circle>();

        newCircle.SetNumber(number);
        newCircle.SetColor(GetColor(number));
        newCircle.SetScale(number);

        // Bounciness modunu kontrol et ve Circle'ýn fizik materyalini ayarla
        int bouncinessMode = PlayerPrefs.GetInt("BouncinessMode", 0);  // Eðer ikinci moddaysa bounciness = 1
        float bounciness = (bouncinessMode == 1) ? 1.0f : 0.2f;
        newCircle.SetPhysicsMaterial(bounciness);  // Fizik materyalini Circle'a uygula

        newCircle.CircleRigidbody.gravityScale = 0;

        OnCircleSpawned?.Invoke();
        return newCircle;
    }

    private int GenerateRandomNumber()
    {
        return (int)Mathf.Pow(2, Random.Range(1, 6));
    }

    public Color GetColor(int number)
    {
        return circleColors[(int)(Mathf.Log(number) / Mathf.Log(2)) - 1];
    }
}

using UnityEngine;
using TMPro;

public class Circle : MonoBehaviour
{
    static int staticID = 0;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private PhysicsMaterial2D physicsMaterial; // Fizik materyali referansý

    [HideInInspector] public int CircleID;
    [HideInInspector] public Color CircleColor;
    [HideInInspector] public int CircleNumber;
    [HideInInspector] public Rigidbody2D CircleRigidbody;
    [HideInInspector] public bool IsMainCircle;
    [HideInInspector] public bool IsMerging = false;  // Birleþtirme kontrolü için

    private SpriteRenderer circleSpriteRenderer;

    private void Awake()
    {
        CircleID = staticID++;
        circleSpriteRenderer = GetComponent<SpriteRenderer>();
        CircleRigidbody = GetComponent<Rigidbody2D>();
        CircleRigidbody.gravityScale = 0;  // Baþlangýçta düþmeyecek
    }

    public void ReleaseCircle()
    {
        CircleRigidbody.gravityScale = 1;
    }

    // Circle'ýn rengini ayarla
    public void SetColor(Color color)
    {
        CircleColor = color;
        color.a = 1.0f;
        circleSpriteRenderer.color = color;
        Debug.Log($"Circle Color Set to: {circleSpriteRenderer.color}");
    }

    public void SetScale(int number)
    {
        float scale = 1.0f;

        switch (number)
        {
            case 2: scale = 0.47f; break;
            case 4: scale = 0.55f; break;
            case 8: scale = 0.64f; break;
            case 16: scale = 0.71f; break;
            case 32: scale = 0.77f; break;
            case 64: scale = 0.84f; break;
            case 128: scale = 0.90f; break;
            case 256: scale = 0.96f; break;
            case 512: scale = 1.02f; break;
            case 1024: scale = 1.1f; break;
            case 2048: scale = 1.15f; break;
            case 4096: scale = 1.20f; break;
            default: scale = 1.0f; break;
        }

        transform.localScale = new Vector3(scale, scale, 1);
    }

    public void SetNumber(int number)
    {
        CircleNumber = number;
        numberText.text = number.ToString();

        // Sayýya göre mass ayarla (1 ile 1.5 arasýnda)
        CircleRigidbody.mass = CalculateMass(number);
    }

    private float CalculateMass(int number)
    {
        int minNumber = 2;
        int maxNumber = 4096;
        float minMass = 1.0f;
        float maxMass = 1.5f;
        float normalizedValue = Mathf.InverseLerp(minNumber, maxNumber, number);
        return Mathf.Lerp(minMass, maxMass, normalizedValue);
    }

    // Fizik materyalini ayarlamak için bir fonksiyon
    public void SetPhysicsMaterial(float bounciness)
    {
        if (physicsMaterial == null)
        {
            physicsMaterial = new PhysicsMaterial2D();  // Eðer fizik materyali yoksa oluþtur
        }

        physicsMaterial.bounciness = bounciness;
        CircleRigidbody.sharedMaterial = physicsMaterial;  // Fizik materyalini Rigidbody'ye uygula
    }
}

using UnityEngine;
using UnityEngine.EventSystems;  // UI t�klamalar�n� kontrol etmek i�in gerekli

public class Player : MonoBehaviour
{
    private Circle mainCircle;

    private void Start()
    {
        SpawnCircle();
    }

    private void Update()
    {
        if (mainCircle != null && mainCircle.IsMainCircle)
        {
            // E�er dokunma/t�klama bir UI eleman�nda de�ilse input'u i�leyelim
            if (!IsPointerOverUIElement())
            {
                HandleInput();
            }
        }
    }

    private void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // Mouse ile kontrol
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            float newX = Mathf.Clamp(mousePosition.x, -1.56f, 1.56f);
            mainCircle.transform.position = new Vector3(newX, mainCircle.transform.position.y, 0f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseCircle();
        }
#endif

        // Dokunma ile kontrol
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0f;

            if (touch.phase == TouchPhase.Moved)
            {
                float newX = Mathf.Clamp(touchPosition.x, -1.56f, 1.56f);
                mainCircle.transform.position = new Vector3(newX, mainCircle.transform.position.y, 0f);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                ReleaseCircle();
            }
        }
    }

    private void ReleaseCircle()
    {
        mainCircle.IsMainCircle = false;
        mainCircle.CircleRigidbody.gravityScale = 1;  // Circle serbest b�rak�l�p d��ecek
        mainCircle = null;

        // 0.5 saniye gecikmeyle yeni circle spawn et
        Invoke("SpawnNewCircle", 0.5f);
    }

    private void SpawnNewCircle()
    {
        SpawnCircle();
    }

    private void SpawnCircle()
    {
        mainCircle = Spawner.Instance.SpawnRandom();
        mainCircle.IsMainCircle = true;
    }

    // Pointer'�n UI eleman�nda olup olmad���n� kontrol et
    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;  // Singleton instance

    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton instance atamas�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ses y�netimi her sahnede kals�n
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Belirli bir ses dosyas�n� �al
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

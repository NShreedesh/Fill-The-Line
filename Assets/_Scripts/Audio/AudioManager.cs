using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public static bool IsSoundOn { get; private set; } = true;

    [Header("AudioSource Info")]
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioSource backgroundAudioSource;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        IsSoundOn = true;
    }

    public void PlayEffectAudio(AudioClip clip)
    {
        if (effectAudioSource != null)
        {
            effectAudioSource.Stop();
        }

        effectAudioSource.PlayOneShot(clip);
    }

    public void PlayBackgroundAudio(AudioClip clip)
    {
        if (backgroundAudioSource.clip != null) return;

        backgroundAudioSource.clip = clip;
        backgroundAudioSource.Play();
    }

    public void SetSound()
    {
        IsSoundOn = !IsSoundOn;

        if (IsSoundOn)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
    }
}

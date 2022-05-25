using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    private static AudioManager Instance { get { return _instance; } }

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

    public void PlayEffectAudio(AudioClip clip)
    {
        effectAudioSource.PlayOneShot(clip);
    }

    public void PlayBackgroundAudio(AudioClip clip)
    {
        backgroundAudioSource.clip = clip;
        backgroundAudioSource.Play();
    }
}

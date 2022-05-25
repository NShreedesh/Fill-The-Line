using UnityEngine;

public class PlayBgMusic : MonoBehaviour
{
    [Header("Audio Info")]
    [SerializeField] private AudioClip backgroundAudioClip;

    private void Start()
    {
        AudioManager.Instance.PlayBackgroundAudio(backgroundAudioClip);
    }
}

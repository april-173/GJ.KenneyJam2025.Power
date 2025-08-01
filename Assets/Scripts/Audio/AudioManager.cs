using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]private AudioSource BgmSource;
    [SerializeField] private AudioSource SfxAudio;

    public AudioClip bgm;
    public AudioClip slide;
    public AudioClip strike;

    private void Start()
    {
        
    }

    public void PlaySfx(AudioClip clip)
    {
        SfxAudio.PlayOneShot(clip);
    }
}

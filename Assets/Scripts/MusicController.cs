using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip levelSong;
    public AudioClip bossSong;
    public AudioClip levelClearSong;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        PlaySong(levelSong);
    }

    public void PlaySong(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}

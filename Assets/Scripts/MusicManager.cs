using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource musicSource;

    //nuevo
    public AudioClip introMusic;
    public AudioClip menuMusic;
    public AudioClip introAirboneDangerMusic;
    public AudioClip ultimateDeliveryMusic;
    public AudioClip mortalBagMusic;
    public AudioClip ultimateDefenseMusic;
    public AudioClip zeroZoneVRMusic;
    public AudioClip scoreScene;
    //fim n
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //nuevo
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();       

    }

    public void PlayMusic(AudioClip clip,bool loop)
    {
        if (clip != null )
        {
            musicSource.clip = clip;
            musicSource.loop = loop; 
            musicSource.Play();
        }
    }
    

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }
}
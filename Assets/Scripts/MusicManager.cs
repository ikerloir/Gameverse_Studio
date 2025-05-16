using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //musica de juegos
    public static MusicManager Instance;
    public AudioSource musicSource;
    public AudioClip introMusic;
    public AudioClip menuMusic;
    public AudioClip introAirboneDangerMusic;
    public AudioClip ultimateDeliveryMusic;
    public AudioClip mortalBagMusic;
    public AudioClip ultimateDefenseMusic;
    public AudioClip zeroZoneVRMusic;
    public AudioClip scoreScene;
    
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
        
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();       

    }
    // carga el clip selecionado en bucle
    public void PlayMusic(AudioClip clip,bool loop)
    {
        if (clip != null )
        {
            musicSource.clip = clip;
            musicSource.loop = loop; 
            musicSource.Play();
        }
    }
    
}
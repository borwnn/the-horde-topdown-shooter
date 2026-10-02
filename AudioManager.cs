using UnityEngine;
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SfxSource;

    public AudioClip background;
    public AudioClip pistol;
    public AudioClip shotgun;
    public AudioClip shotgun_pickup;

    public AudioClip smg_pickup; // added

    public AudioClip smg; // added

    public AudioClip Minigun; // original code


    public AudioClip health_pickup; // added code 

    public AudioClip enemy_death; // added

    public AudioClip take_damage; // added

    public AudioClip death_sound; // added




    void Awake()
    {

        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxClip != null && SfxSource != null)
        {
            SfxSource.PlayOneShot(sfxClip);
        }
    }
    public void PlayBackgroundMusic()
    {
        if (musicSource.isPlaying) return;

        musicSource.clip = background;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("----------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------- Audio Clip ----------")]
    public AudioClip walk;
    public AudioClip run;
    public AudioClip jump;
    public AudioClip crouch;

    [Header("----------- EnemyChase Audio ----------")]
    public AudioClip chaseMusic;

    [Header("----------- Background Music ----------")]
    public AudioClip scene0BackgroundMusic;
    public AudioClip scene1BackgroundMusic;

    private Dictionary<string, AudioClip> sceneBackgroundMusic;
    private bool isChaseMusicPlaying = false; // Track if chase music is playing

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            sceneBackgroundMusic = new Dictionary<string, AudioClip>
            {
                { "Main Menu", scene0BackgroundMusic },
                { "Level Selection", scene0BackgroundMusic },
                { "Options", scene0BackgroundMusic },
                { "Level1", scene1BackgroundMusic }
            };
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlayBackgroundMusicForCurrentScene();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBackgroundMusicForCurrentScene();
    }

    private void PlayBackgroundMusicForCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (sceneBackgroundMusic.TryGetValue(currentSceneName, out AudioClip newBackgroundClip))
        {
            PlayBackgroundMusic(newBackgroundClip);
        }
    }

    public void PlayLoopingSFX(AudioClip clip)
    {
        if (SFXSource.clip != clip)
        {
            SFXSource.clip = clip;
            SFXSource.loop = true;
            SFXSource.Play();
        }
    }

    public void StopLoopingSFX()
    {
        SFXSource.loop = false;
        SFXSource.Stop();
        SFXSource.clip = null;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayChaseMusic()
    {
        if (!isChaseMusicPlaying && musicSource.clip != chaseMusic)
        {
            musicSource.clip = chaseMusic;
            musicSource.loop = true;
            musicSource.Play();
            isChaseMusicPlaying = true;
        }
    }

    public void ResumeNormalMusic()
    {
        if (isChaseMusicPlaying)
        {
            PlayBackgroundMusicForCurrentScene();
            isChaseMusicPlaying = false;
        }
    }
}


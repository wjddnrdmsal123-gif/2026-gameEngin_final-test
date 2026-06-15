using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("오디오 소스")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM")]
    public AudioClip openingBGM;
    public AudioClip ingameBGM;
    public AudioClip bossBGM;
    public AudioClip gameOverBGM;

    [Header("볼륨")]
    public float bgmVolume = 0.5f;
    public float sfxVolume = 0.8f;

    [Header("SFX")]
    public AudioClip shootSound;
    public AudioClip zombieHitSound;
    public AudioClip zombieDieSound;
    public AudioClip playerHitSound;
    public AudioClip chestSound;
    public AudioClip warningSound;
    public AudioClip bossChargeSound;

    private AudioClip currentBGM;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupAudioSources();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        ChangeBGMByScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SetupAudioSources()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = bgmVolume;

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeBGMByScene(scene.name);
    }

    private void ChangeBGMByScene(string sceneName)
    {
        if (sceneName == "MainMenu" || sceneName == "StartScene" || sceneName == "Opening")
        {
            PlayBGM(openingBGM);
        }
        else if (sceneName == "MidBoss_Level")
        {
            PlayBGM(bossBGM);
        }
        else if (sceneName == "GameOver")
        {
            PlayBGM(gameOverBGM);
        }
        else
        {
            PlayBGM(ingameBGM);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
            return;

        // 이미 같은 BGM이면 다시 재생하지 않음
        if (currentBGM == clip && bgmSource.isPlaying)
            return;

        currentBGM = clip;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
        currentBGM = null;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayShoot()
    {
        PlaySFX(shootSound);
    }

    public void PlayZombieHit()
    {
        PlaySFX(zombieHitSound);
    }

    public void PlayZombieDie()
    {
        PlaySFX(zombieDieSound);
    }

    public void PlayPlayerHit()
    {
        PlaySFX(playerHitSound);
    }

    public void PlayChest()
    {
        PlaySFX(chestSound);
    }

    public void PlayWarning()
    {
        PlaySFX(warningSound);
    }

    public void PlayBossCharge()
    {
        PlaySFX(bossChargeSound);
    }
}
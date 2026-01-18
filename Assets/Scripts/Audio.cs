using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer Settings")]
    public AudioMixer mainMixer;

    private AudioSource _oneShotSource;
    public AudioSource _loopSource;

    private const float DefaultVol = 0.5f;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _oneShotSource = gameObject.AddComponent<AudioSource>();
        _oneShotSource.spatialBlend = 0;
        
        _loopSource = gameObject.AddComponent<AudioSource>();
        _loopSource.loop = true;
        _loopSource.spatialBlend = 0;
        _loopSource.volume = 0f;

        if (mainMixer != null)
        {
            _oneShotSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("SFX")[0];
            _loopSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("SFX")[0];
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(LoadVolumeSettings());
    }

    private void Start()
    {
        StartCoroutine(LoadVolumeSettings());
    }

    private IEnumerator LoadVolumeSettings()
    {
        // Wait for the Mixer to wake up and link to the scene
        yield return new WaitForSecondsRealtime(0.2f);

        float master = PlayerPrefs.GetFloat("MasterVolumeSave", DefaultVol);
        float music = PlayerPrefs.GetFloat("MusicVolumeSave", DefaultVol);
        float sfx = PlayerPrefs.GetFloat("SFXVolumeSave", DefaultVol);

        for (int i = 0; i < 15; i++)
        {
            SetMasterVolume(master);
            SetMusicVolume(music);
            SetSFXVolume(sfx);
            yield return null; 
        }
    }

    public void SetMusicVolume(float value)
    {
        if (mainMixer == null) return;
        float dbValue = Mathf.Log10(Mathf.Max(0.0001f, value)) * 30f;
        mainMixer.SetFloat("MusicVol", dbValue);
        PlayerPrefs.SetFloat("MusicVolumeSave", value);
    }

    public void SetSFXVolume(float value)
    {
        if (mainMixer == null) return;
        float dbValue = Mathf.Log10(Mathf.Max(0.0001f, value)) * 30f;
        mainMixer.SetFloat("SFXVol", dbValue);
        PlayerPrefs.SetFloat("SFXVolumeSave", value);
    }

    public void SetMasterVolume(float value)
    {
        if (mainMixer == null) return;
        float dbValue = Mathf.Log10(Mathf.Max(0.0001f, value)) * 30f;
        mainMixer.SetFloat("MasterVol", dbValue);
        PlayerPrefs.SetFloat("MasterVolumeSave", value);
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null) _oneShotSource.PlayOneShot(clip, volume);
    }

    public void PlayLoop(AudioClip clip, bool play, float fadeSpeed = 1f)
    {
        if (_loopSource == null) return;
        if (_loopSource.clip != clip) _loopSource.clip = clip;

        if (play)
        {
            if (!_loopSource.isPlaying) _loopSource.Play();
            _loopSource.volume = Mathf.MoveTowards(_loopSource.volume, 1f, fadeSpeed * Time.deltaTime);
        }
        else
        {
            _loopSource.volume = Mathf.MoveTowards(_loopSource.volume, 0f, fadeSpeed * Time.deltaTime);
            if (_loopSource.volume <= 0f && _loopSource.isPlaying) _loopSource.Stop();
        }
    }

    public void StopLoop()
    {
        if (_loopSource != null)
        {
            _loopSource.Stop();
            _loopSource.volume = 0f;
        }
    }
}
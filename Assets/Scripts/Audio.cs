using UnityEngine;



public class AudioManager : MonoBehaviour

{

    public static AudioManager Instance;

    private AudioSource _oneShotSource;

    public AudioSource _loopSource;

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
            return;
        }

        _oneShotSource = gameObject.AddComponent<AudioSource>();
        _oneShotSource.spatialBlend = 0;



        _loopSource = gameObject.AddComponent<AudioSource>();
        _loopSource.loop = true;
        _loopSource.spatialBlend = 0;
        _loopSource.volume = 0f;

    }



    public void PlaySound(AudioClip clip, float volume = 1f)

    {

        if (clip != null)

        {

            _oneShotSource.PlayOneShot(clip, volume);

        }

    }



    public void PlayLoop(AudioClip clip, bool play, float fadeSpeed = 1f)

    {

        if (_loopSource.clip != clip)

        {

            _loopSource.clip = clip;

        }



        if (play)

        {

            if (!_loopSource.isPlaying)

                _loopSource.Play();

            _loopSource.volume = Mathf.MoveTowards(_loopSource.volume, 1f, fadeSpeed * Time.deltaTime);

        }

        else

        {

            _loopSource.volume = Mathf.MoveTowards(_loopSource.volume, 0f, fadeSpeed * Time.deltaTime);

            if (_loopSource.volume <= 0f && _loopSource.isPlaying)

            {

                _loopSource.Stop();

            }

        }

    }



    public void StopLoop()

    {

        if (_loopSource.isPlaying)

        {

            _loopSource.Stop();

            _loopSource.volume = 0f;

        }

    }


    private void Update()
    {
    }
}
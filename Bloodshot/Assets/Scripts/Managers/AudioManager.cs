using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private float _sfxMinimumDistance;
    [SerializeField] private AudioSource[] _sfx;
    [SerializeField] private AudioSource[] _bgm;

    public bool PlayBgm;

    private int _bgmIndex;

    private bool _canPlaySFX;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;

        Invoke("AllowSFX", 1);

    }

    private void Update()
    {
        if (!PlayBgm)
            StopALlBgM();
        else
        { 
            if (!_bgm[_bgmIndex].isPlaying)
                PlayBgM(_bgmIndex);
        }
    }

    public void PlaySFX(int sfxIndex,Transform source)
    {
        if (_canPlaySFX == false)
            return;

        if (source != null && Vector2.Distance(PlayerManager.Instance.Player.transform.position, source.position) > _sfxMinimumDistance)
            return;

        if (sfxIndex < _sfx.Length)
        {
            _sfx[sfxIndex].pitch = Random.Range(0.80f, 1.1f);
            _sfx[sfxIndex].Play();
        }
    }

    public void StopSFX(int index) => _sfx[index].Stop();

    public void StopSFXWithTime(int index) => StartCoroutine(DecreaseVolume(_sfx[index]));

    public void StopBGMWithTime(int index) => StartCoroutine(DecreaseVolume(_bgm[index]));

    private IEnumerator DecreaseVolume(AudioSource audio)
    { 
        float defaultVolume = audio.volume;

        while (audio.volume > 0.1f)
        {
            audio.volume -= audio.volume * 0.2f;

            yield return new WaitForSeconds(0.25f);

            if (audio.volume <= 0.1f)
            {
                audio.Stop();
                audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayRandomBgM()
    {
        _bgmIndex = Random.Range(0, _bgm.Length);
        PlayBgM(_bgmIndex);
    }

    public void StopALlBgM()
    { 
        for (int i = 0; i < _bgm.Length; i++)
        {
            _bgm[i].Stop();
        }
    }

    public void PlayBgM(int bgmIndex)
    {
        _bgmIndex = bgmIndex;

        StopALlBgM();

        _bgm[_bgmIndex].Play();
    }

    private void AllowSFX() => _canPlaySFX = true;
}

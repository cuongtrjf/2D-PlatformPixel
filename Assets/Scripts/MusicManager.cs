using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    [SerializeField] private Slider musicSlider;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        if (backgroundMusic != null)
        {
            PlayerBackgroundMusic(false, backgroundMusic);
        }
        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });

    }

    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;
    }


    public static void PlayerBackgroundMusic(bool resetSong, AudioClip audioClip = null)
        //neu reset game thi reset lai music,tham so audioclip=null dc hieu no co the co hoac khong truyen vao
    {
        if (audioClip != null)//neu dc truyen vao bai hat moi thi audiosource= cai do
        {
            instance.audioSource.clip = audioClip;
        }
        if (instance.audioSource.clip != null)
        {
            if (resetSong)
            {
                instance.audioSource.Stop();//stop thi sau khi chay no se chay lai tu dau, dat vi tri phat =0
            }
            instance.audioSource.Play();
        }
    }

    public static void PauseBackgroundMusic()
    {
        instance.audioSource.Pause();
    }
}

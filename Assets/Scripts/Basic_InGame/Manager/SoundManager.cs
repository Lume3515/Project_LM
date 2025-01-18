using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Shooting,
    Reload

}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;
        
    [SerializeField] AudioSource[] audioSources;

    // 0 : 장전, 1 : 발사
    [SerializeField] AudioClip[] shootingAndReload;   

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void Sound(SoundType type)
    {

        switch (type)
        {
            case SoundType.Reload:
                audioSources[0].clip = shootingAndReload[0];
                audioSources[0].Play();
                break;

            case SoundType.Shooting:
                audioSources[0].clip = shootingAndReload[1];
                audioSources[0].Play();
                break;

        }



    }


}

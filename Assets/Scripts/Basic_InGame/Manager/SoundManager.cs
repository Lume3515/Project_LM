using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Shooting,

}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    [SerializeField] AudioSource[] audioSources;

    private int index;

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
            case SoundType.Shooting:
                audioSources[0].Play();
                break;
        }



    }


}

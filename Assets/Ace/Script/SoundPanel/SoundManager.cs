using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // private AudioSource[] _audioSources = new AudioSource[(int)Defines.ESoundType.MaxCount];
    // private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SoundManager();
            }
            return _instance;
        }
    }

    private float masterVolumeValue;
}

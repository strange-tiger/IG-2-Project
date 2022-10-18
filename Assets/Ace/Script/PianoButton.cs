using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Color = Defines.EColor;

public class PianoButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _colorSoundClipList;

    private AudioSource _audioSource;
    private Color _color;
    private static Dictionary<_Color, Color> _pianoColorDic = new Dictionary<_Color, Color>();
    private static Dictionary<_Color, AudioClip> _colorSoundDic = new Dictionary<_Color, AudioClip>();

    private bool _isEnterd = false;
    void Start()
    {
        _color = GetComponent<Material>().color;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 1;

        for (int i = 0; i < (int)_Color.MaxCount; i++)
        {
            _colorSoundDic.Add((_Color)i, _colorSoundClipList[i]);
        }
        

    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isEnterd = true;
        _audioSource.PlayOneShot(_colorSoundDic[_Color.Red], _audioSource.volume 
            * PlayerPrefs.GetFloat("EffectVolume"));
    }
    private void OnCollisionStay(Collision collision)
    {
        _isEnterd = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        _isEnterd = false;   
    }
}

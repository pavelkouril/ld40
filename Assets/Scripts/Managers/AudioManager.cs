using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _source;

    [SerializeField]
    public AudioClip _mouseClickClip;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _source.PlayOneShot(_mouseClickClip);
        }
    }
}

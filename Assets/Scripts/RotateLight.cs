using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLight : MonoBehaviour
{
    public float _Speed;

    void Update()
    {
        transform.Rotate(0.0f, Time.deltaTime * _Speed, 0.0f);
    }
}

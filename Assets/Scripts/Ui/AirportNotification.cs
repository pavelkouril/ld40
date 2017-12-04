using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirportNotification : MonoBehaviour
{
    private RectTransform _rt;

    private void Awake()
    {
        _rt = GetComponent<RectTransform>();
    }

    private void Update()
    {
        LeanTween.color(_rt, Color.red, 1).setLoopPingPong();
    }
}

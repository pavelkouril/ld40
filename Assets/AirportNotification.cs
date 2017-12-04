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
        // LeanTween.scale(_rt, new Vector3(2, 2, 2), 1).setLoopClamp();
    }
}

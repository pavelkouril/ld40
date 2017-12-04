using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMoon : MonoBehaviour
{
	void Update ()
    {
        transform.Rotate(0.0f, DaytimeManager.DeltaTimeMs / 2400.0f, 0.0f);
	}
}

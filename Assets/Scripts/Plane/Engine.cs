using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public float RPM;

    public void Update()
    {
        transform.Rotate(0.0f, 0.0f, RPM * Time.deltaTime, Space.Self);
    }
}

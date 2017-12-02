using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGlobe : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(0, 15, 0) * Time.deltaTime);
    }
}

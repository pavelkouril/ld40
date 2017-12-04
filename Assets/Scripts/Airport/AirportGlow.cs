using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirportGlow : MonoBehaviour
{
    private Airport _airport;
    private Vector3 _center;
    private Vector3 _direction;
    private float _passenger;
    private Material _material;

	void Start ()
    {
        _center = new Vector3(0.0f, 0.0f, 0.0f);
        _direction = transform.position - _center;

        _airport = transform.parent.GetComponent<AirportVisualisation>().Airport;

        transform.rotation = Quaternion.FromToRotation(new Vector3(1.0f, 0.0f, 0.0f), _direction);
        transform.localPosition = _center - _direction * 0.35f;

        _material = GetComponent<Renderer>().material;

        _passenger = 0.0f;
    }
	
	void Update ()
    {
		if (_airport.IsUnlocked)
        {
            float eps = 0.1f * Time.deltaTime;

            if (_passenger < _airport.PassengerCount - eps)
            {
                _passenger += 0.1f * Time.deltaTime;
            }
            else if (_passenger > _airport.PassengerCount + eps)
            {
                _passenger -= 0.1f * Time.deltaTime;
            }

            transform.localPosition = _center - _direction * (0.15f * (1.0f - Mathf.Clamp01(_passenger)) + 0.05f);

            _material.SetVector("_Color", Vector4.Lerp(new Vector4(0.0f, 1.0f, 0.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f), Mathf.Clamp01(_passenger)));
        }
	}
}

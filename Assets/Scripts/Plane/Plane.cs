using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public string Name { get; set; }

    public bool HasTarget
    {
        get
        {
            return _currentTarget != null;
        }
    }

    private Queue<Airport> _airports = new Queue<Airport>();

    private Airport _currentTarget;

    private void Update()
    {

    }

    private void AriveAtTarget()
    {
        _airports.Enqueue(_currentTarget);
        StartCoroutine(WaitForPassengers(1));
    }

    private IEnumerator WaitForPassengers(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _currentTarget = _airports.Dequeue();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    private void Awake()
    {

    }

    private void Update()
    {

    }

    public void SpawnAtAirport(Airport airport)
    {
        airport.PassengerCount += Random.Range(0.07f, 0.25f);
    }
}

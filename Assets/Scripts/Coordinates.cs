using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GeoPoint : IEquatable<GeoPoint>
{
    public float Latitude
    {
        get
        {
            return _latitude;
        }
    }
    public float Longitude
    {
        get
        {
            return _longtitude;
        }
    }

    public const float EarthMeanRadius = 6372795.477598f;

    [SerializeField]
    private float _latitude;
    [SerializeField]
    private float _longtitude;

    private float _radLatitude;
    private float _radLongtitude;


    public GeoPoint(float latitude, float longitude) : this()
    {
        _latitude = latitude;
        _longtitude = longitude;
        _radLatitude = Mathf.Deg2Rad * latitude;
        _radLongtitude = Mathf.Deg2Rad * longitude;
    }

    /// <summary>
    ///  Calculates distance between two geographic location, uses spherical geometry
    /// </summary>
    /// <returns></returns>
    public static float Distance(GeoPoint a, GeoPoint b)
    {
        return 2f * EarthMeanRadius * Mathf.Asin(Mathf.Sqrt(Mathf.Pow(Mathf.Sin((b.Latitude - a.Latitude) / 2f), 2f) +
            Mathf.Cos(a.Latitude) * Mathf.Cos(b.Latitude) * Mathf.Pow(Mathf.Sin((b.Longitude - a.Longitude) / 2f), 2f)));
    }

    /// <summary>
    /// Calculates angle from a to b related to equator
    /// </summary>
    /// <returns></returns>
    public static float Angle(GeoPoint a, GeoPoint b)
    {
        var z = Mathf.Log(Mathf.Tan(b.Latitude / 2.0f + Mathf.PI / 4.0f) / Mathf.Tan(a.Latitude / 2.0f + Mathf.PI / 4.0f));
        var x = Mathf.Abs(a.Longitude - b.Longitude);
        return Mathf.Atan2(z, x);
    }

    /// <summary>
    /// Calculates angle from a to b related to north
    /// </summary>
    /// <returns></returns>
    public static float Bearing(GeoPoint a, GeoPoint b)
    {
        var y = Mathf.Sin(b.Longitude - a.Longitude) * Mathf.Cos(b.Latitude);
        var x = Mathf.Cos(a.Latitude) * Mathf.Sin(b.Latitude) - Mathf.Sin(a.Latitude) * Mathf.Cos(b.Latitude) * Mathf.Cos(b.Longitude - a.Longitude);
        return Mathf.Atan2(y, x);
    }

    public bool Equals(GeoPoint other)
    {
        return Mathf.Approximately(other.Latitude, Latitude) && Mathf.Approximately(other.Longitude, Longitude);
    }
}

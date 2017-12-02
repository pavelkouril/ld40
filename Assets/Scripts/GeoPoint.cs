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

    public float LatitudeRad
    {
        get
        {
            return Mathf.Deg2Rad * _latitude;
        }
    }

    public float Longitude
    {
        get
        {
            return _longtitude;
        }
    }

    public float LongitudeRad
    {
        get
        {
            return Mathf.Deg2Rad * _longtitude;
        }
    }

    public const float EarthMeanRadius = 6372795.477598f;

    [SerializeField]
    private float _latitude;
    [SerializeField]
    private float _longtitude;

    public GeoPoint(float latitude, float longitude) : this()
    {
        _latitude = latitude;
        _longtitude = longitude;
    }

    /// <summary>
    ///  Calculates distance between two geographic location, uses spherical geometry
    /// </summary>
    /// <returns></returns>
    public static float Distance(GeoPoint a, GeoPoint b)
    {
        float dPhi = b.LatitudeRad - a.LatitudeRad;
        float dLambda = b.LongitudeRad - a.LongitudeRad;

        float alpha = Mathf.Sin(dPhi * 0.5f) * Mathf.Sin(dPhi * 0.5f) +
            Mathf.Cos(a.LatitudeRad) * Mathf.Cos(b.LatitudeRad) * Mathf.Sin(dLambda * 0.5f) * Mathf.Sin(dLambda * 0.5f);
        float gamma = 2.0f * Mathf.Atan2(Mathf.Sqrt(alpha), Mathf.Sqrt(1.0f - alpha));

        return gamma * EarthMeanRadius;
    }

    /// <summary>
    /// Calculates angle from a to b related to equator
    /// </summary>
    /// <returns></returns>
    public static float Angle(GeoPoint a, GeoPoint b)
    {
        var z = Mathf.Log(Mathf.Tan(b.LatitudeRad / 2.0f + Mathf.PI / 4.0f) / Mathf.Tan(a.LatitudeRad / 2.0f + Mathf.PI / 4.0f));
        var x = Mathf.Abs(a.LongitudeRad - b.LongitudeRad);
        return Mathf.Atan2(z, x);
    }

    /// <summary>
    /// Calculates angle from a to b related to north
    /// </summary>
    /// <returns></returns>
    public static float Bearing(GeoPoint a, GeoPoint b)
    {
        var y = Mathf.Sin(b.LongitudeRad - a.LongitudeRad) * Mathf.Cos(b.LatitudeRad);
        var x = Mathf.Cos(a.LatitudeRad) * Mathf.Sin(b.LatitudeRad) - Mathf.Sin(a.LatitudeRad) * Mathf.Cos(b.LatitudeRad) * Mathf.Cos(b.LongitudeRad - a.LongitudeRad);
        return Mathf.Atan2(y, x);
    }

    public static Vector3 ToSphericalCartesian(GeoPoint g)
    {
        return new Vector3(Mathf.Cos(g.LatitudeRad) * Mathf.Cos(g.LongitudeRad),
            Mathf.Cos(g.LatitudeRad) * Mathf.Sin(g.LongitudeRad),
            Mathf.Sin(g.LatitudeRad)
            );
    }

    public Vector3 ToSphericalCartesian()
    {
        return new Vector3(Mathf.Cos(LatitudeRad) * Mathf.Cos(LongitudeRad),
            Mathf.Sin(LatitudeRad),
            Mathf.Cos(LatitudeRad) * Mathf.Sin(LongitudeRad)
            );
    }

    public Vector2 ToPlanarCartesian()
    {
        return new Vector2((Longitude + 180f) / 360f, (Latitude + 90f) / 180f);
    }

    public bool Equals(GeoPoint other)
    {
        return Mathf.Approximately(other.Latitude, Latitude) && Mathf.Approximately(other.Longitude, Longitude);
    }

    override public string ToString()
    {
        return string.Format("Lat: {0}, Lon: {1}", _latitude, _longtitude);
    }
}

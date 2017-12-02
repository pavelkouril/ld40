using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutePath : MonoBehaviour
{
    public GeoPoint mFrom;
    public GeoPoint mTo;
    public float mDistance;

    /*public RoutePath(GeoPoint from, GeoPoint to)
    {
        mFrom = from;
        mTo = to;
        mDistance = GeoPoint.Distance(mFrom, mTo);
    }*/

    public void Update()
    {
    }

    public void OnDrawGizmos()
    {
        mDistance = GeoPoint.Distance(mFrom, mTo);
        
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f);
        Gizmos.DrawLine(GeoPoint.ToCartesian(mFrom), GeoPoint.ToCartesian(mTo));
    }
}

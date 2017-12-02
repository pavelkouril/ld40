using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutePath : MonoBehaviour
{
    public GeoPoint mFrom;
    public GeoPoint mTo;
    public float mDistance;
    private float scale = 0.05f;
    private float pathProgress = 0.0f;
    public float mSpeed;

    /*public RoutePath(GeoPoint from, GeoPoint to)
    {
        mFrom = from;
        mTo = to;
        mDistance = GeoPoint.Distance(mFrom, mTo);
    }*/

    public void Update()
    {
        pathProgress += Time.deltaTime * mSpeed;
        if (pathProgress > mDistance)
        {
            pathProgress = mDistance;
        }
    }

    private void DrawLine(Vector3 a, Vector3 b)
    {
        Gizmos.DrawLine(transform.parent.rotation * a, transform.parent.rotation * b);
    }

    public void OnDrawGizmos()
    {
        mDistance = GeoPoint.Distance(mFrom, mTo);

        Quaternion q0 = Quaternion.FromToRotation(mFrom.ToSphericalCartesian(), mFrom.ToSphericalCartesian());
        Quaternion q1 = Quaternion.FromToRotation(mFrom.ToSphericalCartesian(), mTo.ToSphericalCartesian());

        Gizmos.color = new Color(1.0f, 0.0f, 0.0f);
        DrawLine(mFrom.ToSphericalCartesian() - new Vector3(scale, 0.0f, 0.0f), mFrom.ToSphericalCartesian() + new Vector3(scale, 0.0f, 0.0f));
        DrawLine(mFrom.ToSphericalCartesian() - new Vector3(0.0f, scale, 0.0f), mFrom.ToSphericalCartesian() + new Vector3(0.0f, scale, 0.0f));
        DrawLine(mFrom.ToSphericalCartesian() - new Vector3(0.0f, 0.0f, scale), mFrom.ToSphericalCartesian() + new Vector3(0.0f, 0.0f, scale));

        float tStep = 1.0f / 100.0f;
        for (int i = 0; i < 100; i++)
        {
            Quaternion qPrev = Quaternion.Slerp(q0, q1, i * tStep);
            Quaternion qNext = Quaternion.Slerp(q0, q1, (i + 1) * tStep);
            Vector3 v0 = qPrev * mFrom.ToSphericalCartesian();
            Vector3 v1 = qNext * mFrom.ToSphericalCartesian();
            DrawLine(v0, v1);
        }

        Vector3 v = Quaternion.Slerp(q0, q1, pathProgress / mDistance) * mFrom.ToSphericalCartesian();

        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
        DrawLine(v - new Vector3(scale, 0.0f, 0.0f), v + new Vector3(scale, 0.0f, 0.0f));
        DrawLine(v - new Vector3(0.0f, scale, 0.0f), v + new Vector3(0.0f, scale, 0.0f));
        DrawLine(v - new Vector3(0.0f, 0.0f, scale), v + new Vector3(0.0f, 0.0f, scale));

        Gizmos.color = new Color(1.0f, 0.0f, 0.0f);
        DrawLine(mTo.ToSphericalCartesian() - new Vector3(scale, 0.0f, 0.0f), mTo.ToSphericalCartesian() + new Vector3(scale, 0.0f, 0.0f));
        DrawLine(mTo.ToSphericalCartesian() - new Vector3(0.0f, scale, 0.0f), mTo.ToSphericalCartesian() + new Vector3(0.0f, scale, 0.0f));
        DrawLine(mTo.ToSphericalCartesian() - new Vector3(0.0f, 0.0f, scale), mTo.ToSphericalCartesian() + new Vector3(0.0f, 0.0f, scale));
    }
}

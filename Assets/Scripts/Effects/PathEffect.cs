using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEffect
{
    private int _iterations;
    private int _routes;
    private LineRenderer _lineRenderer;
    private Material _material;
    private float _offset;
    private float _totalDistance;

    public PathEffect(int iters)
    {
        _iterations = iters;
        _lineRenderer = null;
        _material = null;
    }

    public void Enable(GameObject gameObject, Material material)
    {
        _material = material;
        _lineRenderer = gameObject.GetComponent<LineRenderer>();

        if (_lineRenderer == null)
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.SetWidth(0.01f, 0.01f);
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.material = material;
        }
    }

    public void Disable()
    {
        if (_lineRenderer)
        {
            Component.Destroy(_lineRenderer);
            _lineRenderer = null;
        }
        _routes = 0;
    }

    private void SetRoutes(int count)
    {
        _routes = count;
        if (_lineRenderer != null)
        {
            _lineRenderer.SetVertexCount(_routes * (_iterations + 1));
        }

        if (count == 0)
        {
            _totalDistance = 0.0f;
        }
    }

    public void Clear()
    {
        SetRoutes(0);
    }

    public void AddRoute(GeoPoint a, GeoPoint b)
    {
        int r = _routes;
        SetRoutes(_routes + 1);

        List<Vector3> vertices = new List<Vector3>();

        float distance = 0.0f;

        RoutePath.BuildPath(a, b, _iterations, ref vertices, ref distance);

        _totalDistance += distance;

        for (int i = 0; i < vertices.Count; i++)
        {
            _lineRenderer.SetPosition(r * (_iterations + 1) + i, vertices[i]);
        }
    }

    public void Update(float dt)
    {
        if (_material)
        {
            _material.SetTextureOffset("_MainTex", new Vector2(_offset, _offset));
            _material.SetTextureScale("_MainTex", new Vector2(_totalDistance / 2000000.0f, 1.0f));
            _offset -= dt * 0.1f;
            if (_offset < 0.0f)
            {
                _offset += 1.0f;
            }
        }
    }
}

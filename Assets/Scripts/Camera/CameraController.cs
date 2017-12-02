using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject center;
    public float acceleration = 1.0f;
    public float speed = 1.0f;
    public float EPS = 0.01f;
    private bool dragX = true;
    private bool dragY = true;
    private float speedX = 0.0f;
    private float speedY = 0.0f;
    private float angleX = 0.0f;
    private float angleY = 0.0f;
    public float limitYMin = -60.0f;
    public float limitYMax = 60.0f;
    public float distance = 3.0f;
    private bool dragZoom = true;
    public float speedZoom = 0.0f;
    public float zoomMin = 1.0f;
    public float zoomMax = 5.0f;

    public void Start()
    {
        angleX = transform.eulerAngles.y;
        angleY = transform.eulerAngles.x;
    }

    private static float ClampAngle(ref float speed, float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;

        if (angle > 360.0f)
            angle -= 360.0f;

        if (angle < min)
        {
            speed = 0.0f;
        }

        if (angle > max)
        {
            speed = 0.0f;
        }

        return Mathf.Clamp(angle, min, max);
    }

    private static float ClampZoom(ref float speed, float zoom, float min, float max)
    {
        if (zoom < min)
        {
            speed = 0.0f;
        }

        if (zoom > max)
        {
            speed = 0.0f;
        }

        return Mathf.Clamp(zoom, min, max);
    }

    private void Drag()
    {
        if (dragX)
        {
            if (speedX > EPS)
            {
                speedX -= acceleration * Time.deltaTime;
            }
            else if (speedX < -EPS)
            {
                speedX += acceleration * Time.deltaTime;
            }
            else
            {
                speedX = 0.0f;
            }
        }

        if (dragY)
        {
            if (speedY > EPS)
            {
                speedY -= acceleration * Time.deltaTime;
            }
            else if (speedY < -EPS)
            {
                speedY += acceleration * Time.deltaTime;
            }
            else
            {
                speedY = 0.0f;
            }
        }

        if (dragZoom)
        {
            if (speedZoom > EPS)
            {
                speedZoom -= acceleration * Time.deltaTime;
            }
            else if (speedZoom < -EPS)
            {
                speedZoom += acceleration * Time.deltaTime;
            }
            else
            {
                speedZoom = 0.0f;
            }
        }
    }

    public void LateUpdate()
    {
        dragX = true;
        dragY = true;
        dragZoom = true;

        if (Input.GetKey(KeyCode.D))
        {
            speedX -= acceleration * Time.deltaTime;
            dragX = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            speedX += acceleration * Time.deltaTime;
            dragX = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            speedY -= acceleration * Time.deltaTime;
            dragY = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
            speedY += acceleration * Time.deltaTime;
            dragY = false;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            speedZoom += acceleration * Time.deltaTime;
            dragZoom = false;
        }

        if (Input.GetKey(KeyCode.E))
        {
            speedZoom -= acceleration * Time.deltaTime;
            dragZoom = false;
        }
        
        Drag();

        speedX = Mathf.Clamp(speedX, -speed, speed);
        speedY = Mathf.Clamp(speedY, -speed, speed);
        speedZoom = Mathf.Clamp(speedZoom, -speed, speed);

        angleX += speedX;
        angleY += speedY;
        distance += speedZoom * Time.deltaTime;

        angleY = ClampAngle(ref speedY, angleY, limitYMin, limitYMax);

        Quaternion rotation = Quaternion.Euler(angleY, angleX, 0.0f);
        
        distance = ClampZoom(ref speedZoom, distance, zoomMin, zoomMax);

        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + center.transform.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}

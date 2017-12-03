﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneListItem : MonoBehaviour
{
    public Plane Plane
    {
        get
        {
            return _plane;
        }
        set
        {
            _plane = value;
            _name.text = _plane.Name;
        }
    }

    [SerializeField]
    private Text _name;

    private Plane _plane;

    private void Start()
    {

    }

    private void Update()
    {

    }
}

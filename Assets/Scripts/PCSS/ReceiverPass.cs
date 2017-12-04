using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverPass : MonoBehaviour
{
    public GameObject _shadowCaster;
    private Camera _shadowCamera;
    private ShadowPass _shadow;
    private Material _material;

    void Awake()
    {
        _shadow = _shadowCaster.GetComponent<ShadowPass>();
        _shadowCamera = _shadowCaster.GetComponent<Camera>();
        _material = GetComponent<Renderer>().material;
    }
	
	void Start ()
    {
		
	}
	
	void Update ()
    {
        _material.SetMatrix("_ShadowViewMatrix", _shadow._shadowViewMatrix);
        _material.SetMatrix("_ShadowProjectionMatrix", _shadow._shadowProjectionMatrix);
        _material.SetMatrix("_ShadowBiasMatrix", _shadow._shadowBiasMatrix);
        _material.SetTexture("_ShadowTexture", _shadowCamera.targetTexture);
        _material.SetFloat("_LightSize", _shadow._shadowFilterSize);
        _material.SetFloat("_Bias", _shadow._shadowBias);
        _material.SetFloat("_Offset", 1.0f / _shadow._shadowMapSize);
        _material.SetFloat("_ShadowSize", _shadow._shadowMapSize);
        _material.SetFloat("_NoiseScale", _shadow._noiseScale);
        _material.SetInt("_FilterWidth", _shadow._filterWidth);
        _material.SetVector("_LightDir", new Vector4(_shadowCamera.transform.parent.forward.x, _shadowCamera.transform.parent.forward.y, _shadowCamera.transform.parent.forward.z, 0.0f));
    }
}

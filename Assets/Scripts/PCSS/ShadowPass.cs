using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShadowPass : MonoBehaviour
{
    public int _shadowMapSize;
    public Shader _depthShader;
    public Matrix4x4 _shadowViewMatrix;
    public Matrix4x4 _shadowProjectionMatrix;
    public Matrix4x4 _shadowBiasMatrix;
    public float _lightSize;
    public float _shadowBias;
    public float _shadowFilterSize;
    public int _filterWidth;
    public float _noiseScale;
    private Camera _shadowCamera;

    void Awake()
    {
        _shadowCamera = GetComponent<Camera>();
        _shadowCamera.SetReplacementShader(_depthShader, "");
    }

    void Start()
    {
        _shadowCamera.orthographic = true;
        _shadowCamera.clearFlags = CameraClearFlags.SolidColor;
        RenderTexture _shadowMap = new RenderTexture(_shadowMapSize, _shadowMapSize, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
        _shadowMap.filterMode = FilterMode.Point;
        _shadowMap.wrapMode = TextureWrapMode.Clamp;
        _shadowCamera.targetTexture = _shadowMap;
    }
	
	void Update ()
    {
        _shadowFilterSize = _lightSize / _shadowCamera.orthographicSize;
        _shadowProjectionMatrix = GL.GetGPUProjectionMatrix(_shadowCamera.projectionMatrix, false);
        _shadowViewMatrix = _shadowCamera.worldToCameraMatrix;
        _shadowBiasMatrix = Matrix4x4.identity;
        _shadowBiasMatrix.SetRow(0, new Vector4(0.5f, 0.0f, 0.0f, 0.5f));
        _shadowBiasMatrix.SetRow(1, new Vector4(0.0f, 0.5f, 0.0f, 0.5f));
        _shadowBiasMatrix.SetRow(2, new Vector4(0.0f, 0.0f, 0.5f, 0.5f));
        _shadowBiasMatrix.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
    }
}

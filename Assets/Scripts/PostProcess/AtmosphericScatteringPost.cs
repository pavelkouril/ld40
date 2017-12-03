using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(AtmosphericScatteringRenderer), PostProcessEvent.BeforeStack, "Custom/AtmosphericScattering")]
public sealed class AtmosphericScatteringPost : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("AtmosphericScattering effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };

    public Vector4Parameter cameraPos = new Vector4Parameter { value = new Vector4(0.0f, 0.0f, 0.0f, 0.0f) };
    public Vector4Parameter cameraForward = new Vector4Parameter { value = new Vector4(0.0f, 0.0f, 1.0f, 0.0f) };
    public Vector4Parameter cameraUp = new Vector4Parameter { value = new Vector4(0.0f, 1.0f, 0.0f, 0.0f) };
    public Vector4Parameter cameraRight = new Vector4Parameter { value = new Vector4(1.0f, 0.0f, 0.0f, 0.0f) };
    public Vector4Parameter lightPos = new Vector4Parameter { value = new Vector4(0.0f, 0.0f, 0.0f, 0.0f) };
    public IntParameter width = new IntParameter { value = 1920 };
    public IntParameter height = new IntParameter { value = 1080 };
    public FloatParameter fov = new FloatParameter { value = 45.0f };
    public IntParameter densitySamples = new IntParameter { value = 8 };
    public IntParameter viewSamples = new IntParameter { value = 8 };
    public Vector4Parameter earthCenter = new Vector4Parameter { value = new Vector4(0.0f, 0.0f, 0.0f, 0.0f) };
    public FloatParameter earthRadius = new FloatParameter { value = 1.0f };
    public FloatParameter atmoRadius = new FloatParameter { value = 1.25f };
    public FloatParameter editor = new FloatParameter { value = 1.0f };
}

public sealed class AtmosphericScatteringRenderer : PostProcessEffectRenderer<AtmosphericScatteringPost>
{
    public override void Render(PostProcessRenderContext context)
    {
        CameraController controller = context.camera.GetComponent<CameraController>();
        if (controller != null)
        {
            settings.lightPos.value.x = controller.light.transform.forward.x;
            settings.lightPos.value.y = controller.light.transform.forward.y;
            settings.lightPos.value.z = controller.light.transform.forward.z;

            settings.cameraPos.value.x = controller.transform.position.x;
            settings.cameraPos.value.y = controller.transform.position.y;
            settings.cameraPos.value.z = controller.transform.position.z;

            settings.cameraForward.value.x = controller.transform.forward.x;
            settings.cameraForward.value.y = controller.transform.forward.y;
            settings.cameraForward.value.z = controller.transform.forward.z;

            settings.cameraUp.value.x = controller.transform.up.x;
            settings.cameraUp.value.y = controller.transform.up.y;
            settings.cameraUp.value.z = controller.transform.up.z;

            settings.cameraRight.value.x = controller.transform.right.x;
            settings.cameraRight.value.y = controller.transform.right.y;
            settings.cameraRight.value.z = controller.transform.right.z;

            settings.width.value = context.camera.pixelWidth;
            settings.height.value = context.camera.pixelHeight;
            settings.fov.value = context.camera.fov;

            settings.editor.value = 1.0f;
        }
        else
        {
            settings.editor.value = 0.0f;
        }

        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/AtmosphericScattering"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetVector("_CameraPosition", settings.cameraPos);
        sheet.properties.SetVector("_CameraForward", settings.cameraForward);
        sheet.properties.SetVector("_CameraUp", settings.cameraUp);
        sheet.properties.SetVector("_CameraRight", settings.cameraRight);
        sheet.properties.SetVector("_LightDirection", settings.lightPos);
        sheet.properties.SetFloat("_Width", (float)settings.width);
        sheet.properties.SetFloat("_Height", (float)settings.height);
        sheet.properties.SetFloat("_Fov", settings.fov);
        sheet.properties.SetFloat("_NUM_DENSITY_SAMPLES", (float)settings.densitySamples);
        sheet.properties.SetFloat("_NUM_VIEW_SAMPLES", (float)settings.viewSamples);
        sheet.properties.SetVector("_EarthCenter", settings.earthCenter);
        sheet.properties.SetFloat("_EarthRadius", settings.earthRadius);
        sheet.properties.SetFloat("_AtmoRadius", settings.atmoRadius);
        sheet.properties.SetFloat("_Editor", settings.editor);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}

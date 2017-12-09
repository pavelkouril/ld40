using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class QualityManager : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume _volume;

    private void Start()
    {
        var qualityLevel = QualitySettings.GetQualityLevel();
        var profile = _volume.sharedProfile;
        AtmosphericScatteringPost scattering;
        if (profile.TryGetSettings<AtmosphericScatteringPost>(out scattering))
        {
            switch (qualityLevel)
            {
                case 0:
                    scattering.densitySamples = new IntParameter() { value = 1 };
                    scattering.viewSamples = new IntParameter() { value = 1 };
                    break;
                case 1:
                    scattering.densitySamples = new IntParameter() { value = 2 };
                    scattering.viewSamples = new IntParameter() { value = 2 };
                    break;
                case 2:
                    scattering.densitySamples = new IntParameter() { value = 8 };
                    scattering.viewSamples = new IntParameter() { value = 8 };
                    break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CamPostProcess : MonoBehaviour
{
    PostProcessVolume effect;
    Grain grain;
    ColorGrading colorGrading;
    float Duration = 1f;



    void Awake()
    {
        effect = GameObject.FindWithTag("Settings").GetComponentInChildren<PostProcessVolume>();
        effect.profile.TryGetSettings<Grain>(out grain);
        effect.profile.TryGetSettings<ColorGrading>(out colorGrading);
    }

    IEnumerator GrainStart(float elapsed = 0, float targetInt = 0.6f, float targetSize = 1.63f, float targetLum = 0.27f, float targetColor = 0f)
    {
        float startInt = grain.intensity.value;
        float startSize = grain.size.value;
        float startLum = grain.lumContrib.value;
        float startColor = colorGrading.postExposure.value = -10;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / Duration);
            t = Mathf.SmoothStep(0f, 1f, t);
            colorGrading.postExposure.value = Mathf.Lerp(startColor, targetColor, t);
            yield return null;

        }
        yield return new WaitForSeconds(0.2f);
        elapsed = 0f;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / Duration);
            t = Mathf.SmoothStep(0f, 1f, t);
            grain.intensity.value = Mathf.Lerp(startInt, targetInt, t);
            grain.size.value = Mathf.Lerp(startSize, targetSize, t);
            grain.lumContrib.value = Mathf.Lerp(startLum, targetLum, t);

            yield return null;
        }


        Debug.Log("Smooth transition finished");
    }
    public void ResetAllVals()
    {
        float startInt = grain.intensity.value = 1;
        float startSize = grain.size.value = 3;
        float startLum = grain.lumContrib.value = 0;
        float startColorGranding = colorGrading.postExposure.value = 0;
    }

    public void StartingCoro()
    {
        StartCoroutine(GrainStart());
    }
}

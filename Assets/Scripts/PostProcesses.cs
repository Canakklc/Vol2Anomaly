using System.Collections;
using UnityEngine;

public class PostProcesses : MonoBehaviour
{
    float duration = 0.4f;
    float backToOriginal = 60f;
    float sprintFOV = 85f;
    Coroutine activeCoroutine;

    IEnumerator FOWChange(float targetFOV)
    {
        float startFOV = Camera.main.fieldOfView;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;


            t = Mathf.SmoothStep(0f, 1f, t);

            Camera.main.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }

        Camera.main.fieldOfView = targetFOV;
    }

    public void StartFOW()
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(FOWChange(sprintFOV));
    }

    public void FinishFow()
    {

        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(FOWChange(backToOriginal));
    }
}

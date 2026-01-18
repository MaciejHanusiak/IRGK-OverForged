using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SparkLightPulse : MonoBehaviour
{
    [SerializeField] private Light2D light2D;
    [SerializeField] private float peakIntensity = 0.7f;
    [SerializeField] private float lifeTime = 0.15f;

    private float timer;

    private void OnEnable()
    {
        timer = 0f;
        if (light2D != null)
            light2D.intensity = peakIntensity;
    }

    private void Update()
    {
        if (light2D == null) return;

        timer += Time.deltaTime;
        float t = timer / lifeTime;

        light2D.intensity = Mathf.Lerp(peakIntensity, 0f, t);

        if (t >= 1f)
            light2D.enabled = false;
    }
}
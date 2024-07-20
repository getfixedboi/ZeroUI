using System.Collections;
using UnityEngine;

public class LightBehaviour : MonoBehaviour
{
    public GameManager Manager;
    public Light TargetLight;
    private float _timer;
    [Header("Light behaviour parameters")]
    [Range(0, 1)] public float LightOffDelay;
    public float FlickPeriod;
    [Range(0, 1)] public float IntensityLimit;
    [Range(0, 1)] public float StartFlickingPeriod;

    public GameObject _material;
    public GameObject pointLUGHTEWJGEWGFVEW;

    public Material origig;
    public Material secondoric;

    void Awake()
    {
        TargetLight = GetComponent<Light>();

    }

    void Update()
    {
        if (!Manager.StartGame)
        {
            return;
        }

        float variableValue = Manager.CurrentLampDutability / 100;
        if (TargetLight.intensity >= IntensityLimit)
        {
            TargetLight.intensity = variableValue;
        }

        _timer += Time.deltaTime;
        if (_timer >= FlickPeriod * variableValue && variableValue <= StartFlickingPeriod)
        {
            StartCoroutine(C_Flick());
            _timer = 0;
        }

    }
    private IEnumerator C_Flick()
    {
        TargetLight.enabled = false;
        _material.GetComponent<Renderer>().material = secondoric;
        pointLUGHTEWJGEWGFVEW.SetActive(false);
        yield return new WaitForSeconds(LightOffDelay);
        _material.GetComponent<Renderer>().material = origig;
        TargetLight.enabled = true;
        pointLUGHTEWJGEWGFVEW.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorChanger1 : MonoBehaviour
{
    [SerializeField] private Image _heatImage;
    [SerializeField] private Image _oxygenImage;
    [SerializeField] private GameManager _manager;
    private float heatMultiplier;
    private float oxygenMultiplier;

    private AudioSource AudioSource;

    public AudioClip clipPOPOK;
    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        heatMultiplier = 1;
        oxygenMultiplier = 1;
        StartCoroutine(C_BlinkingIndicator());
    }

    private void Update()
    {
        heatMultiplier = _manager.CurrentHeatCapacity / 100;
        oxygenMultiplier = _manager.CurrentOxygenCapacity / 100;

        _oxygenImage.color = new Color(1 - oxygenMultiplier, 1 * oxygenMultiplier, 0, 1);
        _heatImage.color = new Color(1 - heatMultiplier, 1 * heatMultiplier, 0, 1);
    }
    private IEnumerator C_BlinkingIndicator()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            if (oxygenMultiplier <= 0.25f)
            {
                AudioSource.PlayOneShot(clipPOPOK);
                _oxygenImage.enabled = !_oxygenImage.IsActive();
            }
            else
            {
                _oxygenImage.enabled = true;
            }

            if (heatMultiplier <= 0.25f)
            {
                AudioSource.PlayOneShot(clipPOPOK);
                _heatImage.enabled = !_heatImage.IsActive();
            }
            else
            {
                _heatImage.enabled = true;
            }
        }
    }
}

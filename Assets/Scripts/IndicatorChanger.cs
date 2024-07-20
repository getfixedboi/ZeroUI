using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorChanger : MonoBehaviour
{
    [SerializeField] private Image _hammerImage;
    [SerializeField] private Image _gasolineImage;
    [SerializeField] private GameManager _manager;
    private float gasMultiplier;
    private float generatorMultiplier;
    private void Start()
    {
        gasMultiplier = 1;
        generatorMultiplier = 1;
        StartCoroutine(C_BlinkingIndicator());
    }

    private void Update()
    {
        gasMultiplier = _manager.CurrentGasCapacity / 100;
        generatorMultiplier = _manager.CurrentGeneratorDutability / 100;

        _hammerImage.color = new Color(1 - generatorMultiplier, 1 * generatorMultiplier, 0, 1);
        _gasolineImage.color = new Color(1 - gasMultiplier, 1 * gasMultiplier, 0, 1);
    }
    private IEnumerator C_BlinkingIndicator()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            if (gasMultiplier <= 0.25f)
            {
                _gasolineImage.enabled = !_gasolineImage.IsActive();
            }
            else
            {
                _gasolineImage.enabled = true;
            }

            if (generatorMultiplier <= 0.25f)
            {
                _hammerImage.enabled = !_hammerImage.IsActive();
            }
            else
            {
                _hammerImage.enabled = true;
            }
        }
    }
}

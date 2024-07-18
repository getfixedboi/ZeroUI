using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
[DisallowMultipleComponent]
public class GameManager : Interactable
{
    [SerializeField][Range(1, 100)] private float _maxGasCapacity;
    [SerializeField][Range(1, 100)] private float _maxGeneratorDutability;
    [SerializeField][Range(1, 100)] private float _maxLampDutability;
    [Space]
    private float _currentGasCapacity;
    private float _currentGeneratorDutability;
    private float _currentLampDutability;
    [SerializeField][Range(0, 5)] private float _startGasCost;
    [SerializeField][Range(0, 5)] private float _startGeneratorCost;
    [SerializeField][Range(0, 5)] private float _startLampCost;
    private float _timer = 0;
    private bool startGame = false;

    private bool _level1Reached = false;
    private bool _level2Reached = false;
    private bool _level3Reached = false;
    private bool _level4Reached = false;
    private bool _level5Reached = false;

    protected override void Awake()
    {
        base.Awake();
        _currentGasCapacity = _maxGasCapacity;
        _currentGeneratorDutability = _maxGeneratorDutability;
        _currentLampDutability = _maxLampDutability;
    }

    public override void OnInteract()
    {
        if (!startGame)
        {
            startGame = true;
        }
        else if(startGame)
        {
            switch (ItemHandler.CurrentType)
            {
                case ItemHandler.TypeList.hammer:
                    {
                        _currentGeneratorDutability += _maxGeneratorDutability * 0.25f;
                        if (_currentGeneratorDutability > _maxGeneratorDutability)
                        {
                            _currentGeneratorDutability = _maxGeneratorDutability;
                        }
                        break;
                    }
                case ItemHandler.TypeList.lamp:
                    {
                        _currentLampDutability += _maxLampDutability * 0.25f;
                        if (_currentLampDutability > _maxLampDutability)
                        {
                            _currentLampDutability = _maxLampDutability;
                        }
                        break;
                    }
                case ItemHandler.TypeList.gas:
                    {
                        _currentGasCapacity += _maxGasCapacity * 0.25f;
                        if (_currentGasCapacity > _maxGasCapacity)
                        {
                            _currentGasCapacity = _maxGasCapacity;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    private void Update()
    {
        ClearConsole();
        Debug.Log($"time - {_timer}");
        Debug.Log($"gas - {_currentGasCapacity}");
        Debug.Log($"generator - {_currentGeneratorDutability}");
        Debug.Log($"lamp - {_currentLampDutability}");

        if (!startGame)
        {
            return;
        }
        else if (_currentGasCapacity <= 0 || _currentGeneratorDutability <= 0 || _currentLampDutability <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            _timer += Time.deltaTime;

            _currentGasCapacity -= _maxGasCapacity * _startGasCost * Time.deltaTime;
            _currentGeneratorDutability -= _maxGeneratorDutability * _startGeneratorCost * Time.deltaTime;
            _currentLampDutability -= _maxLampDutability * _startLampCost * Time.deltaTime;

            if (_timer >= 60 && !_level1Reached)
            {
                _startGasCost += 0.01f;
                _startGeneratorCost += 0.01f;
                _startLampCost += 0.01f;
                _level1Reached = true;
            }
            else if (_timer >= 120 && !_level2Reached)
            {
                _startGasCost *= 1.2f;
                _startGeneratorCost *= 1.2f;
                _startLampCost *= 1.2f;
                _level2Reached = true;
            }
            else if (_timer >= 180 && !_level3Reached)
            {
                _startGasCost *= 1.1f;
                _startGeneratorCost *= 1.1f;
                _startLampCost *= 1.1f;
                _level3Reached = true;
            }
            else if (_timer >= 240 && !_level4Reached)
            {
                _startGasCost *= 1.1f;
                _startGeneratorCost *= 1.1f;
                _startLampCost *= 1.1f;
                _level4Reached = true;
            }
            else if (_timer >= 300 && !_level5Reached)
            {
                _startGasCost *= 1.1f;
                _startGeneratorCost *= 1.1f;
                _startLampCost *= 1.1f;
                _level5Reached = true;
            }
            else if(_timer >= 360)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
        }
    }

    public static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }

    public override void OnFocus() { }
    public override void OnLoseFocus() { }
}

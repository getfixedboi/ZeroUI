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
    [HideInInspector] public float CurrentLampDutability;
    [SerializeField][Range(0, 5)] private float _startGasCost;
    [SerializeField][Range(0, 5)] private float _startGeneratorCost;
    [SerializeField][Range(0, 5)] private float _startLampCost;
    private float _timer = 0;
    [Space]
    public bool StartGame = false;
    #region shitcode
    private bool _level1Reached = false;
    private bool _level2Reached = false;
    private bool _level3Reached = false;
    private bool _level4Reached = false;
    private bool _level5Reached = false;
    #endregion
    private bool _interactCD = false;
    private bool _gasTankOpened = false;
    [Header("Interact sounds")]
    [SerializeField] private AudioClip _gasolineSound;
    [SerializeField] private AudioClip _wrenchSound;
    [SerializeField] private AudioClip _hammerSound;
    [SerializeField] private AudioClip _lampSound;

    protected override void Awake()
    {
        base.Awake();
        _currentGasCapacity = _maxGasCapacity;
        _currentGeneratorDutability = _maxGeneratorDutability;
        CurrentLampDutability = _maxLampDutability;
    }

    public override void OnInteract()
    {
        if (!StartGame)
        {
            StartGame = true;
        }
        else if (StartGame && !_interactCD)
        {
            switch (ItemHandler.CurrentType)
            {
                case ItemHandler.TypeList.hammer:
                    {
                        if (!_gasTankOpened)
                        {
                            source.PlayOneShot(_hammerSound);
                            StartCoroutine(C_InteractCD());
                            _currentGeneratorDutability += _maxGeneratorDutability * 0.25f;
                            if (_currentGeneratorDutability > _maxGeneratorDutability)
                            {
                                _currentGeneratorDutability = _maxGeneratorDutability;
                            }
                        }
                        else
                        {
                            source.PlayOneShot(errorSound);
                        }
                        break;
                    }
                case ItemHandler.TypeList.lamp:
                    {
                        if (!_gasTankOpened)
                        {
                            source.PlayOneShot(_lampSound);
                            StartCoroutine(C_InteractCD());
                            CurrentLampDutability += _maxLampDutability * 0.25f;
                            if (CurrentLampDutability > _maxLampDutability)
                            {
                                CurrentLampDutability = _maxLampDutability;
                            }
                        }
                        else
                        {
                            source.PlayOneShot(errorSound);
                        }
                        break;
                    }
                case ItemHandler.TypeList.gasoline:
                    {
                        if (_gasTankOpened)
                        {
                            source.PlayOneShot(_gasolineSound);
                            StartCoroutine(C_InteractCD());
                            _currentGasCapacity += _maxGasCapacity * 0.25f;
                            if (_currentGasCapacity > _maxGasCapacity)
                            {
                                _currentGasCapacity = _maxGasCapacity;
                            }
                        }
                        else
                        {
                            source.PlayOneShot(errorSound);
                        }
                        break;
                    }
                case ItemHandler.TypeList.wrench:
                    {
                        if (_gasTankOpened)
                        {
                            source.PlayOneShot(_wrenchSound);
                            StartCoroutine(C_InteractCD());
                            _gasTankOpened = false;
                        }
                        else
                        {
                            source.PlayOneShot(_wrenchSound);
                            StartCoroutine(C_InteractCD());
                            _gasTankOpened = true;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        else
        {
            source.PlayOneShot(clip);
        }
    }

    private void Update()
    {
        ClearConsole();
        Debug.Log($"time - {_timer}");
        Debug.Log($"gas - {_currentGasCapacity}");
        Debug.Log($"generator - {_currentGeneratorDutability}");
        Debug.Log($"lamp - {CurrentLampDutability}");

        if (!StartGame)
        {
            return;
        }
        else if (_currentGasCapacity <= 0 || _currentGeneratorDutability <= 0 || CurrentLampDutability <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            _timer += Time.deltaTime;

            _currentGasCapacity -= _maxGasCapacity * _startGasCost * Time.deltaTime;
            _currentGeneratorDutability -= _maxGeneratorDutability * _startGeneratorCost * Time.deltaTime;
            CurrentLampDutability -= _maxLampDutability * _startLampCost * Time.deltaTime;

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
            else if (_timer >= 360)
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

    private IEnumerator C_InteractCD()
    {
        _interactCD = true;
        yield return new WaitForSeconds(1.5f);
        _interactCD = false;
    }
}

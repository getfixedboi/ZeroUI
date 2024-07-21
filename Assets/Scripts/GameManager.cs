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
    [Range(1, 100)] public float _maxLampDutability;
    [Space]
    [HideInInspector] public float CurrentGasCapacity;
    [HideInInspector] public float CurrentGeneratorDutability;
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
    #endregion
    [HideInInspector]
    public bool _interactCD = false;

    [HideInInspector]
    public bool GasTankOpened = false;

    [Header("Interact sounds")]
    [SerializeField] private AudioClip _gasolineSound;
    public AudioClip _wrenchSound;
    [SerializeField] private AudioClip _hammerSound;
    public AudioClip _lampSound;
    [Header("Крышки от генератора")]
    public GameObject _closedKrishka;
    public GameObject _openedKrishka;
    [Header("Лампочка от генератора")]
    public GameObject _lampochka;
    [Header("Для проигрыша")]
    private bool gameEnd=false;
    public AudioClip RunOutOfFuel;
    public AudioClip RunOutOfDurability;
    public AudioClip RunOutOfLamp;
    public InteractRaycaster interactRaycaster;
    public GameObject Light1;
    public GameObject Light2;
    public GameObject Lamp;
    public Material BlackMaterial;
    public GameObject Indicator;
    public GameObject EngineSound;
    [Space]
    public UnityEngine.UI.Image DeathScreen;
    public AudioSource DeathSoundSource;
    public AudioClip HardBreath;
    [Header("Для победы")]
    public GameObject ClosedDoor;
    public AudioClip DoorSound;
    public AudioClip WinningAmbient;
    public AudioSource PlayerSource;
    public AudioSource DoorSource;



    public GameObject[] strelochki;
    public AudioSource sourceSTRELOCHEK;
    public AudioClip strelochaTik;
    protected override void Awake()
    {
        base.Awake();
        //_timer = 200;
        CurrentGasCapacity = _maxGasCapacity;
        CurrentGeneratorDutability = _maxGeneratorDutability;
        CurrentLampDutability = _maxLampDutability;

        _openedKrishka.SetActive(false);
        _closedKrishka.SetActive(true);
    }

    public override void OnInteract()
    {
        if (!StartGame)
        {
            source.PlayOneShot(errorSound);
        }
        else if (StartGame && !_interactCD)
        {
            switch (ItemHandler.CurrentType)
            {
                case ItemHandler.TypeList.hammer:
                    {
                        if (!GasTankOpened)
                        {
                            source.PlayOneShot(_hammerSound);
                            StartCoroutine(C_InteractCD());
                            CurrentGeneratorDutability += _maxGeneratorDutability * 0.25f;
                            if (CurrentGeneratorDutability > _maxGeneratorDutability)
                            {
                                CurrentGeneratorDutability = _maxGeneratorDutability;
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
                        if (GasTankOpened)
                        {
                            source.PlayOneShot(_gasolineSound);
                            StartCoroutine(C_InteractCD());
                            CurrentGasCapacity += _maxGasCapacity * 0.25f;
                            if (CurrentGasCapacity > _maxGasCapacity)
                            {
                                CurrentGasCapacity = _maxGasCapacity;
                            }
                        }
                        else
                        {
                            source.PlayOneShot(errorSound);
                        }
                        break;
                    }
                default:
                    {
                        source.PlayOneShot(errorSound);
                        break;
                    }
            }
        }
        else
        {
            source.PlayOneShot(errorSound);
        }
    }

    private void Update()
    {
        //ClearConsole();
        //Debug.Log($"time - {_timer}");
        //Debug.Log($"gas - {CurrentGasCapacity}");
        //Debug.Log($"generator - {CurrentGeneratorDutability}");
        //Debug.Log($"lamp - {CurrentLampDutability}");

        if (!StartGame)
        {
            return;
        }
        
        if(gameEnd)
        {
            return;
        }

        if (CurrentGasCapacity <= 0)
        {
            Lamp.GetComponent<Renderer>().material = BlackMaterial;
            source.PlayOneShot(RunOutOfFuel);
            StartCoroutine(C_Death());
        }
        else if(CurrentGeneratorDutability <= 0)
        {
            Lamp.GetComponent<Renderer>().material = BlackMaterial;
            source.PlayOneShot(RunOutOfDurability);
            StartCoroutine(C_Death());
        }
        else if(CurrentLampDutability <= 0)
        {
            Lamp.SetActive(false);
            source.PlayOneShot(RunOutOfLamp);
            StartCoroutine(C_Death());
        }   
        else
        {
            _timer += Time.deltaTime;

            CurrentGasCapacity -= _maxGasCapacity * _startGasCost * Time.deltaTime;
            CurrentGeneratorDutability -= _maxGeneratorDutability * _startGeneratorCost * Time.deltaTime;
            CurrentLampDutability -= _maxLampDutability * _startLampCost * Time.deltaTime;

            if (_timer >= 60 && !_level1Reached)
            {
              
                sourceSTRELOCHEK.PlayOneShot(strelochaTik);
                strelochki[0].SetActive(false);
                strelochki[1].SetActive(true);
                _level1Reached = true;
            }
            else if (_timer >= 120 && !_level2Reached)
            {
                _startGasCost += 0.01f;
                _startGeneratorCost += 0.01f;
                _startLampCost += 0.01f;
                sourceSTRELOCHEK.PlayOneShot(strelochaTik);
                strelochki[1].SetActive(false);
                strelochki[2].SetActive(true);
                _level2Reached = true;
            }
            else if (_timer >= 180 && !_level3Reached)
            {
                //_startGasCost += 0.01f;
                //_startGeneratorCost += 0.01f;
                //_startLampCost += 0.01f;
                sourceSTRELOCHEK.PlayOneShot(strelochaTik);
                strelochki[2].SetActive(false);
                strelochki[3].SetActive(true);
                _level3Reached = true;
            }
            else if (_timer >= 240 && !_level4Reached)
            {
                _startGasCost += 0.01f;
                _startGeneratorCost += 0.01f;
                _startLampCost += 0.01f;
                sourceSTRELOCHEK.PlayOneShot(strelochaTik);
                strelochki[3].SetActive(false);
                strelochki[4].SetActive(true);
                _level4Reached = true;
            }
            else if (_timer >= 300)
            {
                ClosedDoor.SetActive(false);
                DoorSource.PlayOneShot(DoorSound);
                PlayerSource.PlayOneShot(WinningAmbient);
                interactRaycaster.enabled=false;
                sourceSTRELOCHEK.PlayOneShot(strelochaTik);
                strelochki[4].SetActive(false);
                strelochki[5].SetActive(true);
                EngineSound.SetActive(false);
                this.enabled=false;
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
        yield return new WaitForSeconds(1f);
        _interactCD = false;
    }
    public void StartCD()
    {
        StartCoroutine(C_InteractCD());
    }
    public IEnumerator C_Death()
    {
        gameEnd=true;
        interactRaycaster.enabled=false;
        Light1.SetActive(false);
        Light2.SetActive(false);
        Indicator.SetActive(false);
        EngineSound.SetActive(false);

        yield return new WaitForSeconds(2f);
        source.PlayOneShot(HardBreath);

        float counter=0;
        while(counter<=100)
        {
            DeathScreen.color = new Color(DeathScreen.color.r,DeathScreen.color.g,DeathScreen.color.b,counter/100);
            DeathSoundSource.volume = counter/100;
            counter++;
            yield return new WaitForSeconds(.04f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

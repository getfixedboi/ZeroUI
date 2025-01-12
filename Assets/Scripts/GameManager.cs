using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Data;
using Mono.Data.Sqlite;

[RequireComponent(typeof(Collider))]
[DisallowMultipleComponent]
public class GameManager : Interactable
{
    [SerializeField][Range(1, 100)] private float _maxGasCapacity;

    [SerializeField][Range(1, 100)] private float _maxHeatCapacity;
    [Range(1, 100)] public float _maxOxygenCapacity;

    [SerializeField][Range(1, 100)] private float _maxGeneratorDutability;
    [Range(1, 100)] public float _maxLampDutability;
    [Space]
    [HideInInspector] public float CurrentGasCapacity;

    [HideInInspector] public float CurrentHeatCapacity;
    [HideInInspector] public float CurrentOxygenCapacity;

    [HideInInspector] public float CurrentGeneratorDutability;
    [HideInInspector] public float CurrentLampDutability;
    [SerializeField][Range(0, 5)] private float _startGasCost;
    [SerializeField][Range(0, 5)] private float _startGeneratorCost;
    [SerializeField][Range(0, 5)] private float _startLampCost;

    [SerializeField][Range(0, 5)] private float _startHeatCost;
    [SerializeField][Range(0, 5)] private float _startOxygenCost;
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
    [SerializeField] private AudioClip _ognetushitelSound;
    public AudioClip _lampSound;
    [Header("Крышки от генератора")]
    public GameObject _closedKrishka;
    public GameObject _openedKrishka;
    [Header("Лампочка от генератора")]
    public GameObject _lampochka;
    [Header("Для проигрыша")]
    private bool gameEnd = false;
    public AudioClip RunOutOfFuel;
    public AudioClip RunOutOfDurability;
    public AudioClip Overheat;
    public AudioClip RunOutOfLamp;
    public InteractRaycaster interactRaycaster;
    public GameObject Light1;
    public GameObject Light2;
    public GameObject Lamp;
    public Material BlackMaterial;
    public GameObject Indicator;
    public GameObject Indicator1;
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


    public AudioClip vstuplenie;
    public UnityEngine.UI.Image vstupitScreen;

    public Vent vent;

    public Vavle vavle;

    public AudioSource vstuplenieSource;

    //for database suka blyat
    private float db_add_gas = 0;
    private float db_add_durability = 0;
    public static int db_add_lamp = 0;
    private float db_add_heat = 0;
    [SerializeField] private SaveAdmin saveAdmin;

    protected override void Awake()
    {
        base.Awake();
        //_timer = 200;
        StartCoroutine(C_Vstuplenie());
        CurrentGasCapacity = _maxGasCapacity;
        CurrentGeneratorDutability = _maxGeneratorDutability;
        CurrentLampDutability = _maxLampDutability;

        CurrentHeatCapacity = _maxHeatCapacity;
        CurrentOxygenCapacity = _maxOxygenCapacity;

        _openedKrishka.SetActive(false);
        _closedKrishka.SetActive(true);

        saveAdmin.LoadValuesFromXML_Outscene();
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
                                db_add_durability += CurrentGeneratorDutability - _maxGeneratorDutability;
                                CurrentGeneratorDutability = _maxGeneratorDutability;
                            }
                            else
                            {
                                db_add_durability += _maxGeneratorDutability * 0.25f;
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
                                db_add_gas += CurrentGasCapacity - _maxGasCapacity;
                                CurrentGasCapacity = _maxGasCapacity;
                            }
                            else
                            {
                                db_add_gas += _maxGasCapacity * 0.25f;
                            }
                        }
                        else
                        {
                            source.PlayOneShot(errorSound);
                        }
                        break;
                    }
                case ItemHandler.TypeList.ognetushitel:
                    {
                        source.PlayOneShot(_ognetushitelSound);///
                        StartCoroutine(C_InteractCD());
                        CurrentHeatCapacity += _maxHeatCapacity * 0.25f;
                        if (CurrentHeatCapacity > _maxHeatCapacity)
                        {
                            db_add_heat += CurrentHeatCapacity - _maxHeatCapacity;
                            CurrentHeatCapacity = _maxHeatCapacity;
                        }
                        else
                        {
                            db_add_heat += _maxHeatCapacity * 0.25f;
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

        if (gameEnd)
        {
            return;
        }

        if (CurrentGasCapacity <= 0)
        {
            Lamp.GetComponent<Renderer>().material = BlackMaterial;
            source.PlayOneShot(RunOutOfFuel);
            StartCoroutine(C_Death());
        }
        else if (CurrentGeneratorDutability <= 0)
        {
            Lamp.GetComponent<Renderer>().material = BlackMaterial;
            source.PlayOneShot(RunOutOfDurability);
            StartCoroutine(C_Death());
        }
        else if (CurrentHeatCapacity <= 0)
        {
            Lamp.GetComponent<Renderer>().material = BlackMaterial;
            source.PlayOneShot(Overheat);
            StartCoroutine(C_Death());
        }
        else if (CurrentLampDutability <= 0)
        {
            Lamp.SetActive(false);
            source.PlayOneShot(RunOutOfLamp);
            StartCoroutine(C_Death());
        }
        else if (CurrentOxygenCapacity <= 0)
        {
            StartCoroutine(C_OxygenDeath());
        }
        else
        {
            _timer += Time.deltaTime;

            CurrentGasCapacity -= _maxGasCapacity * _startGasCost * Time.deltaTime * SaveAdmin.GasMul;
            CurrentGeneratorDutability -= _maxGeneratorDutability * _startGeneratorCost * Time.deltaTime * SaveAdmin.DurMul;
            CurrentLampDutability -= _maxLampDutability * _startLampCost * Time.deltaTime * SaveAdmin.LampMul;

            CurrentOxygenCapacity -= _maxOxygenCapacity * _startOxygenCost * Time.deltaTime;
            CurrentHeatCapacity -= _maxHeatCapacity * _startHeatCost * Time.deltaTime * SaveAdmin.TempMul;

            if (_timer >= 60 && !_level1Reached)
            {
                _startGasCost += 0.002f;
                _startGeneratorCost += 0.002f;
                _startLampCost += 0.01f;

                _startHeatCost += 0.01f;
                _startOxygenCost += 0.01f;

                sourceSTRELOCHEK.PlayOneShot(strelochaTik);
                strelochki[0].SetActive(false);
                strelochki[1].SetActive(true);
                _level1Reached = true;
            }
            else if (_timer >= 120 && !_level2Reached)
            {
                _startGasCost += 0.001f;
                _startGeneratorCost += 0.001f;
                _startLampCost += 0.002f;

                _startHeatCost += 0.01f;
                _startOxygenCost += 0.01f;

                sourceSTRELOCHEK.PlayOneShot(strelochaTik);
                strelochki[1].SetActive(false);
                strelochki[2].SetActive(true);
                _level2Reached = true;
            }
            else if (_timer >= 180 && !_level3Reached)
            {
                _startGasCost += 0.001f;
                _startGeneratorCost += 0.001f;
                _startLampCost += 0.002f;

                _startHeatCost += 0.01f;
                _startOxygenCost += 0.01f;

                sourceSTRELOCHEK.PlayOneShot(strelochaTik);
                strelochki[2].SetActive(false);
                strelochki[3].SetActive(true);
                _level3Reached = true;
            }
            else if (_timer >= 240 && !_level4Reached)
            {
                _startGasCost += 0.001f;
                _startGeneratorCost += 0.001f;
                _startLampCost += 0.001f;

                _startHeatCost += 0.01f;
                _startOxygenCost += 0.01f;

                sourceSTRELOCHEK.PlayOneShot(strelochaTik);
                strelochki[3].SetActive(false);
                strelochki[4].SetActive(true);
                _level4Reached = true;
            }
            else if (_timer >= 300)
            {
                SaveDataToDB();

                CurrentGasCapacity = _maxGasCapacity;
                CurrentGeneratorDutability = _maxGeneratorDutability;
                CurrentLampDutability = _maxLampDutability;

                CurrentHeatCapacity = _maxHeatCapacity;
                CurrentOxygenCapacity = _maxOxygenCapacity;

                ClosedDoor.SetActive(false);
                DoorSource.PlayOneShot(DoorSound);
                PlayerSource.PlayOneShot(WinningAmbient);
                interactRaycaster.enabled = false;
                sourceSTRELOCHEK.PlayOneShot(strelochaTik);
                strelochki[4].SetActive(false);
                strelochki[5].SetActive(true);
                EngineSound.SetActive(false);
                this.enabled = false;
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
    public bool IsCatscene = false;
    public IEnumerator C_Vstuplenie()
    {
        IsCatscene = true;
        vstuplenieSource.PlayOneShot(vstuplenie);
        float counter = 100;
        while (counter >= 0)
        {
            vstupitScreen.color = new Color(vstupitScreen.color.r, vstupitScreen.color.g, vstupitScreen.color.b, counter / 100);

            counter--;
            yield return new WaitForSeconds(.04f);
        }
        yield return new WaitForSeconds(1f);
        IsCatscene = false;
    }
    public IEnumerator C_Death()
    {
        SaveDataToDB();
        gameEnd = true;
        interactRaycaster.enabled = false;
        Light1.SetActive(false);
        Light2.SetActive(false);
        Indicator.SetActive(false);
        Indicator1.SetActive(false);
        EngineSound.SetActive(false);

        yield return new WaitForSeconds(2f);
        DeathSoundSource.PlayOneShot(HardBreath);

        float counter = 0;
        while (counter <= 100)
        {
            DeathScreen.color = new Color(DeathScreen.color.r, DeathScreen.color.g, DeathScreen.color.b, counter / 100);
            DeathSoundSource.volume = counter / 100;
            counter++;
            yield return new WaitForSeconds(.04f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public IEnumerator C_OxygenDeath()
    {
        SaveDataToDB();
        gameEnd = true;
        interactRaycaster.enabled = false;

        vavle.enabled = false;
        vent.enabled = false;

        yield return new WaitForSeconds(2f);
        DeathSoundSource.PlayOneShot(HardBreath);

        float counter = 0;
        while (counter <= 100)
        {
            DeathScreen.color = new Color(DeathScreen.color.r, DeathScreen.color.g, DeathScreen.color.b, counter / 100);
            DeathSoundSource.volume = counter / 100;
            counter++;
            yield return new WaitForSeconds(.04f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SaveDataToDB()
    {
        using (var connection = new SqliteConnection(Reg.dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE userGasRefilled SET gas = gas + @gasToAdd WHERE username = @username;";
                command.Parameters.AddWithValue("@username", Reg.currentUser);
                command.Parameters.AddWithValue("@gasToAdd", db_add_gas);
                command.ExecuteNonQuery();

                command.CommandText = "UPDATE userGenDurRefilled SET genDur = genDur + @genDurToAdd WHERE username = @username;";
                command.Parameters.AddWithValue("@genDurToAdd", db_add_durability);
                command.ExecuteNonQuery();

                command.CommandText = "UPDATE userLampChanged SET lampCount = lampCount + @lampCountToAdd WHERE username = @username;";
                command.Parameters.AddWithValue("@lampCountToAdd", db_add_lamp);
                command.ExecuteNonQuery();

                command.CommandText = "UPDATE userHeatReduced SET heat = heat + @heatToAdd WHERE username = @username;";
                command.Parameters.AddWithValue("@heatToAdd", db_add_heat);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}

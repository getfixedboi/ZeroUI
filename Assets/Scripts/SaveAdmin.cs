using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Data;
using Mono.Data.Sqlite; // Не забудьте добавить ссылку на Mono.Data.Sqlite

public class SaveAdmin : MonoBehaviour
{
    [Header("Reg form components")]
    [SerializeField] private TMP_InputField GasMulField;
    [SerializeField] private TMP_InputField DurMulField;
    [SerializeField] private TMP_InputField LampMulField;
    [SerializeField] private TMP_InputField TempMulField;

    public static float GasMul;
    public static float DurMul;
    public static float LampMul;
    public static float TempMul;

    public static string dbName = "URI=file:players.db";

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            CreateTableIfNotExists();
            LoadValuesFromDatabase();
        }
    }

    public void GoBack()
    {
        GasMul = float.Parse(GasMulField.text);
        DurMul = float.Parse(DurMulField.text);
        LampMul = float.Parse(LampMulField.text);
        TempMul = float.Parse(TempMulField.text);

        UpdateValuesInDatabase();
        SceneManager.LoadScene(0);
    }

    private void CreateTableIfNotExists()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS difficult (setting_id INTEGER PRIMARY KEY, GasMul FLOAT, DurMul FLOAT, LampMul FLOAT, TempMul FLOAT);";
                command.ExecuteNonQuery();
            }
        }
    }

    public void LoadValuesFromDatabase()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT GasMul, DurMul, LampMul, TempMul FROM difficult WHERE setting_id = 1;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        GasMul = reader.GetFloat(0);
                        DurMul = reader.GetFloat(1);
                        LampMul = reader.GetFloat(2);
                        TempMul = reader.GetFloat(3);
                    }
                    else
                    {
                        // Если нет записей, инициализируем значения по умолчанию
                        GasMul = 1;
                        DurMul = 1;
                        LampMul = 1;
                        TempMul = 1;

                        // Сохраняем значения по умолчанию в БД
                        UpdateValuesInDatabase();
                    }
                }
            }
        }

        // Обновляем поля ввода
        GasMulField.text = GasMul.ToString();
        DurMulField.text = DurMul.ToString();
        LampMulField.text = LampMul.ToString();
        TempMulField.text = TempMul.ToString();
    }

    public void LoadValuesFromDatabase_Outscene()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT GasMul, DurMul, LampMul, TempMul FROM difficult WHERE setting_id = 1;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        GasMul = reader.GetFloat(0);
                        DurMul = reader.GetFloat(1);
                        LampMul = reader.GetFloat(2);
                        TempMul = reader.GetFloat(3);
                    }
                    else
                    {
                        // Если нет записей, инициализируем значения по умолчанию
                        GasMul = 1;
                        DurMul = 1;
                        LampMul = 1;
                        TempMul = 1;

                        // Сохраняем значения по умолчанию в БД
                        UpdateValuesInDatabase();
                    }
                }
            }
        }
    }
    private void UpdateValuesInDatabase()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO difficult (setting_id, GasMul, DurMul, LampMul, TempMul) VALUES (1, @GasMul, @DurMul, @LampMul, @TempMul);";
                command.Parameters.AddWithValue("@GasMul", GasMul);
                command.Parameters.AddWithValue("@DurMul", DurMul);
                command.Parameters.AddWithValue("@LampMul", LampMul);
                command.Parameters.AddWithValue("@TempMul", TempMul);
                command.ExecuteNonQuery();
            }
        }
    }
}

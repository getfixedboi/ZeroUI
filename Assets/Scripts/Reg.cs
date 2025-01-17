using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Reg : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField confirmPasswordField;
    [Space]
    [SerializeField] Text errorText;
    public static string dbName = "URI=file:players.db";

    [HideInInspector] public static string currentUser;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Start()
    {
        CreateDB();
    }

    private void CreateDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Создание таблицы 1
                command.CommandText = "CREATE TABLE IF NOT EXISTS users (username VARCHAR(20) PRIMARY KEY, password VARCHAR(20));";
                command.ExecuteNonQuery();

                // Создание таблицы 2
                command.CommandText = "CREATE TABLE IF NOT EXISTS userVisiting (username VARCHAR(20), account_creation_date DATETIME, last_visit_date DATETIME, FOREIGN KEY(username) REFERENCES users(username) ON DELETE CASCADE);";
                command.ExecuteNonQuery();

                // Создание таблицы 3
                command.CommandText = "CREATE TABLE IF NOT EXISTS userStats (username VARCHAR(20), gas float, genDur float, lampCount int,heat float, oxygen float,FOREIGN KEY(username) REFERENCES users(username) ON DELETE CASCADE);";
                command.ExecuteNonQuery();

                // Создание таблицы 4
                command.CommandText = "CREATE TABLE IF NOT EXISTS difficult (setting_id PRIMARY KEY,GasMul float , DurMul float, LampMul float, TempMul float);";
                command.ExecuteNonQuery();

                // Создание таблицы 5
                command.CommandText = "CREATE TABLE IF NOT EXISTS userExtraStats (username VARCHAR(20), deaths int, timePassed, FOREIGN KEY(username) REFERENCES users(username) ON DELETE CASCADE);";
                command.ExecuteNonQuery();

                // Заполнение таблицы difficult с одной записью
                //command.CommandText = "INSERT INTO difficult (setting_id, GasMul, DurMul, LampMul, TempMul) VALUES (1, 1.0, 1.0, 1.0, 1.0) ;";
                //command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    private bool Validation()
    {
        errorText.text = "Invalid username or password";
        return usernameField.text != "" && passwordField.text != "" && usernameField.text.Length > 4 && passwordField.text.Length > 4 && passwordField.text == confirmPasswordField.text;
    }

    public void Login()
    {
        bool A = false;
        if (Validation())
        {
            string username = usernameField.text;
            string password = passwordField.text;

            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM users WHERE username=@username AND password=@password;";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentUser = username;
                            // Успешный вход
                            errorText.text = "";
                            Debug.Log("Login successful");
                            reader.Close();

                            A = true;
                        }
                        else
                        {
                            // Ошибка входа
                            errorText.text = "Invalid username or password";
                            Debug.Log("Login failed");
                        }
                    }
                }

                connection.Close();
                if (A)
                {
                    SceneManager.LoadScene(1);
                }
            }
        }
    }

    public void Register()
    {
        bool A = false;
        if (Validation())
        {
            string username = usernameField.text;
            string password = passwordField.text;
            DateTime currentDate = DateTime.Now; // Текущая дата и время

            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    // Проверка на существование пользователя
                    command.CommandText = "SELECT COUNT(*) FROM users WHERE username=@username;";
                    command.Parameters.AddWithValue("@username", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        errorText.text = "User already exists";
                    }
                    else
                    {
                        // Добавление пользователя в таблицу users
                        currentUser = username;
                        command.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password);";
                        command.Parameters.AddWithValue("@password", password);
                        command.ExecuteNonQuery();

                        // Заполнение таблицы userVisiting с текущей датой
                        command.CommandText = "INSERT INTO userVisiting (username, account_creation_date, last_visit_date) VALUES (@username, @currentDate, @currentDate);";
                        command.Parameters.AddWithValue("@currentDate", currentDate);
                        command.ExecuteNonQuery();

                        // Заполнение таблицы userStats с нулевыми значениями
                        command.CommandText = "INSERT INTO userStats (username, gas, genDur, lampCount, heat, oxygen) VALUES (@username, 0, 0, 0, 0, 0);";
                        command.ExecuteNonQuery();

                        // Заполнение таблицы userExtraStats с нулевыми значениями
                        command.CommandText = "INSERT INTO userExtraStats (username, deaths, timePassed) VALUES (@username, 0, 0);";
                        command.ExecuteNonQuery();

                        Debug.Log("Registration successful");
                        Debug.Log("Initial values added for user");
                        A = true;
                    }
                }
                connection.Close();
                if (A)
                {
                    SceneManager.LoadScene(1);
                }
            }
        }
    }


    public void SaveProgress()
    {
        Debug.Log(currentUser);
        if (!string.IsNullOrEmpty(currentUser))
        {
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {


                    Debug.Log("Progress saved");
                }

                connection.Close();
            }
        }
        else
        {
            Debug.LogError("No user logged in");
        }
    }
}
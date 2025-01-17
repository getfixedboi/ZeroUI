using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using System;
using UnityEngine.UIElements;


public class DeleteUser : MonoBehaviour
{
    [SerializeField] private TMP_InputField idField;
    [SerializeField] private Text errorText;
    //field
    [SerializeField] private TMP_InputField gasField;
    [SerializeField] private TMP_InputField durField;
    [SerializeField] private TMP_InputField lampField;
    [SerializeField] private TMP_InputField heatField;
    [SerializeField] private TMP_InputField oxygenField;
    [SerializeField] private TMP_InputField deathsField;
    [SerializeField] private TMP_InputField timePassedField;
    [SerializeField] private Transform scrollView;
    [SerializeField] private GameObject userItemPrefab;

    public void DeleteUserPoId()
    {
        string username = idField.text;

        using (var connection = new SqliteConnection(Reg.dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Проверяем, существует ли пользователь с данным именем
                command.CommandText = "SELECT COUNT(*) FROM users WHERE username = @username;";
                command.Parameters.AddWithValue("@username", username);
                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count > 0)
                {
                    // Удаляем записи из зависимых таблиц
                    command.CommandText = "DELETE FROM userExtraStats WHERE username = @username;";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM userStats WHERE username = @username;";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM userVisiting WHERE username = @username;";
                    command.ExecuteNonQuery();

                    // Удаляем пользователя из таблицы users
                    command.CommandText = "DELETE FROM users WHERE username = @username;";
                    command.ExecuteNonQuery();

                    errorText.text = $"Пользователь с именем {username} был удален.";
                }
                else
                {
                    errorText.text = "Пользователь не найден.";
                }
            }

            connection.Close();
        }
    }

    public void LoadUserData()
    {
        string username = idField.text;

        using (var connection = new SqliteConnection(Reg.dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Проверяем, существует ли пользователь с данным именем
                command.CommandText = "SELECT * FROM userStats WHERE username = @username;";
                command.Parameters.AddWithValue("@username", username);

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Загружаем данные пользователя из userStats
                        gasField.text = reader["gas"].ToString();
                        durField.text = reader["genDur"].ToString();
                        lampField.text = reader["lampCount"].ToString();
                        heatField.text = reader["heat"].ToString();
                        oxygenField.text = reader["oxygen"].ToString();
                    }
                    else
                    {
                        errorText.text = "Пользователь не найден.";
                        return; // Выходим, если пользователь не найден
                    }
                }

                // Теперь загружаем дополнительные данные из userExtraStats
                command.CommandText = "SELECT * FROM userExtraStats WHERE username = @username;";
                using (IDataReader readerExtra = command.ExecuteReader())
                {
                    if (readerExtra.Read())
                    {
                        // Загружаем данные пользователя из userExtraStats
                        deathsField.text = readerExtra["deaths"].ToString();
                        timePassedField.text = readerExtra["timePassed"].ToString();
                    }
                    else
                    {
                        errorText.text = "Дополнительные данные пользователя не найдены.";
                    }
                }
            }

            connection.Close();
        }
    }

    public void SaveUserData()
    {
        string username = idField.text;

        using (var connection = new SqliteConnection(Reg.dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Проверяем, существует ли пользователь с данным именем в userStats
                command.CommandText = "SELECT COUNT(*) FROM userStats WHERE username = @username;";
                command.Parameters.AddWithValue("@username", username);
                int countStats = Convert.ToInt32(command.ExecuteScalar());

                if (countStats > 0)
                {
                    // Обновляем данные пользователя в userStats
                    command.CommandText = "UPDATE userStats SET gas = @gas, genDur = @genDur, lampCount = @lampCount, heat = @heat, oxygen = @oxygen WHERE username = @username;";

                    // Получаем значения из полей, устанавливаем 0 для пустых
                    command.Parameters.Clear(); // Очищаем параметры перед повторным использованием
                    command.Parameters.AddWithValue("@gas", string.IsNullOrEmpty(gasField.text) ? 0 : float.Parse(gasField.text));
                    command.Parameters.AddWithValue("@genDur", string.IsNullOrEmpty(durField.text) ? 0 : float.Parse(durField.text));
                    command.Parameters.AddWithValue("@lampCount", string.IsNullOrEmpty(lampField.text) ? 0 : int.Parse(lampField.text));
                    command.Parameters.AddWithValue("@heat", string.IsNullOrEmpty(heatField.text) ? 0 : float.Parse(heatField.text));
                    command.Parameters.AddWithValue("@oxygen", string.IsNullOrEmpty(oxygenField.text) ? 0 : float.Parse(oxygenField.text));
                    command.Parameters.AddWithValue("@username", username);

                    command.ExecuteNonQuery();

                    // Проверяем, существует ли пользователь с данным именем в userExtraStats
                    command.CommandText = "SELECT COUNT(*) FROM userExtraStats WHERE username = @username;";
                    int countExtraStats = Convert.ToInt32(command.ExecuteScalar());

                    if (countExtraStats > 0)
                    {
                        // Обновляем данные пользователя в userExtraStats
                        command.CommandText = "UPDATE userExtraStats SET deaths = @deaths, timePassed = @timePassed WHERE username = @username;";

                        // Получаем значения из полей, устанавливаем 0 для пустых
                        command.Parameters.Clear(); // Очищаем параметры перед повторным использованием
                        command.Parameters.AddWithValue("@deaths", string.IsNullOrEmpty(deathsField.text) ? 0 : int.Parse(deathsField.text));
                        command.Parameters.AddWithValue("@timePassed", string.IsNullOrEmpty(timePassedField.text) ? 0 : float.Parse(timePassedField.text));
                        command.Parameters.AddWithValue("@username", username);

                        command.ExecuteNonQuery();

                        errorText.text = $"Данные пользователя {username} обновлены.";
                    }
                    else
                    {
                        errorText.text = "Дополнительные данные пользователя не найдены.";
                    }
                }
                else
                {
                    errorText.text = "Пользователь не найден.";
                }
            }

            connection.Close();
        }
    }

    public void ShowUsers()
    {
        foreach (Transform child in scrollView)
        {
            Destroy(child.gameObject);
        }

        using (var connection = new SqliteConnection(Reg.dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Запрос для получения всех ID пользователей
                command.CommandText = "SELECT username FROM users;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string username = reader["username"].ToString();
                        // Создаем новый элемент списка
                        GameObject userItem = Instantiate(userItemPrefab, scrollView);
                        userItem.GetComponentInChildren<Text>().text = username; // Устанавливаем имя пользователя
                    }
                }
            }

            connection.Close();
        }
    }
}



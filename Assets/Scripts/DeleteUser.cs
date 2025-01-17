using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using System;


public class DeleteUser : MonoBehaviour
{
    [SerializeField] private TMP_InputField idField;
    [SerializeField] private Text errorText;

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

}


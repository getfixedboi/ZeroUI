using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;
using System.Xml;

public class Admin : MonoBehaviour
{
    [Header("Reg form components")]
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    public void GoToAdminPanel()
    {
        if (usernameField.text != "admin" || passwordField.text != "admin") { return; }

        SceneManager.LoadScene(2);
    }
}

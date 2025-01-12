using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;
using System.Xml;

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

    private string xmlFilePath;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            xmlFilePath = Path.Combine(Application.persistentDataPath, "config.xml");
            LoadValuesFromXML();
        }
    }

    public void GoBack()
    {
        GasMul = float.Parse(GasMulField.text);
        DurMul = float.Parse(DurMulField.text);
        LampMul = float.Parse(LampMulField.text);
        TempMul = float.Parse(TempMulField.text);

        SaveValuesToXML();
        SceneManager.LoadScene(0);
    }

    private void LoadValuesFromXML()
    {
        if (File.Exists(xmlFilePath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            XmlNodeList xml = xmlDoc.GetElementsByTagName("Config");

            foreach (XmlNode itemNode in xml)
            {
                GasMul = float.Parse(itemNode["GasMul"]?.InnerText.Replace('.', ','));
                DurMul = float.Parse(itemNode["DurMul"]?.InnerText.Replace('.', ','));
                LampMul = float.Parse(itemNode["LampMul"]?.InnerText.Replace('.', ','));
                TempMul = float.Parse(itemNode["TempMul"]?.InnerText.Replace('.', ','));
            }

            GasMulField.text = GasMul.ToString();
            DurMulField.text = DurMul.ToString();
            LampMulField.text = LampMul.ToString();
            TempMulField.text = TempMul.ToString();
        }
        else
        {
            GasMul = 1;
            DurMul = 1;
            LampMul = 1;
            TempMul = 1;

            GasMulField.text = "1";
            DurMulField.text = "1";
            LampMulField.text = "1";
            TempMulField.text = "1";

            SaveValuesToXML();
        }
    }

    public void LoadValuesFromXML_Outscene()
    {
        if (File.Exists(xmlFilePath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            XmlNodeList xml = xmlDoc.GetElementsByTagName("Config");

            foreach (XmlNode itemNode in xml)
            {
                GasMul = float.Parse(itemNode["GasMul"]?.InnerText.Replace('.', ','));
                DurMul = float.Parse(itemNode["DurMul"]?.InnerText.Replace('.', ','));
                LampMul = float.Parse(itemNode["LampMul"]?.InnerText.Replace('.', ','));
                TempMul = float.Parse(itemNode["TempMul"]?.InnerText.Replace('.', ','));
            }
        }
    }
    private void SaveValuesToXML()
    {
        XmlDocument xmlDoc = new XmlDocument();

        XmlElement root = xmlDoc.CreateElement("Config");
        xmlDoc.AppendChild(root);

        XmlElement gasMulElement = xmlDoc.CreateElement("GasMul");
        gasMulElement.InnerText = GasMul.ToString();
        root.AppendChild(gasMulElement);

        XmlElement durMulElement = xmlDoc.CreateElement("DurMul");
        durMulElement.InnerText = DurMul.ToString();
        root.AppendChild(durMulElement);

        XmlElement lampMulElement = xmlDoc.CreateElement("LampMul");
        lampMulElement.InnerText = LampMul.ToString();
        root.AppendChild(lampMulElement);

        XmlElement oxygenMulElement = xmlDoc.CreateElement("TempMul");
        oxygenMulElement.InnerText = TempMul.ToString();
        root.AppendChild(oxygenMulElement);

        xmlDoc.Save(xmlFilePath);
    }

}

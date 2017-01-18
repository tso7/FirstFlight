using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;

public class Config : GenericSingleton<Config> {

    public static string filename = "config.ini";
    private static int devserial;
    private static float sensitivity;

    public static int GetDevSerial () {
        return devserial;
    }

    public static float GetSensitivity () {
        return sensitivity;
    }

    void Awake () {
        loadFile(filename);
        // Other stuff 
    }

    void loadFile (string filename) {
        if (!File.Exists(filename)) {
            File.CreateText(filename);
            devserial = Microphone.devices.Length - 1;
            return;
        }

        try {
            string line;
            StreamReader sReader = new StreamReader(filename, Encoding.Default);
            do {
                line = sReader.ReadLine();
                if (line != null) {
                    // Lines with # are for comments
                    if (!line.Contains("#")) {
                        // Value property identified by string before the colon.
                        string[] data = line.Split(':');
                        if (data.Length == 2) {
                            switch (data[0]) {
                                case "Device Number":
                                    Debug.LogWarning("Device Number: " + data[1]);
                                    devserial = Convert.ToInt32(data[1].Trim());
                                    break;
                                case "Sensitivity":
                                    Debug.LogWarning("Sensitivity: " + data[1]);
                                    sensitivity = float.Parse(data[1].Trim());
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            while (line != null);
            sReader.Close();
            return;
        }
        catch (Exception e) {
        }
    }
}

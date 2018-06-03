using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceChanger : MonoBehaviour {

    public GameObject mouseSentivityInputText;
    int defaultMouseSensitivity = 12;


	// Use this for initialization
	void Start () {
        if (!PlayerPrefs.HasKey("Mouse Sensitivity")) {
            PlayerPrefs.SetInt("Mouse Sensitivity", defaultMouseSensitivity);
        }

        mouseSentivityInputText.GetComponent<UnityEngine.UI.Text>().text = "" + PlayerPrefs.GetInt("Mouse Sensitivity");
	}
    
    public void UpdateMouseSensitivity() {
        int mouseSensitivity;
        try {
            mouseSensitivity = int.Parse(mouseSentivityInputText.GetComponent<UnityEngine.UI.Text>().text);
        } catch (System.Exception e) {
            mouseSensitivity = defaultMouseSensitivity;
        }
        PlayerPrefs.SetInt("Mouse Sensitivity", mouseSensitivity);
        PlayerPrefs.Save();
    }
}

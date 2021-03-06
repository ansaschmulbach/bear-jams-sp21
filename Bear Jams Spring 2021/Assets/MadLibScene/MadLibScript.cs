using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MadLibScript : MonoBehaviour
{
    private String currentHint;
    
    #region Inspector Variables
    [SerializeField]
    private TextMeshProUGUI hintText;

    [SerializeField]
    private TMP_InputField inputField;
    #endregion
    
    List<String> GetMadLib(int numPuppets)
    {
        // I'm so sorry.
        List<List<String>> selectedScripts = Script.scripts[numPuppets - 1];
        List<String> randomScript = selectedScripts[Random.Range(0, selectedScripts.Count)];

        return randomScript;
    }

    #region Current Hint Methods

    String GetCurrentHint()
    {
        GameManager gm = GameManager.instance;

        // Assumes well-formatted strings, UB otherwise.
        foreach (String line in gm.gameState.script)
        {
            int idx = line.IndexOf("[");
            if (idx != -1)
            {
                int idx_end = line.IndexOf("]");
                currentHint = line.Substring(idx, idx_end-idx+1);
                return currentHint;
            }
        }

        currentHint = "";
        return "";
    }

    void SetCurrentHint(String replacement)
    {
        GameManager gm = GameManager.instance;
        
        // Assumes well-formatted strings, UB otherwise.
        var regex = new Regex(Regex.Escape(currentHint));
        for (int i = 0; i < gm.gameState.script.Count; i++)
        {
            gm.gameState.script[i] = regex.Replace(gm.gameState.script[i], replacement);
        }
    }

    #endregion

    #region UI Methods

    public void OnClickNext()
    {
        String replacement = inputField.text;
        SetCurrentHint(replacement);
        GetCurrentHint();
        if (currentHint == "")
        {
            GameManager gm = GameManager.instance;
            gm.LoadFinal();
        }
        else
        {
            hintText.text = currentHint;
            inputField.text = "";
        }
    }

    #endregion

    #region Instantiation Methods

    void Start()
    {
        GameManager gm = GameManager.instance;
        gm.gameState.script = GetMadLib(gm.gameState.numberOfSocks);
        
        hintText.text = GetCurrentHint();
    }

    void Update()
    {
        if (Input.GetKeyDown("enter"))
        {
            OnClickNext();
        }
    }

    #endregion
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using System.Linq;


public class CodeAnalyzer : MonoBehaviour
{
    private int lastLength = 0;
    public TMP_InputField inputField;
    public float defaultBlinkRate = 1.5f;

    private void Start()
    {
        defaultBlinkRate = inputField.caretBlinkRate;
    }

    public string AddIndent(string text)
    {
        // text = text.Replace("\t", "    "); only pc
        // TODO: formating current editing line, not whole text
        if (lastLength < text.Length)
        {
            var lines = text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                return text;
            }
            
            var lastLine = lines[^1];
            if (text[^1] == '\n')
            {
                lastLine += "\n";
            }

            if (lastLine.Length > 2 && lastLine[^1] == '\n' && lastLine[^2] == ':')
            {
                int countSpaces = lastLine.Count(c => c == ' ');
                text += new string(' ', (countSpaces + 4));
                inputField.text = text;
                inputField.caretBlinkRate = 100f;
                StartCoroutine(SetCaretToEndNextFrame(text.Length));
            }
            else if (lastLine.Length > 1 && lastLine[^1] == '\n')
            {
                int countSpaces = lastLine.Count(c => c == ' ');
                text += new string(' ', (countSpaces));
                inputField.text = text;
                inputField.caretBlinkRate = 100f;
                StartCoroutine(SetCaretToEndNextFrame(text.Length));
            }
        }
        lastLength = text.Length;
       
        return text;
    }
    private IEnumerator SetCaretToEndNextFrame(int position = 0)
    {
        yield return new WaitForSeconds(0.01f);
        inputField.caretPosition = position;
        yield return new WaitForSeconds(0.01f);
        inputField.caretBlinkRate = defaultBlinkRate;
    }
    
    public string AnalyzeCode(string code)
    {
        // TODO: brackets check
        string result = "";
        
        return result;
    }
}

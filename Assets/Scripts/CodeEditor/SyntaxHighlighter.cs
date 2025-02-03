using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public class KeywordMapping
{
    public string keyword;
    public Color color;
}


public class SyntaxHighlighter : MonoBehaviour
{
    [Header("Base Settings")]
    [Tooltip("Size of font")] public int fontSize = 18;
    [Tooltip("Color of non keywords code")]public Color defaultTextColor = Color.white;
    
    [Header("Fields for correct work")]
    public TMP_InputField codeInputField;
    public TextMeshProUGUI displayedText;
    [Tooltip("This <fake> input field will be used to synchronize changes, its unseen for user")] 
    public TextMeshProUGUI inputText;
    

    public List<KeywordMapping> colorMappings = new List<KeywordMapping>()
    {
        new KeywordMapping { keyword = "class", color = new Color(0.305f, 0.788f, 0.690f)},
        new KeywordMapping { keyword = "def", color = new Color(0.305f, 0.788f, 0.690f) },
        new KeywordMapping { keyword = "if", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "else", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "elif", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "for", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "while", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "return", color = new Color(0.863f, 0.424f, 0.349f),},
        new KeywordMapping { keyword = "import", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "from", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "as", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "try", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "except", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "finally", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "raise", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "with", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "lambda", color = new Color(0.305f, 0.788f, 0.690f) },
        new KeywordMapping { keyword = "True", color = new Color(0.773f, 0.525f, 0.753f)},
        new KeywordMapping { keyword = "False", color = new Color(0.773f, 0.525f, 0.753f)},
        new KeywordMapping { keyword = "None", color = new Color(0.773f, 0.525f, 0.753f)},
        new KeywordMapping { keyword = "and", color = new Color(0.863f, 0.424f, 0.349f) },
        new KeywordMapping { keyword = "or", color = new Color(0.863f, 0.424f, 0.349f) },
        new KeywordMapping { keyword = "not", color = new Color(0.863f, 0.424f, 0.349f) },
        new KeywordMapping { keyword = "is", color = new Color(0.863f, 0.424f, 0.349f) },
        new KeywordMapping { keyword = "in", color = new Color(0.863f, 0.424f, 0.349f) },
        new KeywordMapping { keyword = "global", color = new Color(0.305f, 0.788f, 0.690f) },
        new KeywordMapping { keyword = "nonlocal", color = new Color(0.305f, 0.788f, 0.690f) },
        new KeywordMapping { keyword = "assert", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "pass", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "break", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "continue", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "yield", color = new Color(0.305f, 0.788f, 0.690f)},
        new KeywordMapping { keyword = "del", color = new Color(0.800f, 0.471f, 0.196f) },
    };
    
    public MissionHandler missionHandler = null;
    private void Start()
    {
        StartCoroutine(SetStartProgram());
    }

    private void Update()
    {
        if (displayedText && codeInputField.textComponent)
        {
            displayedText.rectTransform.anchoredPosition = 
                new Vector2(inputText.rectTransform.anchoredPosition.x,
                    inputText.rectTransform.anchoredPosition.y);
        }
    }


    private IEnumerator SetStartProgram()
    {
        while (missionHandler.isReady != 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        codeInputField.text = missionHandler.Server.settings.program;
        codeInputField.onValueChanged.AddListener(HighlightSyntax);
        HighlightSyntax(codeInputField.text);
        
        inputText.color = new Color(0, 0, 0, 0); // setting fake input as transparent
        inputText.fontSize = fontSize;
        inputText.font = displayedText.font;
    }
    
    void HighlightSyntax(string input)
    {
        displayedText.text = "";
        displayedText.text = ApplySyntaxHighlighting(input);
    }


    private string ApplySyntaxHighlighting(string text)
    {
        displayedText.fontSize = fontSize;
        string formattedText = $"<color=#{ColorUtility.ToHtmlStringRGB(defaultTextColor)}>{text}</color>";
        foreach (var mapping in colorMappings)
        {
            string pattern = $@"\b{Regex.Escape(mapping.keyword)}\b";
            
            formattedText = Regex.Replace(formattedText, pattern, 
                $"<color=#{ColorUtility.ToHtmlStringRGB(mapping.color)}>{mapping.keyword}</color>");
        }

        return formattedText;
    }


}
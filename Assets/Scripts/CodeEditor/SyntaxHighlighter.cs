using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public enum SyntaxStyle
{
  None, Italic, Bold, Underline
}

[Serializable]
public class KeywordMapping
{
    public string keyword;
    public Color color;
    public SyntaxStyle style = SyntaxStyle.None;
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
    
    private CodeAnalyzer _codeAnalyzer = null;

    public List<KeywordMapping> colorMappings = new List<KeywordMapping>()
    {
        new KeywordMapping { keyword = "class", color = new Color(0.305f, 0.788f, 0.690f), style = SyntaxStyle.Bold },
        new KeywordMapping { keyword = "def", color = new Color(0.305f, 0.788f, 0.690f), style = SyntaxStyle.Bold },
        new KeywordMapping { keyword = "if", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "else", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "elif", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "for", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "while", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "return", color = new Color(0.863f, 0.424f, 0.349f), style = SyntaxStyle.Bold },
        new KeywordMapping { keyword = "import", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "from", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "as", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "try", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "except", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "finally", color = new Color(0.000f, 0.478f, 0.800f) },
        new KeywordMapping { keyword = "raise", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "with", color = new Color(0.800f, 0.471f, 0.196f) },
        new KeywordMapping { keyword = "lambda", color = new Color(0.305f, 0.788f, 0.690f) },
        new KeywordMapping { keyword = "True", color = new Color(0.773f, 0.525f, 0.753f), style = SyntaxStyle.Bold },
        new KeywordMapping { keyword = "False", color = new Color(0.773f, 0.525f, 0.753f), style = SyntaxStyle.Bold },
        new KeywordMapping { keyword = "None", color = new Color(0.773f, 0.525f, 0.753f), style = SyntaxStyle.Bold },
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
        new KeywordMapping { keyword = "yield", color = new Color(0.305f, 0.788f, 0.690f), style = SyntaxStyle.Bold },
        new KeywordMapping { keyword = "del", color = new Color(0.800f, 0.471f, 0.196f) },
    };

    void Start()
    {
        _codeAnalyzer = gameObject.AddComponent<CodeAnalyzer>();
        _codeAnalyzer.inputField = codeInputField;
        
        codeInputField.onValueChanged.AddListener(AnalyzeCode);
        
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

    void AnalyzeCode(string input)
    {
        displayedText.text = "";
        HighlightSyntax(input); // TODO: also add indient
    }
    
    
    string ApplySyntaxHighlighting(string input)
    {
        displayedText.fontSize = fontSize;
        string formattedText = $"<color=#{ColorUtility.ToHtmlStringRGB(defaultTextColor)}>{input}</color>";
        foreach (var mapping in colorMappings)
        {
            var style = GetSyntaxStyleTags(mapping.style);
            string pattern = $@"\b{Regex.Escape(mapping.keyword)}\b";
            
            formattedText = Regex.Replace(formattedText, pattern, 
                $"{style.Item1}<color=#{ColorUtility.ToHtmlStringRGB(mapping.color)}>{mapping.keyword}</color>{style.Item2}");
        }

        return formattedText;
    }

    private Tuple<string, string> GetSyntaxStyleTags(SyntaxStyle style)
    {
        string leftTags = "", rightTags = "";
        if (style == SyntaxStyle.Bold)
        {
            leftTags = "<b>";
            rightTags = "</b>";
        } else if (style == SyntaxStyle.Italic)
        {
            leftTags = "<i>";
            rightTags = "</i>";
        } else if (style == SyntaxStyle.Underline)
        {
            leftTags = "<u>";
            rightTags = "</u>";
        }
        return new Tuple<string, string>(leftTags, rightTags);
    }
}
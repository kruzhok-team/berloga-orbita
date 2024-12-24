using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace XML
{
    /// <summary>
    /// Uses for inserting code into resulting xml file
    /// </summary>
    public class XHandlerInsertCode : MonoBehaviour, IXHandler
    {
        private XModule _module;
        
        public TextMeshProUGUI codeText;
        public string xpath = "//python_code";
        
        
        public void CallBack()
        {
            _module = FindFirstObjectByType<GameManager>().GetXModule();
            if (_module == null)
            {
                Debug.LogError("No module for object found");
                return;
            }
            //string cleaned = Regex.Replace(codeText.text, @"[^a-zA-Zа-яА-Я0-9_+\-*/%=<>(){}\[\] :.,]", "");
            //string code = "<![CDATA[\n" + codeText.text + "\n]]>";
            _module.InsertInnerRawValue( xpath, codeText.text);
        }
    }
}


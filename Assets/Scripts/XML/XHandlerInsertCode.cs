using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using System;
using System.Collections;
using Menu;
using UnityEngine.UI;

namespace XML
{
    /// <summary>
    /// Uses for inserting code into resulting xml file
    /// </summary>
    public class XHandlerInsertCode : MonoBehaviour, IXHandler
    {
        private XModule _module;
        
        public TMP_InputField codeText;
        public string xpath = "//python_code";
        
        private MissionHandler _missionHandler;

        public void Start()
        {
            _missionHandler = FindFirstObjectByType<MissionHandler>();
            //StartCoroutine(PasteSample());
        }

        public IEnumerator PasteSample()
        {
            while (_missionHandler.isReady != 0)
            {
                yield return new WaitForSeconds(0.1f);
            }
            // codeText.SetText(_missionHandler.Server.settings.program);
        }
        
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
            _module.InsertInnerRawValue(xpath, "\n" + codeText.text);
            //Debug.Log(codeText.text);
        }
    }
}


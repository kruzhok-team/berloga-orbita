using System;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

namespace XML
{
    /// <summary>
    /// This class for input type of any param, where we need to change value of param
    /// </summary>
    public class XHandlerCreateNew : MonoBehaviour, IXHandler
    {
        [SerializeField] private string xpath;
        [SerializeField] private string elementName;
        [SerializeField] public List<Pair> attributes;

        public XModule module;
        public string GetValue()
        {
            return "no value";
        }
        
        void Start()
        {
            module = GameObject.FindWithTag("GameManager")?.GetComponent<GameManager>()?.GetXModule();
            if (module == null)
            {
                Debug.LogError("XMLHandler: Can't find GameManager");
            }
        }

        public void CallBack()
        {
            var elem = module.CreateNewXmlElement(elementName, attributes);
            module.InsertElement(xpath, elem);
        }

        public string GetXPath()
        {
            return xpath;
        }
    }
}

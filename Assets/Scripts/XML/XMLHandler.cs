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
    public class XMLHandler : MonoBehaviour, XHandler
    {
        [SerializeField] private TextMeshProUGUI textObject;
        [SerializeField] private string xpath;
        [SerializeField] private string _fieldName;
        [SerializeField] public List<Pair> _params;

        public XMLModule module; // who will init??
        public string GetValue()
        {
            return textObject.text;
        }

        // Update is called once per frame
        void Start()
        {
            if (textObject == null)
            {
                Debug.LogError("XMLHandler: No text object attached");
            }
        }

        XmlElement GetXmlElement()
        {
            return module.CreateElement(name, _params);;
        }
    }
}

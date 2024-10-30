using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace XML
{
    [System.Serializable]
    public class ParametrizedPair
    {
        public string key;
        public TextMeshProUGUI valueRef;
    }
    /// <summary>
    /// TODO: write here smth
    /// </summary>
    public class XHandlerPasteAttribute : MonoBehaviour, IXHandler
    {
        [SerializeField] private bool needCreateNewElement = false; // possibly can mark by server response
        
        [SerializeField] public string xpath;
        [SerializeField] private string elementName = "null"; // uses only if creating new
        
        // inner value if needed
        
        [SerializeField] public List<Pair> defaultAttributes;
        [SerializeField] public List<ParametrizedPair> attributes;

        private XModule _module;
        void Start()
        {
            
            _module = FindFirstObjectByType<GameManager>().GetXModule();
           
            if (_module == null)
            {
                Debug.LogError("XMLHandler: Can't find GameManager");
            }
        }

        private void CreateNewElement(List<Pair> pairs)
        {
            var elem = _module.CreateNewXmlElement(elementName, pairs);
            _module.InsertElement(xpath, elem);
        }
        
        public void CallBack()
        {
            if (_module == null) return;
            
            List<Pair> allAttributes = new List<Pair>(defaultAttributes);
            
            foreach (var attribute in attributes)
            {
                string cleaned = Regex.Replace(attribute.valueRef.text, @"[^a-zA-Zа-яА-Я0-9]", "");
                allAttributes.Add(new Pair() { key = attribute.key, value = cleaned});
            }

            if (needCreateNewElement)
            {
                CreateNewElement(allAttributes);
            }
            else
            {
                _module.PasteNewAttributes(xpath, allAttributes);
            }
           
        }

        public string GetXPath()
        {
            return xpath;
        }
    }
}


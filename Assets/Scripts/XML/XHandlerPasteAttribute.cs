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
    public class XHandlerPasteAttribute : MonoBehaviour, IXHandler
    {
        private XModule _module;
        
        [SerializeField] private string xpath;
        [SerializeField] public List<Pair> defaultAttributes;
        [SerializeField] public List<ParametrizedPair> attributes;

    
        void Start()
        {
            _module = GameObject.FindWithTag("GameManager")?.GetComponent<GameManager>()?.GetXModule();
           
            if (_module == null)
            {
                Debug.LogError("XMLHandler: Can't find GameManager");
            }
        }
        
        public void CallBack()
        {
            if (_module == null) return;
            
            foreach (var attribute in defaultAttributes)
            {
                _module.PasteNewAttribute(xpath, attribute.key, attribute.value);
            }

            foreach (var attribute in attributes)
            {
                string cleaned = Regex.Replace(attribute.valueRef.text, @"[^a-zA-Zа-яА-Я0-9]", "");
                _module.PasteNewAttribute(xpath, attribute.key, cleaned);
            }
           
        }

        public string GetXPath()
        {
            return xpath;
        }
    }
}


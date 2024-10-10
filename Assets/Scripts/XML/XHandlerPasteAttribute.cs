using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace XML
{
    public class XHandlerPasteAttribute : MonoBehaviour, IXHandler
    {
        private XModule _module;
    
        [SerializeField] private TextMeshProUGUI textObject;
        
        [SerializeField] private string xpath;
        [SerializeField] public List<Pair> attributes;

    
        void Start()
        {
            if (textObject == null)
            {
                Debug.LogError("XMLHandler: No text object attached");
            }
            _module = GameObject.FindWithTag("GameManager")?.GetComponent<GameManager>()?.GetXModule();
            if (_module == null)
            {
                Debug.LogError("XMLHandler: Can't find GameManager");
            }
        }

    
        public string GetValue()
        {
            return textObject.text;
        }

        public void CallBack()
        {
            if (_module == null) return;
            
            foreach (var attribute in attributes)
            {
                _module.PasteNewAttribute(xpath, attribute.key, attribute.value);
            }
           
        }

        public string GetXPath()
        {
            return xpath;
        }
    }
}


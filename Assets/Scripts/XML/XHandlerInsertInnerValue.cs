using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace XML
{
    public class XHandlerInsertInnerValue : MonoBehaviour, IXHandler
    {
        private XModule _module;
    
        [SerializeField] private TextMeshProUGUI textObject;
        [SerializeField] private string xpath;
        [SerializeField] private string elementName;
        [SerializeField] public List<Pair> attributes;

    
        private void Start()
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
            throw new System.NotImplementedException();
        }

        public string GetXPath()
        {
            return xpath;
        }
    }
}


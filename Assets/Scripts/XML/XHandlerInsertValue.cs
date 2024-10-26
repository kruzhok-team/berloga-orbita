using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace XML
{
    [Serializable]
    public class PlaceToInsert
    {
        public string xpath;
        public TextMeshProUGUI valueRef;
    }
    /// <summary>
    /// This class for using where need paste values in existing params as inner value
    /// Made specially for ballistic calculator
    /// </summary>
    public class XHandlerInsertValue : MonoBehaviour, IXHandler
    {
        private XModule _module;
        public Button calculateBtn;
        
        [SerializeField] public List<PlaceToInsert> attributes;
        
        private void Start()
        {
            var manager = FindFirstObjectByType<GameManager>();
            _module = manager.GetXModuleBallistics();
            if (_module == null)
            {
                Debug.LogError("XMLHandler: Can't find GameManager");
            }
            calculateBtn.onClick.AddListener(() => manager.BallisticCalculatorBtnPushDown());
        }
        
        public void CallBack()
        {
            if (_module == null) return;
            List<Pair> insertingValues = new List<Pair>();
            
            foreach (var attribute in attributes)
            {
                string cleaned = Regex.Replace(attribute.valueRef.text, @"[^a-zA-Zа-яА-Я0-9]", "");

                if (attribute.xpath == "//square") // TODO: should put this logic into text getter
                {
                    // TODO: here can be no value so inner logic should check this and not allow do callback
                    cleaned = (Convert.ToDouble(cleaned) * Convert.ToDouble(cleaned) * Math.PI).ToString();
                }
                insertingValues.Add(new Pair() { key = attribute.xpath, value = cleaned});
            }
            _module.InsertInnerValues(insertingValues);
        }
    }
}


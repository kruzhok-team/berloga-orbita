using System.Collections.Generic;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnitCreation
{
    public class MaxValuesChangedTrigger : MonoBehaviour
    {
        public BaseParameterSetup setup;
    
        public List<TMP_InputField> fields;
        public Button button;

        private void Start()
        {
            foreach (TMP_InputField field in fields)
            {
            //    field.onValueChanged.AddListener(Trigger);
            }
            button.onClick.AddListener(() => Trigger(null));
        }
    

        private void Trigger(string s)
        {
            setup.OnMaxValueChanged();
        }
    }
}

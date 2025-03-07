using TMPro;
using UnityEngine;

namespace UnitCreation
{
    public class DeviceInfoHolder : MonoBehaviour
    {
        public TextMeshProUGUI parameter;
        public TextMeshProUGUI value;

        public void Initialize(string param, string val)
        {
            parameter.SetText(param);
            value.SetText(val);
        }
        
    }
}

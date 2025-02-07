using System.Collections.Generic;
using Connections;
using TMPro;
using UnitCreation;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public enum RadiusType
    {
        None, 
        Iternal,
        Both
    }
    public class BaseParameterSetup : MonoBehaviour
    {
        public RadiusType radiusType = RadiusType.None;
        public StatsCounter statsCounter;
        public TextMeshProUGUI startHeight;
        
        public GameObject internalRadiusObject;
        public TMP_InputField internalRadius;

        public GameObject externalRadiusObject;
        public TMP_InputField externalRadius;
        
        public float maxMass = 20000f;
        public float maxExternalRadius = 2.0f;
        public float startHeightValue = 0.0f;
        
        private List<Device> _devices = new List<Device>();

        public void RadiusSettingsSetup(string param)
        {
            if (param == "none ")
            {
                internalRadiusObject.SetActive(false);
                externalRadiusObject.SetActive(false);
                radiusType = RadiusType.None;
            }
            else if (param == "both")
            {
                radiusType = RadiusType.Both;
            }
            else if (param == "internal")
            {
                radiusType = RadiusType.Iternal;
                externalRadiusObject.SetActive(false);
            }
        }
        
        public void HeightSettingsSetup(List<double> heights)
        {
            startHeightValue = Random.Range((float)heights[0], (float)heights[1]);
            startHeight.text = startHeightValue.ToString();
        }

        protected double GetMaxVolume()
        {
            return (4f / 3f) * Mathf.PI * Mathf.Pow(maxExternalRadius, 3);
        }

        protected double GetMaxMass()
        {
            return maxMass;
        }

        public void OnValueChanged()
        {
            statsCounter.UpdateValues(GetMaxMass(), GetMaxVolume());
        }

        public void OnValueChanged(List<Device> devices)
        {
            _devices = devices;
            statsCounter.UpdateValues(GetMaxMass(), GetMaxVolume());
        }
    }
}

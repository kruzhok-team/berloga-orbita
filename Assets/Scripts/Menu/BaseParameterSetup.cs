using System;
using System.Collections.Generic;
using System.Globalization;
using Connections;
using TMPro;
using UnitCreation;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        //public float startHeightValue = 0.0f;
        
        private List<Device> _devices;

        public void Start()
        {
            OnBasedValueChanged(new List<Device>());
            OnMaxValueChanged();
        }
        
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
            startHeight.text = Math.Round(Random.Range((float)heights[0], (float)heights[1]), 2)
                               .ToString(CultureInfo.InvariantCulture);
        }

        protected virtual double GetMaxVolume()
        {
            double input = maxExternalRadius;
            try
            {
                input = Convert.ToDouble(internalRadius.text);
            }
            catch (FormatException)
            {
                // Input was not valid, but it's okay we will use max
                input = -1;
            }

            if (input > maxExternalRadius || input <= 0)
            {
                internalRadius.text = maxExternalRadius.ToString(CultureInfo.InvariantCulture);
                input = maxExternalRadius;
            }
            
            return ((double) (4f / 3f) * Mathf.PI * Math.Pow
                (Math.Min((double) maxExternalRadius, input) , 3));
        }

        protected virtual double GetMaxMass()
        {
            return maxMass;
        }

        protected virtual double GetCurrentVolume()
        {
            double volume = 0;
            foreach (Device device in _devices)
            {
                volume += device.Volume;
            }
            return volume;
        }
        
        protected virtual double GetCurrentMass()
        {
            double mass = GetMaxVolume() * 100;
            foreach (Device device in _devices)
            {
                mass += device.Mass;
            }
            return mass;
        }
        

        
        public void OnMaxValueChanged()
        {
            statsCounter.UpdateMaximums(GetMaxMass(), GetMaxVolume());
        }

        public void OnBasedValueChanged(List<Device> devices)
        {
            _devices = devices;
            statsCounter.UpdateValues(GetCurrentMass(), GetCurrentVolume());
        }
    }
}

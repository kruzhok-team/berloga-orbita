using System;
using System.Globalization;
using UnityEngine;

namespace Menu
{
    public class AdvancedParameterSetup : BaseParameterSetup
    {
        protected override double GetCurrentVolume()
        {
            return base.GetCurrentVolume();
        }
        protected override double GetCurrentMass() // from doc
        {
            double inputRadius1 = maxExternalRadius;
            double inputRadius2 = maxExternalRadius;
            try
            {
                inputRadius1 = Convert.ToDouble(internalRadius.text);
            }
            catch (FormatException)
            {
                // Input was not valid, but it's okay we will use max
                inputRadius1 = -1;
            }

            try
            {
                inputRadius2 = Convert.ToDouble(externalRadius.text);
            }
            catch (FormatException)
            {
                // Input was not valid, but it's okay we will use max
                inputRadius2 = -1;
            }
            
            if (inputRadius1 > maxExternalRadius || inputRadius1 <= 0)
            {
                internalRadius.text = maxExternalRadius.ToString(CultureInfo.InvariantCulture);
                inputRadius1 = maxExternalRadius;
            }

            if (inputRadius2 < 0)
            {
                internalRadius.text = Math.Max(maxExternalRadius, inputRadius1).ToString(CultureInfo.InvariantCulture);
                inputRadius2 = Math.Max(maxExternalRadius, inputRadius1);
            }

            var sphere1 = (4f / 3f) * Mathf.PI * Math.Pow(Math.Min((double)maxExternalRadius, inputRadius1), 3);
            var sphere2 = (4f / 3f) * Mathf.PI * Math.Pow(Math.Max((double)inputRadius1, inputRadius2), 3);
            
            var mk = sphere1 * 400.0;
            
            var miz = (sphere2 - sphere1) * 4000.0;

            double vInside = 0;
            double mass = 0;
            foreach (var device in _devices)
            {
                vInside += device.Volume;
                mass += device.Mass;
            }

            var mpg = sphere1 - vInside;
            return mk + miz + mpg + mass;
        }
        
        protected override double GetMaxVolume()
        {
            double inputRadius1 = maxExternalRadius;
            double inputRadius2 = maxExternalRadius;
            try
            {
                inputRadius1 = Convert.ToDouble(internalRadius.text);
            }
            catch (FormatException)
            {
                // Input was not valid, but it's okay we will use max
                inputRadius1 = -1;
            }

            try
            {
                inputRadius2 = Convert.ToDouble(externalRadius.text);
            }
            catch (FormatException)
            {
                // Input was not valid, but it's okay we will use max
                inputRadius2 = -1;
            }
            
            if (inputRadius1 > maxExternalRadius || inputRadius1 <= 0)
            {
                internalRadius.text = maxExternalRadius.ToString(CultureInfo.InvariantCulture);
                inputRadius1 = maxExternalRadius;
            }

            if (inputRadius2 < 0)
            {
                internalRadius.text = Math.Max(maxExternalRadius, inputRadius1).ToString(CultureInfo.InvariantCulture);
                inputRadius2 = Math.Max(maxExternalRadius, inputRadius1);
            }
            
            var sphere2 = (4f / 3f) * Mathf.PI * Math.Pow(Math.Max((double)inputRadius1, inputRadius2), 3);

            return sphere2;
        }

        protected override double GetMaxMass()
        {
            return base.GetMaxMass();
        }
    }
}

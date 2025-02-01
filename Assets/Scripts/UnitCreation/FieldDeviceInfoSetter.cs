using System;
using System.Globalization;
using Connections;
using Unity.VisualScripting;
using UnityEngine;

namespace UnitCreation
{
    public class FieldDeviceInfoSetter : MonoBehaviour
    {
        public GameObject parent;
        public GameObject fieldDevice;

        public void Initialize(Device device)
        {
            RemoveAllChildrenExceptFirst(parent);
            parent.transform.GetChild(0).gameObject.SetActive(true);
            if (device == null) return;
            if (device.FullName != null)
            {
                AddNewField("Имя", device.FullName);
            }

            if (device.Mass > 0)
            {
                AddNewField("Масса", Math.Round(device.Mass, 2).ToString(CultureInfo.InvariantCulture));
            }

            if (device.Code != null)
            {
                AddNewField("Код", device.Code);
            }
            // TODO: add all
            
            parent.transform.GetChild(0).gameObject.SetActive(false);
        }

        private void AddNewField(string fieldName, string value)
        {
            GameObject newChild = Instantiate(fieldDevice, parent.transform, true);
            newChild.SetActive(true);
            DeviceInfoHolder holder = newChild.GetComponent<DeviceInfoHolder>();
            holder.Initialize(fieldName, value);
        }
    
        void RemoveAllChildrenExceptFirst(GameObject parent)
        {
            int childCount = parent.transform.childCount;
            if (childCount > 1)
            {
                for (int i = childCount - 1; i > 0; i--)
                {
                    GameObject.Destroy(parent.transform.GetChild(i).gameObject);
                }
            }
        }

    }
}

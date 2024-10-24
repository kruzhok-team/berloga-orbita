using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnitCreation
{
    public class DeviceItem : MonoBehaviour
    {
        public Device device;
        public TextMeshProUGUI nameText;
        public Image image;

        public void Setup(Device currentDevice)
        {
            device = currentDevice;
            nameText.text = currentDevice.name;
        
            Sprite deviceSprite = Resources.Load<Sprite>(currentDevice.imagePath);

            if (deviceSprite != null)
            {
                image.sprite = deviceSprite;
            }
            else
            {
                Debug.LogError("Sprite not found at path: " + currentDevice.imagePath);
            }
        }
    }
}

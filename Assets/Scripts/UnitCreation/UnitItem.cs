using Connections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XML;

namespace UnitCreation
{
    public class UnitItem : MonoBehaviour
    {
        public Device device;

        public Button deleteBtn;
        public Button infoBtn;
        public Image image;
        
        public TextMeshProUGUI nameText;
        public PartTumblerController tumbler;
        
        private void Start()
        {
            deleteBtn.onClick.AddListener(() =>
            {
                FindFirstObjectByType<XGeneration>().RemoveItem(this);
                Destroy(gameObject);
            });
        }
        
        public void Setup(Device currentDevice)
        {
            device = currentDevice;
            image.sprite = Resources.Load<Sprite>(GameManager.devicePrefixPath + device.Name);
            nameText.text = device.FullName;
            FindFirstObjectByType<XGeneration>().AddItem(this);
        }

        public string GetMode()
        {
            return !tumbler.GetState() ? "OFF" : "ON";
        }
    }
}

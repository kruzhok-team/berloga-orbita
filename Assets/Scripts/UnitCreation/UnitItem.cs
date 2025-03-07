using Connections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XML;

namespace UnitCreation
{
    public class UnitItem : MonoBehaviour
    {
        public StatsCounter statsCounter;
        
        public Device device;

        public Button deleteBtn;
        public Button infoBtn;
        public Image image;
        
        public static GameObject infoPanel;
        public TextMeshProUGUI nameText;
        public PartTumblerController tumbler;
        

        private void Start()
        {
            statsCounter = FindFirstObjectByType<StatsCounter>();
            statsCounter.AddNewDevice(device);
            deleteBtn.onClick.AddListener(() =>
            {
                statsCounter.RemoveDevice(device);
                FindFirstObjectByType<XGeneration>().RemoveItem(this);
                Destroy(gameObject);
            });

            infoBtn.onClick.AddListener(() =>
            {
                infoPanel.SetActive(true);
                infoPanel.GetComponent<InfoPanelController>().Initialize(device);
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

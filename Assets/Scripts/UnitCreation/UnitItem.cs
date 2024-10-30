using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnitCreation
{
    public class UnitItem : MonoBehaviour
    {
        public Device device;

        public Button deleteBtn;
        public Button infoBtn;
        public Image image;
        
        public TextMeshProUGUI nameText;
        public TMP_Dropdown dropdown;
        
        private void Start()
        {
            deleteBtn.onClick.AddListener(() => Destroy(gameObject));
        }
        

        public void Setup(Device currentDevice)
        {
            device = currentDevice;
            image.sprite = Resources.Load<Sprite>(device.imagePath);
            nameText.text = device.name;
        }
    }
}

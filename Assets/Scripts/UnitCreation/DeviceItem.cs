using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeviceItem : MonoBehaviour
{
    public Device device;
    public TextMeshProUGUI nameText;
    public Image image;

    public void Setup(Device device)
    {
        this.device = device;
        nameText.text = device.name;
        
        Sprite deviceSprite = Resources.Load<Sprite>(device.imagePath);

        if (deviceSprite != null)
        {
            image.sprite = deviceSprite;
        }
        else
        {
            Debug.LogError("Sprite not found at path: " + device.imagePath);
        }
    }
}

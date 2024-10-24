using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private GameObject dragCopy;
    private bool droppedInZone = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>(); 
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragCopy = Instantiate(gameObject, canvas.transform);
        dragCopy.GetComponent<CanvasGroup>().blocksRaycasts = false;
        
        dragCopy.transform.SetAsLastSibling();
        Image originalImage = GetComponent<Image>();
        Image copyImage = dragCopy.GetComponent<Image>();
        
        if (originalImage != null && copyImage != null)
        {
            copyImage.sprite = originalImage.sprite; 
            dragCopy.GetComponent<Image>().sprite = originalImage.sprite;
            dragCopy.GetComponent<RectTransform>().sizeDelta = originalImage.rectTransform.sizeDelta;
            dragCopy.GetComponent<RectTransform>().position = originalImage.rectTransform.position;
        }
        else
        {
            Debug.LogError("Image is null");
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        RectTransform copyRectTransform = dragCopy.GetComponent<RectTransform>();
        copyRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!droppedInZone)
        {
            DestroyCopyObject();
        }
        else
        {
            dragCopy.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        droppedInZone = false;
    }
    
    public void SetDroppedInZone()
    {
        droppedInZone = true;
    }

    public void DestroyCopyObject()
    {
        Destroy(dragCopy);
    }
}
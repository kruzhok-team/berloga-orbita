using System.Collections;
using Connections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnitCreation
{
    public class DropArea : MonoBehaviour, IDropHandler
    {
        public float scrollDuration = 0.5f; 
        
        private ScrollRect _scrollRect;
        
        public GameObject unitPrefab;
        public Transform contentTransform;
        public GameObject infoPanel;
        
        private void Start()
        {
            infoPanel.SetActive(false);
            UnitItem.infoPanel = infoPanel;
            _scrollRect = GetComponent<ScrollRect>();
            gameObject.transform.SetAsFirstSibling();
        }

        public void OnDrop(PointerEventData eventData)
        {
            DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();
            if (item == null) return;
            
            item.SetDroppedInZone();
            item.DestroyCopyObject();
            
            CreateUnit(item.deviceItem.device);
        }

        private void CreateUnit(Device device)
        {
            GameObject unit = Instantiate(unitPrefab, contentTransform);
            unit.GetComponent<UnitItem>().Setup(device);
            StartCoroutine(SmoothScrollToBottomRoutine());
            
        }
        
        
        private IEnumerator SmoothScrollToBottomRoutine()
        {
            yield return new WaitForEndOfFrame();
            
            float startPos = _scrollRect.verticalNormalizedPosition;
            float targetPos = 0f;
            float elapsedTime = 0f;
            
            while (elapsedTime < scrollDuration)
            {
                elapsedTime += Time.deltaTime;
                _scrollRect.verticalNormalizedPosition = Mathf.Lerp(startPos, targetPos, elapsedTime / scrollDuration);

                yield return null;
            }
            _scrollRect.verticalNormalizedPosition = targetPos;
        }
    }
}
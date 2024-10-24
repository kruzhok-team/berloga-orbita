using System.Collections;
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
        private void Start()
        {
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
            // Ждем конец кадра, чтобы элемент был добавлен в layout
            yield return new WaitForEndOfFrame();

            // Начальная позиция прокрутки
            float startPos = _scrollRect.verticalNormalizedPosition;

            // Целевая позиция (вниз)
            float targetPos = 0f;

            // Время, прошедшее с начала анимации
            float elapsedTime = 0f;

            // Плавно изменяем позицию прокрутки
            while (elapsedTime < scrollDuration)
            {
                elapsedTime += Time.deltaTime;

                // Линейная интерполяция от начальной позиции к целевой
                _scrollRect.verticalNormalizedPosition = Mathf.Lerp(startPos, targetPos, elapsedTime / scrollDuration);

                yield return null; // Ждем следующий кадр
            }

            // Устанавливаем конечную позицию прокрутки
            _scrollRect.verticalNormalizedPosition = targetPos;
        }
    }
}
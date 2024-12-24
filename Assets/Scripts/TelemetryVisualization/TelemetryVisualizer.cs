using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TelemetryVisualization
{
    public class TelemetryVisualizer : MonoBehaviour
    {
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI heightText;
        public RectTransform ball;
        
        public RectTransform startPoint;
        public RectTransform endPoint;
        public float duration = 10.0f;
        

        private List<TelemetryData> telemetryData = new List<TelemetryData>();
        // TODO: add state type and varible
        
        public void CloseVisualization()
        {
            gameObject.SetActive(false);
        }
        public void Visualize(List<TelemetryData> data) // TODO: collect state varible
        {
            telemetryData = data;
            // TODO: end logic 
            StartCoroutine(MoveBall());
        }

        public void ReVisualize()
        {
            if (telemetryData.Count == 0)
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(MoveBall());
        }
        
        private IEnumerator MoveBall()
        {
            if (telemetryData.Count == 0)
            {
                Debug.LogWarning("No telemetry data to visualize");
                yield break;
            }
            
            var endPos =  endPoint.anchoredPosition.y;
            ball.anchoredPosition = new Vector2(ball.anchoredPosition.x, startPoint.anchoredPosition.y);
            
            float oneUnitHeight = (startPoint.anchoredPosition.y - endPos) / telemetryData[0].height;
            
            heightText.text = $"{ telemetryData[0].height}";
            timeText.text = $"{ telemetryData[0].time}";
        
            yield return new WaitForSeconds(duration / telemetryData.Count);

            for (int i = 1; i < telemetryData.Count; i++)
            {
                float targetY = oneUnitHeight * telemetryData[i].height;
                ball.anchoredPosition = new Vector2(ball.anchoredPosition.x, targetY);
                
                heightText.text = $"{ telemetryData[i].height}";
                timeText.text = $"{ telemetryData[i].time}";
            
                yield return new WaitForSeconds(duration / telemetryData.Count);
            }
        }
    }
}
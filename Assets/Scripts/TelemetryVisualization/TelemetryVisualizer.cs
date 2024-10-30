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
        public float duration = 10.0f;

        private List<TelemetryData> telemetryData = new List<TelemetryData>();
        // TODO: add state type and varible
       
        public void Visualize(List<TelemetryData> data) // TODO: collect state varible
        {
            telemetryData = data;
            // TODO: end logic 
            StartCoroutine(MoveBall());
        }
        
        private IEnumerator MoveBall()
        {
            if (telemetryData.Count == 0)
            {
                Debug.LogWarning("No telemetry data to visualize");
                yield break;
            }
            float offset = ball.rect.height * ball.localScale.y;
            float oneUnitHeight = (1280 - offset) / telemetryData[0].height;
            
            ball.anchoredPosition = new Vector2(ball.anchoredPosition.x, 1280 - offset);
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
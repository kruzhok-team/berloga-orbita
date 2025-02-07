using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TelemetryVisualization
{
    public class TelemetryVisualizer : MonoBehaviour
    {
        private static readonly int Reboot = Animator.StringToHash("reboot");
        private static readonly int BlowUp = Animator.StringToHash("blowUp");
        
        public Animator animator;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI heightText;
        public RectTransform ball;
        
        public RectTransform startPoint;
        public RectTransform endPoint;
        public float duration = 10.0f;
        

        private List<TelemetryData> telemetryData = new List<TelemetryData>();
        private FinalResults finalResults;
        
        public void CloseVisualization()
        {
            gameObject.SetActive(false);
        }
        public void Visualize(List<TelemetryData> data, FinalResults results) // TODO: collect state varible
        {
            telemetryData = data;
            finalResults = results;
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
            animator.SetTrigger(Reboot);
            if (telemetryData.Count == 0)
            {
                Debug.LogWarning("No telemetry data to visualize");
                yield break;
            }
            
            var endPos =  endPoint.anchoredPosition.y;
            ball.anchoredPosition = new Vector2(ball.anchoredPosition.x, startPoint.anchoredPosition.y);
            
            float oneUnitHeight = (startPoint.anchoredPosition.y - endPos) / telemetryData[0].height;
            
            heightText.text = $"{ telemetryData[0].height} м";
            timeText.text = $"{ telemetryData[0].time} с";
        
            yield return new WaitForSeconds(duration / telemetryData.Count);

            for (int i = 1; i < telemetryData.Count; i++)
            {
                float targetY = startPoint.anchoredPosition.y - oneUnitHeight * (telemetryData[0].height - telemetryData[i].height);

                ball.anchoredPosition = new Vector2(ball.anchoredPosition.x, targetY);
                
                heightText.text = $"{ telemetryData[i].height} м";
                timeText.text = $"{ telemetryData[i].time} с";
            
                yield return new WaitForSeconds(duration / telemetryData.Count);
            }

            if (!finalResults.IsSuccess)
            {
                animator.SetTrigger(BlowUp);
            }
        }
        
    }
}
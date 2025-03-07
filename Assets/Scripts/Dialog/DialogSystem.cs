using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    [HideInInspector] public bool isEnded = true;
    private string text = null;
    
    private Coroutine coroutine = null;

    public void EndWriting()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            dialogText.SetText(text);
        }
        coroutine = null;
        text = null;
        isEnded = true;
    }
    
    public void WriteLine(string line)
    {
        EndWriting();
        isEnded = false;
        text = line;
        coroutine = StartCoroutine(WriteTextRoutine());
    }

    private IEnumerator WriteTextRoutine()
    {
        dialogText.text = "";
        string nowText = "";
        foreach (var symb in text)
        {
            dialogText.SetText(nowText);
            nowText += symb.ToString();
            yield return new WaitForSeconds(0.05f);
        }
        EndWriting();
    }
}

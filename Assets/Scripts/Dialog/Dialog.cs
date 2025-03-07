using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(DialogSystem))]
public class Dialog : MonoBehaviour, IPointerClickHandler
{
    private static readonly int LineCount = Animator.StringToHash("lineCount");
    public Animator guyAnimator;
    public List<string> lines;
    public DialogSystem dialogSystem;
    private int currentLine = 0;

    void Start()
    {
        dialogSystem = GetComponent<DialogSystem>();
        NextLine();
    }

    private void EndDialog()
    {
        
    }
    
    private void NextLine()
    {
        guyAnimator.SetInteger(LineCount, currentLine % 3);
        if (currentLine >= lines.Count)
        {
            EndDialog();
            Destroy(gameObject);
            return; 
        }
        dialogSystem.WriteLine(lines[currentLine++]);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (dialogSystem.isEnded)
        {
            NextLine();
            return;
        }
        dialogSystem.EndWriting();
    }
}

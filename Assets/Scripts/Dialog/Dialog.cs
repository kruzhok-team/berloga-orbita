using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(DialogSystem))]
public class Dialog : MonoBehaviour, IPointerClickHandler
{
    public List<string> lines;
    public DialogSystem dialogSystem;
    private int currentLine = 0;

    void Start()
    {
        dialogSystem = GetComponent<DialogSystem>();
        //NextLine();

    }

    private void EndDialog()
    {
        
    }
    
    private void NextLine()
    {
        if (currentLine >= lines.Count)
        {
            EndDialog();
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

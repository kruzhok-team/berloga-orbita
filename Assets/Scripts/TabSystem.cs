using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
[Serializable]
public class TabController
{
    public Button trigger;
    public GameObject panel;
}

/// <summary>
/// 
/// </summary>
public class TabSystem : MonoBehaviour
{
    private TabController activeTab;
    public List<TabController> tabs = new List<TabController>();
    
    private void Start()
    {
        if (tabs.Count == 0)
        {
            Debug.LogWarning("No Tabs Available, check inspector");
            Debug.LogWarning("For save usage component self destroys");
            Destroy(this);
            return;
        }
        gameObject.transform.SetAsLastSibling();
        
        foreach (var tab in tabs)
        {
            if (tab.trigger == null || tab.panel == null)
            {
                Debug.LogWarning("Tab was null, check inspector");
                continue;
            }
            tab.panel.SetActive(false);
            tab.trigger.onClick.AddListener(() => ActivateTab(tab));
        }
        
        activeTab = tabs[0];
        activeTab.panel.SetActive(true);
    }

    private void ActivateTab(TabController tab)
    {
        activeTab.panel.SetActive(false);
        
        activeTab = tab;
        activeTab.panel.SetActive(true);
    }

}

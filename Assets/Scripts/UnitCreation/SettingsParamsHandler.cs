using System;
using Menu;
using UnityEngine;
using XML;

namespace UnitCreation
{
    [RequireComponent(typeof(MissionHandler))]
    public class SettingsParamsHandler : MonoBehaviour, IXHandler
    {
        private MissionHandler missionHandler;

        private void Start()
        {
            missionHandler = GetComponent<MissionHandler>();
        }


        public void CallBack()
        {
            var xModule = FindFirstObjectByType<GameManager>().GetXModule();
            xModule.InsertInnerValue("//start_height", missionHandler.startHeightField.text); // TODO
        }
    }
}

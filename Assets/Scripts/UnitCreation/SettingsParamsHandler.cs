using System;
using Menu;
using UnityEngine;
using XML;

namespace UnitCreation
{
    [RequireComponent(typeof(MissionHandler))]
    public class SettingsParamsHandler : MonoBehaviour, IXHandler
    {
        private MissionHandler _missionHandler;

        private void Start()
        {
            _missionHandler = GetComponent<MissionHandler>();
        }


        public void CallBack()
        {
            var xModule = FindFirstObjectByType<GameManager>().GetXModule();
            xModule.InsertInnerValue("//start_height", _missionHandler.baseParameterSetup.startHeight.text);
            if (_missionHandler.baseParameterSetup.radiusType == RadiusType.Both)
            {
                xModule.InsertInnerValue("//radius_external", _missionHandler.baseParameterSetup.externalRadius.text);
                xModule.InsertInnerValue("//radius_internal", _missionHandler.baseParameterSetup.internalRadius.text);
            }
            if (_missionHandler.baseParameterSetup.radiusType == RadiusType.Iternal)
            {
                xModule.InsertInnerValue("//radius_internal", _missionHandler.baseParameterSetup.internalRadius.text);
            }
        }
    }
}

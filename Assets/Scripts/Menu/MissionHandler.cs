using Connections;
using UnityEngine;

namespace Menu
{
    [System.Serializable]
    public class Mission
    { 
        public string missionName;
        public bool allowedBallisticCalculator;
    }
    
    public class MissionHandler : MonoBehaviour
    {
        private ConnectionsModule _server;
    
        void Start()
        {
            _server = gameObject.AddComponent(typeof(ConnectionsModule)) as ConnectionsModule;
        }

  
        void Update()
        {
        
        }
    }
}

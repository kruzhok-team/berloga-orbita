using System.Collections.Generic;
using UnitCreation;
using UnityEngine;

namespace XML
{
    public class XGeneration : MonoBehaviour
    {
        [HideInInspector] public XHandlerPasteAttribute AttributeMngr;

        private void Start()
        {
            AttributeMngr = gameObject.AddComponent<XHandlerPasteAttribute>();
        }
        private void CollectParts()
        {
            AttributeMngr.xpath = "0"; // TODO: paste
            var parts = FindObjectsOfType<UnitItem>();
            Dictionary<string, int> uniq = new Dictionary<string, int>();
            
            foreach (var part in parts)
            {
                if (!uniq.ContainsKey(part.name))
                {
                    AttributeMngr.defaultAttributes.Add(new Pair());
                }
                
            }

        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

using System.Collections.Generic;
using Connections;
using TMPro;
using UnitCreation;
using UnityEngine;

namespace XML
{
    public class XGeneration : MonoBehaviour
    {
        public GameObject prefab;
        public XHandlerInsertCode xCode;
        public SettingsParamsHandler settings;
        
        
        private Transform _transform;
        private List<UnitItem> _items = new List<UnitItem>();

        public void AddItem(UnitItem item)
        {
            _items.Add(item);
        }

        public void RemoveItem(UnitItem item)
        {
            _items.Remove(item);
        }
        
        private void Start()
        {
            _transform = gameObject.GetComponent<Transform>();
        }

        private List<IXHandler> CollectUnt()
        {
            GameManager m = FindFirstObjectByType<GameManager>();
            List<IXHandler> handlers = new List<IXHandler>();

            Dictionary<string, int> counter = new Dictionary<string, int>();

            int i = 1;
            foreach (var part in _items)
            {
                Device d = part.device;
                var mgr = Instantiate(prefab, _transform).GetComponent<XHandlerPasteAttribute>();
                mgr._module = m.GetXModule();
                handlers.Add(mgr);
                mgr.xpath = "//devices"; // [" + i + "]" ;
                mgr.needCreateNewElement = true;
                mgr.elementName = "device";

                counter.TryAdd(d.Name, 1);

                mgr.defaultAttributes.Add(new Pair() { key = "number", value = counter[d.Name].ToString() });
                mgr.defaultAttributes.Add(new Pair() { key = "name", value = d.Name });
                mgr.defaultAttributes.Add(new Pair() { key = "start_state", value = part.GetMode() });

                counter[d.Name]++;
                ++i;
            }
            return handlers;
        }
        
        public List<IXHandler> CollectParts()
        {
            List<IXHandler> handlers = CollectUnt();
            handlers.Add(xCode);
            handlers.Add(settings);
            return handlers;

        }
    }
}

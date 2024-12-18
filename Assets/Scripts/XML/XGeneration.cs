using System.Collections.Generic;
using TMPro;
using UnitCreation;
using UnityEngine;

namespace XML
{
    public class XGeneration : MonoBehaviour
    {
        public GameObject prefab;
        public XHandlerInsertCode xCode;
        
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
                var d = part.device;
                var mgr = Instantiate(prefab, _transform).GetComponent<XHandlerPasteAttribute>();
                mgr._module = m.GetXModule();
                handlers.Add(mgr);
                mgr.xpath = "//devices"; // [" + i + "]" ;
                mgr.needCreateNewElement = true;
                mgr.elementName = "device";

                counter.TryAdd(d.name, 1);

                mgr.defaultAttributes.Add(new Pair() { key = "number", value = counter[d.name].ToString() });
                mgr.defaultAttributes.Add(new Pair() { key = "name", value = d.name });
                mgr.defaultAttributes.Add(new Pair() { key = "start_state", value = part.GetMode() });

                counter[d.name]++;
                ++i;
            }
            return handlers;
        }
        
        public List<IXHandler> CollectParts()
        {
            List<IXHandler> handlers = CollectUnt();
            handlers.Add(xCode);
            return handlers;

        }
    }
}

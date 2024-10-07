using UnityEngine;

namespace XML
{
    public class test : MonoBehaviour
    {
        private XMLModule module;
        void Start()
        {
            module = new XMLModule(XMLModule.DefaultFilePath, "test.xml");
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Debug.Log(module.test);
        }
    }
}

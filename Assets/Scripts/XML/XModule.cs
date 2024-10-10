using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;

namespace XML
{
    [Serializable]
    public class Pair
    {
        public string key;
        public string value;
    }
    public class XModule
    {
        public static string DefaultFilePath { get; } = Application.dataPath;
    
        private XmlDocument _document = new XmlDocument();
    
        private string _filePath;
        private string _savePath;
        private string _fileName;
        public XModule(string filepath, string filename) // change 
        {
            _filePath = filepath;
            _fileName = filename;
        
            _document.Load(filepath + "/" +  filename);
        }

        public void InsertElement(string xpath, XmlElement newElement)
        {
            XmlNode node = _document.SelectSingleNode(xpath);
            if (node != null)
            {
                node.AppendChild(newElement);
            }
        }

        public void InsertInnerValue(string xpath, string value)
        {
            XmlNode node = _document.SelectSingleNode(xpath);
            if (node == null)
            {
                Debug.LogError($"Cannot find element by xpath: {xpath}");
                return;
            }
            node.InnerText = value;
        }
        
        public void PasteNewAttribute(string xpath, string name, string value)
        {
            XmlNode node = _document.SelectSingleNode(xpath);

            if (node == null)
            {
                Debug.LogError($"Given xpath : {xpath} not found in xml file");
                return;
            }
            XmlAttribute newAttr = _document.CreateAttribute(name);
            newAttr.Value = value;
            
            if (node.Attributes == null)
            {
                Debug.LogError($"By given xpath : {xpath}, null reference for attributes");
                return;
            }
            node.Attributes.Append(newAttr);
        }

        public XmlElement CreateNewXmlElement(string name, List<Pair> attributes)
        {
            XmlElement newElement = _document.CreateElement(name);

            foreach (var attribute in attributes)
            {
                newElement.SetAttribute(attribute.key, attribute.value);
            }

            return newElement; // ref
        }
        
        
        public void SaveDocument(string name, string path)
        {
            _document.Save(path + "/" + name);
        }
 
    }
}

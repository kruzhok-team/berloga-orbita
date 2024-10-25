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
        private readonly XmlDocument _document = new XmlDocument();

        public string FilePath = "";
        public string FileName = "";
        
        //private string _savePath;

        public XModule() {}
        public XModule(string filepath, string filename)
        {
            FilePath = filepath;
            FileName = filename;
        }

        public void Reload()
        { 
            _document.Load(FilePath + "/" +  FileName);
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
        
        /// <summary>
        /// Inserts values into existing attribute by following xpath
        /// </summary>
        /// <param name="pairs"> Key of pair is xpath, Value is inserting value</param>

        public void InsertInnerValues(List<Pair> pairs)
        {
            foreach (var pair in pairs)
            {
                InsertInnerValue(pair.key, pair.value);
            }
        }

        public void PasteNewAttributes(string xpath, List<Pair> newAttributes)
        {
            foreach (var pair in newAttributes)
            {
                PasteNewAttribute(xpath, pair.key, pair.value);
            }
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

        public void LogDocument()
        {
            Debug.Log(_document.OuterXml);
        }
 
    }
}

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace XML
{
    [Serializable]
    public class Pair
    {
        public string Key;  // Первый элемент пары
        public string Value;  // Второй элемент пары
    }
    public class XMLModule
    {
        public static string DefaultFilePath { get; } = Application.dataPath;
    
        private XmlDocument _document = new XmlDocument();
    
        private string _filePath;
        private string _fileName;
        public XMLModule(string filepath, string filename)
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
        public XmlElement CreateElement(string name, List<Pair> attributes)
        {
            XmlElement newElement = _document.CreateElement(name);

            foreach (var attribute in attributes)
            {
                newElement.SetAttribute(attribute.Key, attribute.Value);
            }

            return newElement; // ref
        }

        public void SaveDocument(string name, string path)
        {
            _document.Save(path + "/" + name);
        }
 
    }
}

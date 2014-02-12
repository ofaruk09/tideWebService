using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using tideWebService.Models;

namespace tideWebService.Controllers
{
    public class portController : ApiController
    {
        string filePath;
        // GET api/port/5
        portController()
        {
            filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            //filePath = @"C:\Users\Omorr";
            filePath = filePath + @"\portIDs.xml";
        }
        public xmlPort[] Get()
        {
            XmlDocument devIDDocument = new XmlDocument();
            devIDDocument.Load(filePath);
            XmlNodeList nodes = devIDDocument.GetElementsByTagName("Port");
            int count = nodes.Count;
            xmlPort[] ports = new xmlPort[count];
            for (int i = 0; i < count; i++)
            {
                xmlPort port = new xmlPort();
                XmlNode node = nodes.Item(i);
                XmlNodeList innerNodes = node.ChildNodes;
                String portID = innerNodes.Item(0).InnerText;
                String portName = innerNodes.Item(1).InnerText;
                String latitude = innerNodes.Item(2).InnerText;
                String longitude = innerNodes.Item(3).InnerText;

                port.portID = portID;
                port.portName = portName;
                port.Latitude = latitude;
                port.Longitude = longitude;
                ports[i] = port;
            }
            return ports;
        }
    }
}

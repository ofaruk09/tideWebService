using HtmlAgilityPack;
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
    public class ValuesController : ApiController
    {
        static HtmlNode tablesRootNodeCollection;
        static HtmlDocument doc;
        // GET api/values
       List<DayTideCollection> getDataFor6Days()
        {
            List<DayTideCollection> ForecastFor6Days = new List<DayTideCollection>();
            DateTime now = DateTime.Now;
           //for loop downloads data for 7 days, today inclusive.
            for (int i = 1; i < 8; i++)
            {
                String dayXpath = tablesRootNodeCollection.XPath + "/table[" + i + "]";
                DayTideCollection dayEntry = new DayTideCollection();
                dayEntry.EventData = getDataForDay(tablesRootNodeCollection.SelectSingleNode(dayXpath));
                //deal with date data here
                if (i == 1)
                {
                    //i is equal to 1, the first day in the forecast is always todays date
                    dayEntry.Date = now.Date.ToString().Split(' ')[0];
                }
                else
                {
                    //when i is more than 1, this is i-1 days more than today
                    //e.g. tomorrow i = 2. so tomorrow will be 1 day more than today (i - 1 = 1)
                    dayEntry.Date = now.AddDays((double)i - 1).Date.ToString().Split(' ')[0];
                }
                ForecastFor6Days.Add(dayEntry);
            } 
            return ForecastFor6Days;
        }
        //this method returns the tide data for a given day
        List<TidalEvent> getDataForDay(HtmlNode dayIndex)
        {
            int columnCount = dayIndex.SelectSingleNode(dayIndex.XPath + "/tr[2]").ChildNodes.Count;
            List<TidalEvent> eventList = new List<TidalEvent>();

            // start filling tidal event here
            for (int i = 1; i < columnCount - 1; i++) // we need the minus 1 because the library picks up 5 nodes when we only want the first 4
            {
                TidalEvent thisEvent = new TidalEvent();
                String headerNumber = "/th[" + i + "]";
                String definitionNumber = "/td[" + i + "]";
                thisEvent.WaterMode = dayIndex.SelectSingleNode(dayIndex.XPath + "/tr[2]" + headerNumber).InnerText;
                thisEvent.Time = dayIndex.SelectSingleNode(dayIndex.XPath + "/tr[3]" + definitionNumber).InnerText;
                thisEvent.Height = dayIndex.SelectSingleNode(dayIndex.XPath + "/tr[4]" + definitionNumber).InnerText.Replace("&nbsp;m","");
                eventList.Add(thisEvent);

            }
            return eventList;
        }
        public Port Post(Port port)
        {
            // port ids are received unformatted and requests must be made with a 4 character port id. this will format it
            string portId = port.portID.PadLeft(4, '0');
            string url = @"http://www.ukho.gov.uk/easytide/EasyTide/ShowPrediction.aspx?PortID=" + portId + "&PredictionLength=7";
            string tablesRootNodeXpath = @"/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[1]";
            doc = new HtmlWeb().Load(url);
            tablesRootNodeCollection = doc.DocumentNode.SelectSingleNode(tablesRootNodeXpath);
            port.portForecast = getDataFor6Days();
            return port;
        }
    }
}
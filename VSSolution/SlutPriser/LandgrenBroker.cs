using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsQuery;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Data.Objects.DataClasses;
using System.Xml;

namespace SlutPriser
{
    public class LandgrenBroker : Broker
    {
        //private readonly string selector = ".Highres .img img";
        private readonly string imagesUrl = "http://www.peterlandgren.se/objekt/getFasadObjXml.php?objId=";
        private string objectId;
        private string _xml;

        public LandgrenBroker(string moreImagesLink, string brokerName) : base(moreImagesLink, brokerName)
        {
            objectId = moreImagesLink.Split('?')[1];
        }

        public override List<string> GetImageLinks()
        {
            try
            {
                WebRequest moreImagesRequest = WebRequest.Create(imagesUrl + objectId);
                ((HttpWebRequest)moreImagesRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                var moreImagesResponse = moreImagesRequest.GetResponse();
                using (var stream3 = moreImagesResponse.GetResponseStream())
                using (var reader3 = new StreamReader(stream3))
                {
                    _xml = reader3.ReadToEnd();
                }
                XmlSerializer serializer = new XmlSerializer(typeof(Item));
                XmlTextReader reader = new XmlTextReader(new StringReader(_xml));

                Item h = (Item)serializer.Deserialize(reader);

                return h.Images.Select(x => x.Medium.First().path).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to retrieve images at: " + imagesUrl + objectId);
                return new List<string>();
            }
        }
    }
}

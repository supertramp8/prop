using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsQuery;
using System.Net;
using System.IO;
using System.Data.Objects.DataClasses;
using System.Diagnostics;

namespace SlutPriser
{
    public class Broker
    {
        protected string Selector { get; set;}
        protected string MoreImagesLink {get; set;}
        public string BrokerName { get; set; }

        public Broker(string moreImagesLink, string brokerName)
        {
            this.MoreImagesLink = moreImagesLink;
            this.BrokerName = brokerName;
        }

        public Broker(string moreImagesLink, string selector, string brokerName) : this(moreImagesLink, brokerName)
        {
            this.Selector = selector;
            
        }

        public virtual List<string> GetImageLinks()
        {
            string domString;
            WebRequest moreImagesRequest = WebRequest.Create(MoreImagesLink);
            ((HttpWebRequest)moreImagesRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
            var moreImagesResponse = moreImagesRequest.GetResponse();
            using (var stream3 = moreImagesResponse.GetResponseStream())
            using (var reader3 = new StreamReader(stream3))
            {
                domString = reader3.ReadToEnd();
            }

            CQ moreImagesDom = domString;
            CQ imageLinks = moreImagesDom[Selector];

            return imageLinks.Select(x => GetImageLinkForBroker(x, MoreImagesLink)).ToList();
        }

        public virtual EntityCollection<Images> DownloadImages(string address, string hash)
        {
            try
            {
                var imageLinks = GetImageLinks();

                string localFilename = @"c:\temp\" + BrokerName + "\\" + address + "\\";
                int i = 1;

                var images = new EntityCollection<Images>();
                string subPath = hash + "\\";
                bool exists = System.IO.Directory.Exists(localFilename + subPath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(localFilename + subPath);
                foreach (var imageLink in imageLinks)
                {
                    using (WebClient requestPic = new WebClient())
                    {
                        var location = localFilename + subPath + i++ + ".jpg";
                        requestPic.DownloadFile(imageLink, location);
                        images.Add(new Images()
                        {
                            Location = location,
                        });
                    };
                }

                return images;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to download image: " + address);
                return new EntityCollection<Images>();
            }
        }

        private string GetImageLinkForBroker(IDomObject dom, string brokerUrl)
        {
            string selector = "src";
            string imageUrl = "";
            brokerUrl = brokerUrl.ToLower();
            if (brokerUrl.Contains("bjurfors"))
            {
                selector = "data-original";
            }
            else if (brokerUrl.Contains("svenskfast"))
            {
                selector = "data-url-medium";
            }
            else if (brokerUrl.Contains("bulowlind"))
            {
                selector = "data-src";
            }
            else if (brokerUrl.Contains("erasweden"))
            {
                selector = "data-mobile";
            }


            var imageLink = dom.GetAttribute(selector);
            imageUrl = !string.IsNullOrEmpty(imageLink) ? imageLink : imageUrl + dom.GetAttribute("src");

            if (!imageUrl.Contains("http://") && !imageUrl.Contains("https://"))
            {
                imageUrl = brokerUrl.Substring(0, brokerUrl.LastIndexOf('/')) + imageUrl;
            }

            return imageUrl;
        }
    }
}

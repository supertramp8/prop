using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsQuery;
using System.Net;
using System.IO;
using System.Data.Objects.DataClasses;

namespace SlutPriser
{
    public class Broker
    {
        protected string selector { get; set;}
        protected string moreImagesLink {get; set;}
        protected string brokerName { get; set; }

        public Broker(string moreImagesLink, string brokerName)
        {
            this.moreImagesLink = moreImagesLink;
            this.brokerName = brokerName;
        }

        public Broker(string moreImagesLink, string selector, string brokerName) : this(moreImagesLink, brokerName)
        {
            this.selector = selector;
            
        }

        public virtual List<string> GetImageLinks()
        {
            string domString;
            WebRequest moreImagesRequest = WebRequest.Create(moreImagesLink);
            ((HttpWebRequest)moreImagesRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
            var moreImagesResponse = moreImagesRequest.GetResponse();
            using (var stream3 = moreImagesResponse.GetResponseStream())
            using (var reader3 = new StreamReader(stream3))
            {
                domString = reader3.ReadToEnd();
            }

            CQ moreImagesDom = domString;
            CQ imageLinks = moreImagesDom[selector];

            return imageLinks.Select(x => GetImageLinkForBroker(x, moreImagesLink)).ToList();
        }

        public virtual EntityCollection<Images> DownloadImages()
        {
            var imageLinks = GetImageLinks();

            string localFilename = @"c:\temp\" + brokerName + "\\";
            int i = 1;

            var images = new EntityCollection<Images>();
            string subPath = Guid.NewGuid().ToString() + "\\";
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

        private string GetImageLinkForBroker(IDomObject dom, string brokerUrl)
        {
            string selector = "src";
            string imageUrl = "";
            brokerUrl = brokerUrl.ToLower();
            if (brokerUrl.Contains("bjurfors"))
            {
                selector = "data-original";
            }

            var imageLink = dom.GetAttribute(selector);
            imageUrl = !string.IsNullOrEmpty(imageLink) ? imageLink : imageUrl + dom.GetAttribute("src");

            if (!imageUrl.Contains("http://"))
            {
                imageUrl = brokerUrl.Substring(0, brokerUrl.LastIndexOf('/')) + imageUrl;
            }

            return imageUrl;
        }
    }
}

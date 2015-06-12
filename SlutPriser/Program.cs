using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using CsQuery;
using System.Web;
using System.Data.Objects.DataClasses;

namespace SlutPriser
{
    class Program
    {
        static CQ GetListItems()
        {
            string domString;
            WebRequest wr = WebRequest.Create("http://www.hemnet.se/resultat");
            wr.Headers.Add("Cookie", "cx_profile_timeout=1433961137360; cx_profile_data=%7B%22CxenseSegments%22%3A%5B%228kack5xybxki%22%5D%7D; results%2Fh3%2Fresults%2Fresult_settings%2Fversion=2; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fsorting=sale_date+desc; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fsorting_history=sale_date; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fper_page=20; cX_S=iar3celtawp2gtlj; last_search=BAhJIgL1AUJBaDdDVG9SYkc5allYUnBiMjVmYVdSeld3ZHBBNGc3QjJrRHZiVU5PaFJzYjJOaGRHbHZibDl6WldGeQpZMmhKSWdHYlczc2lhV1FpT2pRM016azVNaXdpYm1GdFpTSTZJbEpwWW1WeWMySnZjbWNpTENKd1lYSmwKYm5SZmFXUWlPamc1T0RjeE5Td2ljR0Z5Wlc1MFgyNWhiV1VpT2lKTllXeHR3N1lpZlN4N0ltbGtJam80Ck9UZzBPVE1zSW01aGJXVWlPaUpUYkc5MGRITnpkR0ZrWlc0aUxDSndZWEpsYm5SZmFXUWlPamc1T0RjeApOU3dpY0dGeVpXNTBYMjVoYldVaU9pSk5ZV3h0dzdZaWZWMEdPZ1pGVkRvYWJHOWpZWFJwYjI1ZmMyVmgKY21Ob1gyRnljbUY1V3dkN0NUb0hhV1JwQTRnN0J6b0pibUZ0WlVraUQxSnBZbVZ5YzJKdmNtY0dPd2RVCk9nNXdZWEpsYm5SZmFXUnBBNXUyRFRvUWNHRnlaVzUwWDI1aGJXVkpJZ3ROWVd4dHc3WUdPd2RVZXdrNwpDV2tEdmJVTk93cEpJaEZUYkc5MGRITnpkR0ZrWlc0R093ZFVPd3RwQTV1MkRUc01RQXM2Q0dGblpVa2kKQnpOa0Jqc0hWQT09CgY6BkVG--bf843bb2758ee6bdb2b9295c24a62e1fa55a97fb; last_search_info=Ribersborg%2C%2520Malm%25C3%25B6%3B%2520Slottsstaden%2C%2520Malm%25C3%25B6%3B%2520max%25203%2520dagar%2520gammalt; member_type=0; _cX_segmentTime=1434044500; _cX_segmentIds=8kack5xybxki; results%2Fh3%2Fresults%2Fresult_settings%2Fsorting=creation+desc; results%2Fh3%2Fresults%2Fresult_settings%2Fsorting_history=creation; results%2Fh3%2Fresults%2Fresult_settings%2Flist=collapsed; results%2Fh3%2Fresults%2Fresult_settings%2Fper_page=50; _ga=GA1.2.1324110632.1431890058; _gat=1; cX_P=i9sudc8hizip46sy; cx_profile_timeout=1434044502552; cx_profile_data=%7B%22CxenseSegments%22%3A%5B%228kack5xybxki%22%5D%7D; OAS_SC1=1434044502912; ki_t=1431890059151%3B1434044500007%3B1434044502993%3B15%3B148; ki_r=; _hemnet_session_id=56547f82ef5a063c7e35af7a84d12473");
            var h = wr.GetResponse();
            using (var stream = h.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                domString = reader.ReadToEnd();
            }
            CQ dom = domString;
            CQ items = dom["#search-results .item-link-container"];
            return items;
        }

        static Property GetObjectData(IDomObject item)
        {
            string domString;
            WebRequest wr = WebRequest.Create("http://www.hemnet.se/resultat");
            wr.Headers.Add("Cookie", "cx_profile_timeout=1433961137360; cx_profile_data=%7B%22CxenseSegments%22%3A%5B%228kack5xybxki%22%5D%7D; results%2Fh3%2Fresults%2Fresult_settings%2Fversion=2; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fsorting=sale_date+desc; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fsorting_history=sale_date; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fper_page=20; cX_S=iar3celtawp2gtlj; last_search=BAhJIgL1AUJBaDdDVG9SYkc5allYUnBiMjVmYVdSeld3ZHBBNGc3QjJrRHZiVU5PaFJzYjJOaGRHbHZibDl6WldGeQpZMmhKSWdHYlczc2lhV1FpT2pRM016azVNaXdpYm1GdFpTSTZJbEpwWW1WeWMySnZjbWNpTENKd1lYSmwKYm5SZmFXUWlPamc1T0RjeE5Td2ljR0Z5Wlc1MFgyNWhiV1VpT2lKTllXeHR3N1lpZlN4N0ltbGtJam80Ck9UZzBPVE1zSW01aGJXVWlPaUpUYkc5MGRITnpkR0ZrWlc0aUxDSndZWEpsYm5SZmFXUWlPamc1T0RjeApOU3dpY0dGeVpXNTBYMjVoYldVaU9pSk5ZV3h0dzdZaWZWMEdPZ1pGVkRvYWJHOWpZWFJwYjI1ZmMyVmgKY21Ob1gyRnljbUY1V3dkN0NUb0hhV1JwQTRnN0J6b0pibUZ0WlVraUQxSnBZbVZ5YzJKdmNtY0dPd2RVCk9nNXdZWEpsYm5SZmFXUnBBNXUyRFRvUWNHRnlaVzUwWDI1aGJXVkpJZ3ROWVd4dHc3WUdPd2RVZXdrNwpDV2tEdmJVTk93cEpJaEZUYkc5MGRITnpkR0ZrWlc0R093ZFVPd3RwQTV1MkRUc01RQXM2Q0dGblpVa2kKQnpOa0Jqc0hWQT09CgY6BkVG--bf843bb2758ee6bdb2b9295c24a62e1fa55a97fb; last_search_info=Ribersborg%2C%2520Malm%25C3%25B6%3B%2520Slottsstaden%2C%2520Malm%25C3%25B6%3B%2520max%25203%2520dagar%2520gammalt; member_type=0; _cX_segmentTime=1434044500; _cX_segmentIds=8kack5xybxki; results%2Fh3%2Fresults%2Fresult_settings%2Fsorting=creation+desc; results%2Fh3%2Fresults%2Fresult_settings%2Fsorting_history=creation; results%2Fh3%2Fresults%2Fresult_settings%2Flist=collapsed; results%2Fh3%2Fresults%2Fresult_settings%2Fper_page=50; _ga=GA1.2.1324110632.1431890058; _gat=1; cX_P=i9sudc8hizip46sy; cx_profile_timeout=1434044502552; cx_profile_data=%7B%22CxenseSegments%22%3A%5B%228kack5xybxki%22%5D%7D; OAS_SC1=1434044502912; ki_t=1431890059151%3B1434044500007%3B1434044502993%3B15%3B148; ki_r=; _hemnet_session_id=56547f82ef5a063c7e35af7a84d12473");
            var h = wr.GetResponse();
            using (var stream = h.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                domString = reader.ReadToEnd();
            }
            CQ dom = domString;
            CQ items = dom["#search-results .item-link-container"];

            return null;
            //return items;
        }

        static void Main()
        {
            string domString;

            //Get List Items
            var items = GetListItems();

            foreach (var item in items.Take(5))
            {
                //var property = GetObjectData(item);
                var path = item.Attributes["href"];
                WebRequest wr2 = WebRequest.Create("http://www.hemnet.se" + path);
                var h2 = wr2.GetResponse();
                using (var stream2 = h2.GetResponseStream())
                using (var reader2 = new StreamReader(stream2))
                {
                    domString = reader2.ReadToEnd();
                }

                CQ dom2 = domString;
                CQ items2 = dom2[".attributes dd"];
                CQ title = dom2["title"];
                CQ price = dom2["span.price"];
                var titleItems = title.First().Text().Split('-');
                string areaId;
                using (var context = new SlutPriserEntities())
                {
                    var areaName = GetArea(titleItems[2].Split(',')[0].Trim());
                    var cityName = titleItems[2].Split(',')[1].Split('.')[0].Trim();
                    areaId = areaName + cityName;
                    if (!context.Areas.Any(x => x.AreaId == areaName+cityName)) //TODO: needed?
                    {
                        var area = new Areas()
                        {
                            AreaId = areaName + cityName,
                            Area = areaName,
                            City = cityName
                        };
                        context.Areas.AddObject(area);
                    }
                    
                    context.SaveChanges();
                }

                Property property;
                if (items2.Length > 6)
                {
                    property = new Property()
                    {
                        Type = HttpUtility.HtmlDecode(items2.ElementAt(0).InnerText.Trim()),
                        Area = ParseInt(items2.ElementAt(1).InnerText.Trim()),
                        Rooms = ParseInt(items2.ElementAt(2).InnerText.Trim()),
                        Rent = ParseInt(items2.ElementAt(3).InnerText.Trim()),
                        ListingPricePerArea = ParseInt(items2.ElementAt(4).InnerText.Trim()),
                        OperatingCost = ParseInt(items2.ElementAt(5).InnerText.Trim()),
                        BuildYear = ParseInt(items2.ElementAt(6).InnerText.Trim()),
                        Address = titleItems.ElementAt(0),
                        ListingPrice = ParseInt(price.Elements.First().InnerText)
                    };
                }
                else
                {
                    property = new Property()
                    {
                        Type = HttpUtility.HtmlDecode(items2.ElementAt(0).InnerText.Trim()),
                        Area = ParseDouble(items2.ElementAt(1).InnerText.Trim()),
                        Rooms = ParseInt(items2.ElementAt(2).InnerText.Trim()),
                        Rent = ParseInt(items2.ElementAt(3).InnerText.Trim()),
                        ListingPricePerArea = ParseInt(items2.ElementAt(4).InnerText.Trim()),
                        BuildYear = ParseInt(items2.ElementAt(5).InnerText.Trim()),
                        Address = titleItems.ElementAt(0),
                        ListingPrice = ParseInt(price.Elements.First().InnerText)
                    };
                }

                CQ moreImagesButton = dom2[".button.image-list"];
                var moreImagesLink = moreImagesButton.ElementAt(0).GetAttribute("href");

                Broker broker = GetClassForBroker(moreImagesLink);
                var images = broker.DownloadImages();

                using (var context = new SlutPriserEntities()) {
                    var prop = new Properties() {
                        AreaId = areaId,
                        Area = property.Area,
                        BuildYear = property.BuildYear,
                        Rent = property.Rent,
                        Rooms = property.Rooms,
                        Type = property.Type,
                        OperatingCost = property.OperatingCost,
                        ListingPrice = property.ListingPrice,
                        Address = property.Address,
                        Key = property.GetHashCode(),
                        ListingPricePerArea = property.ListingPricePerArea,
                        Images = images

                    };
                    
                    context.Properties.AddObject(prop);

                    //foreach (var image in images)
                    //{
                    //    context.AddToImages(image);
                    //}
                    context.SaveChanges();
                }
            }

        }

        private static Broker GetClassForBroker(string brokerUrl)
        {
            brokerUrl = brokerUrl.ToLower();
            if (brokerUrl.Contains("mohv"))
            {
                return new Broker(brokerUrl, ".ObjectView img.ObjectImg", "MOHV");
            }
            else if (brokerUrl.Contains("fastighetsbyran"))
            {
                return new Broker(brokerUrl, ".ff img", "Fastighetsbyran");
            }
            else if (brokerUrl.Contains("bjurfors"))
            {
                return new Broker(brokerUrl, ".wall-item img", "Bjurfors");
            }
            else if (brokerUrl.Contains("peterlandgren"))
            {
                return new LandgrenBroker(brokerUrl, "PeterLandgren");
            }
            else if (brokerUrl.Contains("hemnet"))
            {
                return new Broker(brokerUrl, "img", "Hemnet");
            }
            else if (brokerUrl.Contains("bo-laget"))
            {
                return new Broker(brokerUrl, "#main .photo img", "BoLaget");
            }

            return new Broker(brokerUrl, "Unknown");
        }

        private static string GetArea(string areaString)
        {
            return areaString.Replace(" ", "");
        }

        private static double ParseDouble(string str)
        {
            var dec = HttpUtility.HtmlDecode(str);

            var numbers = "";
            foreach (var chr in dec)
            {
                if (char.IsDigit(chr))
                {
                    numbers += chr;
                }if (chr == ','){
                    numbers += chr;
                }
            }
            return double.Parse(numbers);
        }

        private static int ParseInt(string str) {

            var dec = HttpUtility.HtmlDecode(str);
                
            var numbers = "";
            foreach (var chr in dec)
            {
                if (char.IsDigit(chr))
                {
                    numbers += chr;
                }
            }
            return int.Parse(numbers);
        }

        private static string GetSelectorForBroker(string brokerUrl) {
            brokerUrl = brokerUrl.ToLower();
            if (brokerUrl.Contains("mohv")) {
                return ".ObjectView img.ObjectImg";
            }else if (brokerUrl.Contains("fastighetsbyran")) {
                return ".ff img";
            }else if (brokerUrl.Contains("bjurfors")) {
                return ".wall-item img";
            }else if (brokerUrl.Contains("peterlandgren")) {
                return ".Highres .img img";
            }

            return "";
        }

        private static string GetImageLinkForBroker(IDomObject dom, string brokerUrl)
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

        public FormUrlEncodedContent GetFormContent()
        {
            var h = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("search[location_search]", "[{'id':473992,'name':'Ribersborg','parent_id':898715,'parent_name':'Malmö'},{'id':898493,'name':'Slottsstaden','parent_id':898715,'parent_name':'Malmö'}]")
            });

            return h;
        }

        public class Property
        {
            public string Type { get; set; }
            public double Area { get; set; }
            public int Rooms { get; set; }
            public int Rent { get; set; }
            public int OperatingCost { get; set; }
            public int ListingPrice { get; set; }
            public int ListingPricePerArea { get; set; }
            public int FinalPrice { get; set; }
            public int BuildYear { get; set; }
            public string Address { get; set; }
           
            public string GetHashCode() {
                return this.Address.GetHashCode() + this.Rooms.GetHashCode() + this.Rent.GetHashCode() + this.Area.GetHashCode() + this.BuildYear.GetHashCode() + "";
            }
        }

    }
}
﻿using System;
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
using System.Diagnostics;
using SlutPriser.Helpers;
using SlutPriser.Models;

namespace SlutPriser
{
    class Program
    {
        static List<CQ> GetListItems()
        {
            List<CQ> cqItems = new List<CQ>();
            string nextPage = "/resultat";
            int i = 0;
            do
            {
                string domString;
                WebRequest wr = WebRequest.Create("http://www.hemnet.se" + nextPage);
                wr.Headers.Add("Cookie", "cx_profile_timeout=1434149239496; cx_profile_data=%7B%22CxenseSegments%22%3A%5B%228kack5xybxki%22%5D%7D; results%2Fh3%2Fresults%2Fresult_settings%2Fversion=2; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fsorting=sale_date+desc; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fsorting_history=sale_date; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fper_page=20; member_type=0; cX_S=iau5y67664o1kmks; last_search=BAhJIgLgAUJBaDdDRG9SYkc5allYUnBiMjVmYVdSeld3ZHBBNGc3QjJrRHZiVU5PaFJzYjJOaGRHbHZibDl6WldGeQpZMmhKSWdHYlczc2lhV1FpT2pRM016azVNaXdpYm1GdFpTSTZJbEpwWW1WeWMySnZjbWNpTENKd1lYSmwKYm5SZmFXUWlPamc1T0RjeE5Td2ljR0Z5Wlc1MFgyNWhiV1VpT2lKTllXeHR3N1lpZlN4N0ltbGtJam80Ck9UZzBPVE1zSW01aGJXVWlPaUpUYkc5MGRITnpkR0ZrWlc0aUxDSndZWEpsYm5SZmFXUWlPamc1T0RjeApOU3dpY0dGeVpXNTBYMjVoYldVaU9pSk5ZV3h0dzdZaWZWMEdPZ1pGVkRvYWJHOWpZWFJwYjI1ZmMyVmgKY21Ob1gyRnljbUY1V3dkN0NUb0hhV1JwQTRnN0J6b0pibUZ0WlVraUQxSnBZbVZ5YzJKdmNtY0dPd2RVCk9nNXdZWEpsYm5SZmFXUnBBNXUyRFRvUWNHRnlaVzUwWDI1aGJXVkpJZ3ROWVd4dHc3WUdPd2RVZXdrNwpDV2tEdmJVTk93cEpJaEZUYkc5MGRITnpkR0ZrWlc0R093ZFVPd3RwQTV1MkRUc01RQXM9CgY6BkVG--59b7749f7f192587a2a93586cebd89d01a304bf4; last_search_info=Ribersborg%2C%2520Malm%25C3%25B6%3B%2520Slottsstaden%2C%2520Malm%25C3%25B6; results%2Fh3%2Fresults%2Fresult_settings%2Flist=collapsed; results%2Fh3%2Fresults%2Fresult_settings%2Fsorting=creation+desc; results%2Fh3%2Fresults%2Fresult_settings%2Fsorting_history=creation; results%2Fh3%2Fresults%2Fresult_settings%2Fper_page=50; _ga=GA1.2.1324110632.1431890058; _gat=1; _cX_segmentTime=1434152029; cX_P=i9sudc8hizip46sy; cx_profile_timeout=1434152029099; _cX_segmentIds=8kack5xybxki; cx_profile_data=%7B%22CxenseSegments%22%3A%5B%228kack5xybxki%22%5D%7D; OAS_SC1=1434152029728; ki_t=1431890059151%3B1434146755742%3B1434152029919%3B16%3B207; ki_r=; _hemnet_session_id=d93a37c07fd7b9cc107434f342bc5a58");
                var h = wr.GetResponse();
                using (var stream = h.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    domString = reader.ReadToEnd();
                }
                CQ dom = domString;
                CQ items = dom["#search-results .item-link-container"];
                cqItems.Add(items);
                nextPage = dom[".next_page"].Length > 0 ? dom[".next_page"][0].GetAttribute("href") : null;
            } while (!string.IsNullOrEmpty(nextPage) && i++ < int.MaxValue);

            return cqItems;
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
            //FinalPriceImporter.Run();
            //return;
            string domString;

            //Get List Items
            var cqList = GetListItems();
            using (var context = new SlutPriserEntities())
            {
                foreach (var propertyList in cqList)
                {
                    foreach (var item in propertyList)
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
                        CQ items2 = dom2[".item.full .attributes"].Children();
                        CQ title = dom2["title"];
                        CQ price = dom2["span.price"];
                        var titleItems = title.First().Text().Split('-');

                        var areaString = PropertyHelper.GetArea(titleItems[2].Split(',')[0].Trim()).Split('/');
                        var area1 = areaString[0];
                        var area2 = areaString.Length > 1 ? areaString[1] : null;
                        var cityName = titleItems[2].Split(',')[1].Split('.')[0].Trim();

                        Property property = new Property(); ;
                        property.Address = titleItems.ElementAt(0);
                        property.ListingPrice = PropertyHelper.ParseInt(price.Elements.First().InnerText);
                        property.City = cityName;
                        property.Area1 = area1;
                        property.Area2 = area2;
                        for (int i = 0; i < items2.Length; i += 2)
                        {
                            var dataType = HttpUtility.HtmlDecode(items2[i].InnerText.Trim());
                            var data = HttpUtility.HtmlDecode(items2[i+1].InnerText.Trim());

                            switch(dataType) {
                                case "Bostadstyp":
                                    property.Type = data;
                                    break;
                                case "Boarea":
                                    property.Area = PropertyHelper.ParseDouble(data);
                                    break;
                                case "Biarea":
                                    property.BiArea = PropertyHelper.ParseDouble(data);
                                    break;
                                case "Tomtarea":
                                    property.PlotArea = PropertyHelper.ParseDouble(data);
                                    break;
                                case "Antal rum":
                                    property.Rooms = PropertyHelper.ParseInt(data);
                                    break;
                                case "Driftkostnad":
                                    property.OperatingCost = PropertyHelper.ParseInt(data);
                                    break;
                                case "Byggår":
                                    property.BuildYear = PropertyHelper.ParseInt(data, 4);
                                    break;
                                case "Avgift/månad":
                                    property.Rent = PropertyHelper.ParseInt(data);
                                    break;
                                case "Pris/m²":
                                    property.ListingPricePerArea = PropertyHelper.ParseInt(data);
                                    break;
                                default:
                                    Debug.WriteLine("Property: '" + dataType + "' not found");
                                    break;
                            }
                            if (dataType == "Bostadstyp")
                            {
                                
                            }
                        }

                        string hash = property.GetPropertyKey();
                        if (!context.Properties.Any(x => x.Key == hash))
                        {
                            CQ moreImagesButton = dom2[".button.image-list"];
                            var moreImagesLink = moreImagesButton.ElementAt(0).GetAttribute("href");

                            Broker broker = GetClassForBroker(moreImagesLink);
                            var images = broker.DownloadImages(property.Address.Trim(), property.GetPropertyKey());

                            var prop = new Properties()
                            {
                                Area1 = property.Area1,
                                Area2 = property.Area2,
                                Area = property.Area,
                                City = property.City,
                                BuildYear = property.BuildYear,
                                Rent = property.Rent,
                                Rooms = property.Rooms,
                                Type = property.Type,
                                OperatingCost = property.OperatingCost,
                                ListingPrice = property.ListingPrice,
                                Address = property.Address,
                                Key = property.GetPropertyKey(),
                                ListingPricePerArea = property.ListingPricePerArea,
                                Images = images,
                                Broker = broker.BrokerName,
                                BiArea = property.BiArea,
                                PlotArea = property.PlotArea
                            };
                            context.Properties.AddObject(prop);
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        private static Broker GetClassForBroker(string brokerUrl)
        {
            if (brokerUrl.IndexOf("mohv", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".ObjectView img.ObjectImg", "MOHV");
            }
            else if (brokerUrl.IndexOf("fastighetsbyran", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".ff img", "Fastighetsbyran");
            }
            else if (brokerUrl.IndexOf("bjurfors", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".wall-item img", "Bjurfors");
            }
            else if (brokerUrl.IndexOf("peterlandgren", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new LandgrenBroker(brokerUrl, "PeterLandgren");
            }
            else if (brokerUrl.IndexOf("hemnet.sfd.se", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, "img", "Hemnet");
            }
            else if (brokerUrl.IndexOf("bo-laget", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, "#main .photo img", "BoLaget");
            }
            else if (brokerUrl.IndexOf("svenskfast", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".object-images__gallery .img-container", "SvenskFast");
            }
            else if (brokerUrl.IndexOf("vaningen", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".objectItemImg img", "Vaningen");
            }
            else if (brokerUrl.IndexOf("erikolsson", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".container-fluid img", "ErikOlsson");
            }
            else if (brokerUrl.IndexOf("fasad", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, "#content img.imagelist", "boHem");
            }
            else if (brokerUrl.IndexOf("skandiamaklarna", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".object-images .object-image-container img", "SkandiaMaklarna");
            }
            else if (brokerUrl.IndexOf("skeppsholmen", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".image img", "Skeppsholmen");
            }
            else if (brokerUrl.IndexOf("bulowlind", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".images img", "BulowLind");
            }
            else if (brokerUrl.IndexOf("highestate.se", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".thumbnail-chapter img", "Hemverket");
            }
            else if (brokerUrl.IndexOf("erasweden", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, "section.images .lazyimage div", "EraSweden");
            }
            else if (brokerUrl.IndexOf("riksmaklaren", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new Broker(brokerUrl, ".holder div div div img", "RiksMäklaren");
            }
            Debug.WriteLine("Unknown broker: " + brokerUrl);
            return new Broker(brokerUrl, "Unknown");
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


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlutPriser.Models;
using SlutPriser;
using CsQuery;
using System.Net;
using System.IO;
using SlutPriser.Helpers;
using System.Web;

namespace SlutPriser
{
    public class FinalPriceImporter
    {
        public static string GetCityForProperty(string url)
        {
            string domString;
            WebRequest wr = WebRequest.Create("http://www.hemnet.se/salda/" + url);
            var h = wr.GetResponse();
            using (var stream = h.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                domString = reader.ReadToEnd();
            }
            CQ dom = domString;
            return HttpUtility.HtmlDecode(dom[".sold-property-details .area"][0].InnerText.Split(',')[1].Trim());
        }

        static List<CQ> GetListItems()
        {
            List<CQ> cqItems = new List<CQ>();
            string nextPage = "/salda/resultat";
            int i = 0;

            do
            {
                string domString;
                WebRequest wr = WebRequest.Create("http://www.hemnet.se" + nextPage);
                wr.Headers.Add("Cookie", "cx_profile_timeout=1434208869641; cx_profile_data=%7B%22CxenseSegments%22%3A%5B%228kack5xybxki%22%5D%7D; results%2Fh3%2Fresults%2Fresult_settings%2Fversion=2; member_type=0; last_search=BAhJIgLgAUJBaDdDRG9SYkc5allYUnBiMjVmYVdSeld3ZHBBNGc3QjJrRHZiVU5PaFJzYjJOaGRHbHZibDl6WldGeQpZMmhKSWdHYlczc2lhV1FpT2pRM016azVNaXdpYm1GdFpTSTZJbEpwWW1WeWMySnZjbWNpTENKd1lYSmwKYm5SZmFXUWlPamc1T0RjeE5Td2ljR0Z5Wlc1MFgyNWhiV1VpT2lKTllXeHR3N1lpZlN4N0ltbGtJam80Ck9UZzBPVE1zSW01aGJXVWlPaUpUYkc5MGRITnpkR0ZrWlc0aUxDSndZWEpsYm5SZmFXUWlPamc1T0RjeApOU3dpY0dGeVpXNTBYMjVoYldVaU9pSk5ZV3h0dzdZaWZWMEdPZ1pGVkRvYWJHOWpZWFJwYjI1ZmMyVmgKY21Ob1gyRnljbUY1V3dkN0NUb0hhV1JwQTRnN0J6b0pibUZ0WlVraUQxSnBZbVZ5YzJKdmNtY0dPd2RVCk9nNXdZWEpsYm5SZmFXUnBBNXUyRFRvUWNHRnlaVzUwWDI1aGJXVkpJZ3ROWVd4dHc3WUdPd2RVZXdrNwpDV2tEdmJVTk93cEpJaEZUYkc5MGRITnpkR0ZrWlc0R093ZFVPd3RwQTV1MkRUc01RQXM9CgY6BkVG--59b7749f7f192587a2a93586cebd89d01a304bf4; last_search_info=Ribersborg%2C%2520Malm%25C3%25B6%3B%2520Slottsstaden%2C%2520Malm%25C3%25B6; results%2Fh3%2Fresults%2Fresult_settings%2Flist=collapsed; results%2Fh3%2Fresults%2Fresult_settings%2Fsorting=creation+desc; results%2Fh3%2Fresults%2Fresult_settings%2Fsorting_history=creation; results%2Fh3%2Fresults%2Fresult_settings%2Fper_page=50; cX_S=iauuf4xtuep9txdz; cx_profile_timeout=1434187857927; cx_profile_data=%7B%22CxenseSegments%22%3A%5B%228kack5xybxki%22%5D%7D; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fsorting=sale_date+desc; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fsorting_history=sale_date; results%2Fh3%2Fsold%2Fresults%2Fresult_settings%2Fper_page=20; _hemnet_session_id=d93a37c07fd7b9cc107434f342bc5a58; _ga=GA1.2.1324110632.1431890058; OAS_SC1=1434208869049; _cX_segmentTime=1434208870; cX_P=i9sudc8hizip46sy; ki_t=1431890059151%3B1434208331804%3B1434208869799%3B17%3B228; ki_r=; _cX_segmentIds=8kack5xybxki");
                var h = wr.GetResponse();
                using (var stream = h.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    domString = reader.ReadToEnd();
                }
                CQ dom = domString;
                cqItems.Add(dom["#search-results div.item.sold.result"]);

                nextPage = dom[".next_page"][0].GetAttribute("href");
            } while (!string.IsNullOrEmpty(nextPage) && i++ < 20);

            return cqItems;
        }

        static public List<Property> GetPropertyInformation(List<CQ> cqList)
        {
            var properties = new List<Property>();

            foreach (var propertyList in cqList)
            {
                foreach (var item in propertyList)
                {
                    CQ dom = item.InnerHTML;
                    var property = new Property()
                    {
                        SellingDate = PropertyHelper.ParseDate(dom[".sold-date"][0].InnerText),
                        ListingPricePerArea = PropertyHelper.ParseInt(dom[".price-per-m2"][0].InnerText),
                        FinalPrice = PropertyHelper.ParseInt(dom[".price .item-link"][0].InnerText),
                        Rent = PropertyHelper.ParseInt(dom[".fee"][0].InnerText),
                        ListingPrice = PropertyHelper.ParseInt(dom[".asked-price"][0].InnerText),
                        Address = HttpUtility.HtmlDecode(dom[".address .primary-text"][0].InnerText),
                        Area1 = HttpUtility.HtmlDecode(dom[".area .item-link"][0].InnerText.Trim()),
                        City = HttpUtility.HtmlDecode(dom[".city"][0].InnerText.Trim()),
                        Broker = HttpUtility.HtmlDecode(dom[".broker a"][0].InnerText.Trim()),
                        Area = PropertyHelper.ParseInt(dom[".living-area .item-link"][0].InnerText.Split(new string[] { "&nbsp;" }, StringSplitOptions.None)[0]),
                        Rooms = PropertyHelper.ParseInt(dom[".living-area .item-link"][0].InnerText.Split(new string[] { "&nbsp;" }, StringSplitOptions.None)[1]),
                    };

                    if (string.IsNullOrEmpty(property.City))
                    {
                        property.City = GetCityForProperty(dom[".item-link-container"][0].GetAttribute("href"));
                    }

                    properties.Add(property);
                }
            }

            return properties;
        }

        public static void Run()
        {
            Console.Write("Starting Import of Sold Properties");
            var items = GetListItems();
            var properties = GetPropertyInformation(items);

            foreach (var property in properties)
            {
                using (var context = new SlutPriserEntities()) {
                    var finalPriceProperty = property.ToSoldProperties();
                    if (!context.SoldProperties.Any(x=> x.Key == finalPriceProperty.Key))
                    {
                        context.AddToSoldProperties(finalPriceProperty);
                    }
                    context.SaveChanges(); //TODO: Move outside foreach for performance
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;

namespace SlutPriser.Helpers
{
    public class PropertyHelper
    {
        public static string GetArea(string areaString)
        {
            return areaString.Replace(" ", "");
        }

        public static double ParseDouble(string str)
        {
            var dec = HttpUtility.HtmlDecode(str);

            var numbers = "";
            foreach (var chr in dec)
            {
                if (char.IsDigit(chr))
                {
                    numbers += chr;
                } if (chr == ',')
                {
                    numbers += chr;
                }
            }
            return double.Parse(numbers);
        }

        public static int ParseInt(string str, int maxDigits = int.MaxValue)
        {

            var dec = HttpUtility.HtmlDecode(str);

            var numbers = "";
            foreach (var chr in dec)
            {
                if (char.IsDigit(chr))
                {
                    numbers += chr;
                }
                if (numbers.Length >= maxDigits)
                {
                    break;
                }
            }
            return int.Parse(numbers);
        }

        public static string Sha256(string password)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(password), 0, Encoding.ASCII.GetByteCount(password));
            foreach (byte bit in crypto)
            {
                hash += bit.ToString("x2");
            }
            return hash;
        }

        public static string GetSelectorForBroker(string brokerUrl)
        {
            brokerUrl = brokerUrl.ToLower();
            if (brokerUrl.Contains("mohv"))
            {
                return ".ObjectView img.ObjectImg";
            }
            else if (brokerUrl.Contains("fastighetsbyran"))
            {
                return ".ff img";
            }
            else if (brokerUrl.Contains("bjurfors"))
            {
                return ".wall-item img";
            }
            else if (brokerUrl.Contains("peterlandgren"))
            {
                return ".Highres .img img";
            }

            return "";
        }

        public static DateTime ParseDate(string str, int maxDigits = int.MaxValue)
        {

            var dec = HttpUtility.HtmlDecode(str);

            var numbers = "";
            foreach (var chr in dec)
            {
                if (char.IsDigit(chr) || chr.Equals('-'))
                {
                    numbers += chr;
                }
                if (numbers.Length >= maxDigits)
                {
                    break;
                }
            }
            return DateTime.Parse(numbers);
        }


    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlutPriser;

namespace SlutPriserTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //string imagesLink = "http://www.peterlandgren.se/objekt/bilder.html?470509";
            string imagesLink = "http://www.mohv.se/till-salu/bostad.aspx?gid=OBJ12441_1375093482&ViewAllImages=1";
        
            //var broker = new LandgrenBroker(imagesLink, "PeterLandgren");
            var broker = new Broker(imagesLink,".ObjectView img.ObjectImg", "MOHV");

           var images = broker.DownloadImages("adress");
        }
    }
}

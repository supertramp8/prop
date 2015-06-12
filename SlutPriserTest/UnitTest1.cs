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
            string imagesLink = "http://www.peterlandgren.se/objekt/bilder.html?470509";
            var broker = new LandgrenBroker(imagesLink, "PeterLandgren");

           var images = broker.DownloadImages();
        }
    }
}

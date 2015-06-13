using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlutPriser.Models;
using SlutPriser;

namespace SlutPriser
{
    class FinalPriceImporter
    {
        public static void Run()
        {

            List<Property> properties = new List<Property>();

            foreach (var property in properties)
            {
                using (var context = new SlutPriserEntities()) {
                    var finalPriceProperty = property.ToSoldProperties();
                    context.AddToSoldProperties(finalPriceProperty);
                    context.SaveChanges(); //TODO: Move outside foreach for performance
                }
            }
        }
    }
}

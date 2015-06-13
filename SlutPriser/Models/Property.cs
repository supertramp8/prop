using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlutPriser.Models
{
    public class Property
    {
        public string Type { get; set; }
        public double Area { get; set; }
        public string Area1 { get; set; }
        public string Area2 { get; set; }
        public string City { get; set; }
        public DateTime? SellingDate { get; set; }
        public int Rooms { get; set; }
        public int Rent { get; set; }
        public int OperatingCost { get; set; }
        public int ListingPrice { get; set; }
        public int ListingPricePerArea { get; set; }
        public int FinalPrice { get; set; }
        public int BuildYear { get; set; }
        public string Address { get; set; }
        public string Broker { get; set; }

        public string GetHashCode()
        {
            return this.Address.GetHashCode() + this.Rooms.GetHashCode() + this.Rent.GetHashCode() + this.Area.GetHashCode() + this.BuildYear.GetHashCode() + +this.ListingPrice.GetHashCode() + "";
        }

        public SoldProperties ToSoldProperties() {
            return new SoldProperties()
                    {
                        Address = this.Address,
                        Area = this.Area,
                        Area1 = this.Area1,
                        Area2 = this.Area2,
                        Broker = this.Broker,
                        BuildYear = this.BuildYear,
                        City = this.City,
                        FinalPrice = this.FinalPrice,
                        Key = this.GetHashCode(),
                        ListingPricePerArea = this.ListingPricePerArea,
                        OperatingCost = this.OperatingCost,
                        Rent = this.Rent,
                        Rooms = this.Rooms,
                        SellingDate = this.SellingDate,
                        Type = this.Type
                    };
        }
    }
}

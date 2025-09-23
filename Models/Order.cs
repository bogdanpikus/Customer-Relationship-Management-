using System;

namespace CRM.Models
{
    public class Order
    {
        public bool IsSelected {  get; set; } = false;
        public DateTime OrderDate {  get; set; }
        public string? Articul { get; set; }
        public string? OrderID { get; set; }
        public string CustomerID { get; set; }         // key
        public Customer Customer { get; set; }       // object link Contact
        public string? Item { get; set; }
        public int Amount { get; set; }
        public decimal? Price { get; set; } 
        public decimal? PrimeCost { get; set; }
        public string? PaymentWay { get; set; }
        public string? DelivarWay { get; set; }
        public string? DeliverAdress { get; set; }
        public string? Status { get; set; }
        public decimal? Spending { get; set; }
        public decimal? Income { get; set; }
        public string? Organization { get; set; }
        public string? Comment { get; set; }
    }

    public class Customer
    {
        public string Id { get; set; }
        public string? SecondName { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }

        // its variable
        public int AmountOrders { get; set; }
    }
}

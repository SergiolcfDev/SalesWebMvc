using System;
using System.Collections.Generic;
using System.Linq;


namespace SalesWebMvc.Models
{
    public class Department
    {
        public string Nome { get; set; }
        public int ID { get; set; }
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public Department()
        {
        }
        public Department(string nome, int iD)
        {
            Nome = nome;
            ID = iD;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sellers.Sum(Sellers => Sellers.TotalSales(initial, final));   
        }
    }
}

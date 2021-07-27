using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Models
{
    public class Department
    {
        public string Nome { get; set; }
        public int ID { get; set; }

        public Department()
        {
        }
        public Department(string nome, int iD)
        {
            Nome = nome;
            ID = iD;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ciomag_Andreea_Museum.Models
{
    public class Gallery
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        [DataType(DataType.Time)]
        public DateTime Opening { get; set; }
        [DataType(DataType.Time)]
        public DateTime Closing { get; set; }

        public ICollection<Exhibition> Exhibitions { get; set; }
        public ICollection<Visit> Visits { get; set; }
    }
}


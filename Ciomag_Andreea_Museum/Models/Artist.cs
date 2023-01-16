using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciomag_Andreea_Museum.Models
{
    public class Artist
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }

        public ICollection<Exhibit> Exhibits { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciomag_Andreea_Museum.Models
{
    public class Exhibit
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Movement { get; set; }
        public int ExhibitionID { get; set; }
        public int ArtistID { get; set; }

        public Exhibition Exhibition { get; set; }
        public Artist Artist { get; set; }
    }
}


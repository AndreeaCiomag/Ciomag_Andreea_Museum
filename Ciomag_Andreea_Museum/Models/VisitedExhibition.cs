using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciomag_Andreea_Museum.Models
{
    public class VisitedExhibition
    {
        public int ClientID { get; set; }
        public int ExhibitionID { get; set; }
        public Client Client { get; set; }
        public Exhibition Exhibition { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ciomag_Andreea_Museum.Models
{
    public class Visit
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int GalleryID { get; set; }
        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; }

        public Client Client { get; set; }
        public Gallery Gallery { get; set; }
    }
}


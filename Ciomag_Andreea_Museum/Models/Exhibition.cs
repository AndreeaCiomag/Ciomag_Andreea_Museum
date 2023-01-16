using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ciomag_Andreea_Museum.Models
{
    public class Exhibition
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Theme { get; set; }
        public int GalleryID { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        public Gallery Galleries { get; set; }
        public ICollection<Exhibit> Exhibits { get; set; }
    }
}


using System;
using System.ComponentModel.DataAnnotations;

namespace Ciomag_Andreea_Museum.Models.MuseumViewModels
{
    public class VisitGroup
    {
        [DataType(DataType.Date)]
        public DateTime? VisitDate { get; set; }
        public int ClientsCount { get; set; }
    }
}


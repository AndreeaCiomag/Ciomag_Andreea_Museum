using System;
namespace Ciomag_Andreea_Museum.Models.MuseumViewModels
{
    public class ExhibitionIndexData
    {
        public IEnumerable<Exhibition> Exhibitions { get; set; }
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Gallery> Galleries { get; set; }
    }
}


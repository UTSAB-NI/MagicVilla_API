using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        private static List<VillaDTO> villalist = new List<VillaDTO>{ new VillaDTO { Id=1,Name="Paradise resort",Occupancy=4,sqft=100} ,
            new VillaDTO { Id=2,Name="Fulmaya restro",Occupancy=2,sqft=50}};
        public static List<VillaDTO> VillaList()
        {
           return villalist;
            
        }
    }
}

using AutoMapper;
using MachineVision.Core.Entity;
using MachineVision.Core.Models;


namespace MachineVision.Core.Mapper
{
    public class AppMapper: Profile
    {
        public AppMapper()
        {
            CreateMap<ProductModel, Product>().ReverseMap(); 
        }

      
    }

   
}

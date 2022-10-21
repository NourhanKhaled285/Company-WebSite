using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.Models;

namespace Project.PL.Mappers
{
    public class DepartmentProfile:Profile
    {
        public DepartmentProfile()
        {   
            CreateMap<Department, DepartmentViewModel>().ReverseMap();
        }
    }
}

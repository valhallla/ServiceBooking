using System.Collections.Generic;
using System.Linq;
using Ninject;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using AutoMapper;

namespace ServiceBooking.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        [Inject]
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<CategoryViewModelBLL> GetAll()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Category, CategoryViewModelBLL>());
            return Mapper.Map<List<Category>, List<CategoryViewModelBLL>>(_categoryRepository.GetAll().ToList());
        }

        public CategoryViewModelBLL FindById(int id)
        {
            Category category = _categoryRepository.Get(id);

            if (category != null)
                return new CategoryViewModelBLL { Id = category.Id, Name = category.Name};

            return null;
        }

        public CategoryViewModelBLL FindByName(string name)
        {
            Category category = _categoryRepository.GetAll().FirstOrDefault(c => c.Name.Equals(name));

            if (category != null)
                return new CategoryViewModelBLL { Id = category.Id, Name = category.Name };

            return null;
        }
    }
}

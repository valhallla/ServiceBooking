using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.DAL.Repositories;
using ServiceBooking.DAL.EF;

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

        public IEnumerable<CategoryViewModel> GetAll()
        {
            var categories = _categoryRepository.GetAll().ToArray();
            var categoryViewModels = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                categoryViewModels.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                });
            }
            return categoryViewModels;
        }

        public CategoryViewModel FindById(int id)
        {
            Category category = _categoryRepository.Get(id);

            if (category != null)
                return new CategoryViewModel { Id = category.Id, Name = category.Name};

            return null;
        }

        public CategoryViewModel FindByName(string name)
        {
            Category category = _categoryRepository.GetAll().FirstOrDefault(c => c.Name.Equals(name));

            if (category != null)
                return new CategoryViewModel { Id = category.Id, Name = category.Name };

            return null;
        }
    }
}

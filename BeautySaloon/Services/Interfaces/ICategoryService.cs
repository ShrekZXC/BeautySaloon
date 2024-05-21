using BeautySaloon.Model;

namespace BeautySaloon.Services.Interfaces;

public interface ICategoryService
{
    Task<Guid> Create(CategoryModel categoryModel);

    Task<CategoryModel> Get(Guid categoryId);

    Task Update(CategoryModel categoryModel);

    List<CategoryModel> GetAll();

    Task Delete(Guid categoryId);
}
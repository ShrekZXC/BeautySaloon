using BeautySaloon.Model;

namespace BeautySaloon.Services.Interfaces;

public interface ICategoryService
{
    Task<Guid> Create(CategoryModel categoryModel);

    Task<CategoryModel> Get(Guid categoryId);

    Task<bool> Update(CategoryModel categoryModel);

    Task<List<CategoryModel>> GetAll();

    Task<bool> Delete(Guid categoryId);
}
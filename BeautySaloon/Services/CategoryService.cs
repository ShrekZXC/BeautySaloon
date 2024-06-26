﻿using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class CategoryService : ICategoryService
{
    private readonly IDbRepository _dbRepository;
    private readonly IMapper _mapper;

    public CategoryService(IDbRepository dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    public async Task<Guid> Create(CategoryModel categoryModel)
    {
        var entity = _mapper.Map<CategoryEntity>(categoryModel);
            
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
            
        return result;
    }

    public async Task<CategoryModel> Get(Guid categoryId)
    {
        var entity = await _dbRepository.Get<CategoryEntity>().FirstOrDefaultAsync(x => x.Id == categoryId);
        var categoryModel = _mapper.Map<CategoryModel>(entity);

        return categoryModel;
    }

    public async Task<List<CategoryModel>> GetAll()
    {
        var entities = await  _dbRepository.GetAll<CategoryEntity>().ToListAsync();
        var categoriesModel = _mapper.Map<List<CategoryModel>>(entities).ToList();

        return categoriesModel;
    }

    public async Task<bool> Update(CategoryModel categoryModel)
    {
        try
        {
            var entity = _mapper.Map<CategoryEntity>(categoryModel);
            
            await _dbRepository.Update(entity);
            await _dbRepository.SaveChangesAsync();

            return true;
        }
        catch (System.Exception exception)
        {
            return false;
        }
    }

    public async Task<bool> Delete(Guid categoryId)
    {
        var entity = await _dbRepository.Get<CategoryEntity>()
            .FirstOrDefaultAsync(x => x.Id == categoryId);

        if (entity != null)
        {
            await _dbRepository.Remove<CategoryEntity>(entity);
            await _dbRepository.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }
}
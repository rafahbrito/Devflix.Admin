using Devflix.Admin.Application.Interfaces;
using Devflix.Admin.Domain.Repository;
using DomainEntity = Devflix.Admin.Domain.Entity;

namespace Devflix.Admin.Application.UseCases.Category.CreateCategory;

public class CreateCategoryHandler : ICreateCategoryHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCategoryResponse> Handle(
        CreateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var category = new DomainEntity.Category(
            request.Name,
            request.Description,
            request.IsActive
        );

        await _categoryRepository.Insert(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return new CreateCategoryResponse(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt
        );
    }
}

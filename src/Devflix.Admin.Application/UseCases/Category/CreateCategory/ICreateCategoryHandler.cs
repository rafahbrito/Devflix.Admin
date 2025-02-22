namespace Devflix.Admin.Application.UseCases.Category.CreateCategory;
public interface ICreateCategoryHandler
{
    public Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken);
}

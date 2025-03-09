using MediatR;

namespace Devflix.Admin.Application.UseCases.Category.CreateCategory;
public interface ICreateCategoryHandler : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse> { }

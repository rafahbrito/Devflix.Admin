using MediatR;

namespace Devflix.Admin.Application.UseCases.Category.CreateCategory;

public class CreateCategoryRequest : IRequest<CreateCategoryResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public CreateCategoryRequest(string name, string? description = null, bool isActive = true)
    {
        Name = name;
        Description = description ?? "";
        IsActive = isActive;
    }
}

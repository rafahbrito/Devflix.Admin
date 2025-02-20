using Devflix.Admin.Domain.Exceptions;
using Devflix.Admin.Domain.SeedWork;
using Devflix.Admin.Domain.Validator;

namespace Devflix.Admin.Domain.Entity;
public class Category : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true) : base()
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public void Activate()
    {
        IsActive = true;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }

    public void Update(string name, string? description = null)
    {
        Name = name;
        Description = description ?? Description;

        Validate();
    }

    private void Validate()
    {
        DomainValidator.NotNullOrEmpty(Name, nameof(Name));
        DomainValidator.MinLength(Name, 3, nameof(Name));
        DomainValidator.MaxLength(Name, 255, nameof(Name));

        DomainValidator.NotNull(Description, nameof(Description));
        DomainValidator.MaxLength(Description, 10_000, nameof(Description));
    }
}

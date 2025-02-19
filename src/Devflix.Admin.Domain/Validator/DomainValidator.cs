using Devflix.Admin.Domain.Exceptions;

namespace Devflix.Admin.Domain.Validator;
public class DomainValidator
{
    public static void NotNull(object? target, string fieldName)
    {
        if (target is null)
            throw new EntityValidationException($"{fieldName} should not be null");
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (String.IsNullOrWhiteSpace(target))
            throw new EntityValidationException($"{fieldName} should not be null or empty");
    }
}

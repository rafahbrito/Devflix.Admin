using Bogus;
using Devflix.Admin.Domain.Exceptions;
using Devflix.Admin.Domain.Validator;

using FluentAssertions;

namespace Devflix.Admin.UnitTests.Domain.Validation;
public class DomainValidatorTest
{
    private Faker Faker {  get; set; } = new Faker();

    // Validar null
    [Fact(DisplayName = nameof(ValidateShouldPassWhenValueIsNotNull))]
    [Trait("Domain", "DomainValidation - Validators")]
    public void ValidateShouldPassWhenValueIsNotNull()
    {
        var value = Faker.Commerce.Categories(1)[0];

        Action action = () => DomainValidator.NotNull(value, "FieldName");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(ValidateShouldThrowExceptionWhenValueIsNull))]
    [Trait("Domain", "DomainValidation - Validators")]
    public void ValidateShouldThrowExceptionWhenValueIsNull()
    {
        string? value = null;
        string fieldName = Faker.Database.Column();

        Action action = () => DomainValidator.NotNull(value, fieldName);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage($"{fieldName} should not be null");
    }
    // Validar null ou vazio
    // Validar tamanho min
    // Validar tamanho max
}

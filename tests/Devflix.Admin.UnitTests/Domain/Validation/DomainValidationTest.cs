using Bogus;
using Devflix.Admin.Domain.Validator

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

        Action action = () => DomainValidation.NotNull(value, "Value");

        action.Should().NotThrow();
    }
    // Validar null ou vazio
    // Validar tamanho min
    // Validar tamanho max
}

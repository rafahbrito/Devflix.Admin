using Devflix.Admin.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = Devflix.Admin.Domain.Entity;

namespace Devflix.Admin.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture)
    {
        _categoryTestFixture = categoryTestFixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregate")]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var dateTimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default);
        category.CreatedAt.Should().BeAfter(dateTimeBefore).And.BeBefore(dateTimeAfter);
        category.IsActive.Should().BeTrue();

    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var dateTimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default);
        category.CreatedAt.Should().BeAfter(dateTimeBefore).And.BeBefore(dateTimeAfter);
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateShouldThrowExceptionWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void InstantiateShouldThrowExceptionWhenNameIsNullOrEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(name, validCategory.Description);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateShouldThrowExceptionWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregate")]
    public void InstantiateShouldThrowExceptionWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(validCategory.Name, null);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Description should not be null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData("A")]
    [InlineData("Ab")]
    [InlineData("0")]
    [InlineData("12")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(name, validCategory.Description);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregate")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var name = String.Join(null, Enumerable.Range(0, 130).Select(_ => "ab").ToArray()); //TODO: Melhorar lógica desse teste

        Action action = () => new DomainEntity.Category(name, validCategory.Description);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregate")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var description = String.Join(null, Enumerable.Range(0, 5001).Select(_ => "ab").ToArray()); //TODO: Melhorar lógica desse teste

        Action action = () => new DomainEntity.Category(validCategory.Name, description);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Description should be less or equal 10.000 characters long");
    }

    [Fact(DisplayName = nameof(ActivateShouldChangeIsActiveToTrue))]
    [Trait("Domain", "Category - Aggregate")]
    public void ActivateShouldChangeIsActiveToTrue()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);

        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(DeactivateShouldChangeIsActiveToFalse))]
    [Trait("Domain", "Category - Aggregate")]
    public void DeactivateShouldChangeIsActiveToFalse()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);

        category.Deactivate();

        category.IsActive.Should().BeFalse();

    }

    [Fact(DisplayName = nameof(UpdateShouldModifyCategoryProperties))]
    [Trait("Domain", "Category - Aggregate")]
    public void UpdateShouldModifyCategoryProperties()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var newValues = new { Name = "New category name", Description = "New category description" };

        category.Update(newValues.Name, newValues.Description);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateShouldChangeOnlyName))]
    [Trait("Domain", "Category - Aggregate")]
    public void UpdateShouldChangeOnlyName()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var currentDescription = category.Description;
        var newValue = new { Name = "New category name" };

        category.Update(newValue.Name);

        category.Name.Should().Be(newValue.Name);
        category.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateShouldThrowExceptionWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void UpdateShouldThrowExceptionWhenNameIsNullOrEmpty(string? name)
    {
        var category = _categoryTestFixture.GetValidCategory();

        Action action = () => category.Update(name);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData("A")]
    [InlineData("Ab")]
    [InlineData("0")]
    [InlineData("12")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string name)
    {
        var category = _categoryTestFixture.GetValidCategory();

        Action action = () => category.Update(name);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregate")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();

        var newName = String.Join(null, Enumerable.Range(0, 130).Select(_ => "ab").ToArray()); //TODO: Melhorar lógica desse teste

        Action action = () => category.Update(newName);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregate")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();

        var description = String.Join(null, Enumerable.Range(0, 5001).Select(_ => "ab").ToArray()); //TODO: Melhorar lógica desse teste

        Action action = () => category.Update("Category name", description);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Description should be less or equal 10.000 characters long");
    }
}

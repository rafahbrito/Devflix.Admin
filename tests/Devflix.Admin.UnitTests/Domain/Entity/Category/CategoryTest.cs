using Devflix.Admin.Domain.Exceptions;
using DomainEntity = Devflix.Admin.Domain.Entity;

namespace Devflix.Admin.UnitTests.Domain.Entity.Category;
public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregate")]
    public void Instantiate()
    {
        var validData = new
        {
            Name = "Category name",
            Description = "Category description",
        };

        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default, category.Id);
        Assert.NotEqual(default, category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = new
        {
            Name = "Category name",
            Description = "Category description",
        };

        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var dateTimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default, category.Id);
        Assert.NotEqual(default, category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateShouldThrowExceptionWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void InstantiateShouldThrowExceptionWhenNameIsNullOrEmpty(string? name)
    {
        Action action = () => new DomainEntity.Category(name, "Category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateShouldThrowExceptionWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregate")]
    public void InstantiateShouldThrowExceptionWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("Category name", null);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData("A")]
    [InlineData("Ab")]
    [InlineData("0")]
    [InlineData("12")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string name)
    {
        Action action = () => new DomainEntity.Category(name, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at leats 3 characters long", exception.Message);

    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregate")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var name = String.Join(null, Enumerable.Range(0, 130).Select(_ => "ab").ToArray()); //TODO: Melhorar lógica desse teste

        Action action = () => new DomainEntity.Category(name, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregate")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var description = String.Join(null, Enumerable.Range(0, 5001).Select(_ => "ab").ToArray()); //TODO: Melhorar lógica desse teste

        Action action = () => new DomainEntity.Category("Category name", description);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(ActivateShouldChangeIsActiveToTrue))]
    [Trait("Domain", "Category - Aggregate")]
    public void ActivateShouldChangeIsActiveToTrue()
    {
        var validData = new
        {
            Name = "Category name",
            Description = "Category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, false);

        category.Activate();

        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(DeactivateShouldChangeIsActiveToFalse))]
    [Trait("Domain", "Category - Aggregate")]
    public void DeactivateShouldChangeIsActiveToFalse()
    {
        var validData = new
        {
            Name = "Category name",
            Description = "Category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);

        category.Deactivate();

        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(UpdateShouldModifyCategoryProperties))]
    [Trait("Domain", "Category - Aggregate")]
    public void UpdateShouldModifyCategoryProperties()
    {
        var category = new DomainEntity.Category("Old category name", "Old category description");
        var newValues = new { Name = "New category name", Description = "New category description" };

        category.Update(newValues.Name, newValues.Description);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }

    [Fact(DisplayName = nameof(UpdateShouldChangeOnlyName))]
    [Trait("Domain", "Category - Aggregate")]
    public void UpdateShouldChangeOnlyName()
    {
        var category = new DomainEntity.Category("Old category name", "Old category description");
        var currentDescription = category.Description;
        var newValue = new { Name = "New category name" };

        category.Update(newValue.Name);

        Assert.Equal(newValue.Name, category.Name);
        Assert.Equal(currentDescription, category.Description);
    }

    [Theory(DisplayName = nameof(UpdateShouldThrowExceptionWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void UpdateShouldThrowExceptionWhenNameIsNullOrEmpty(string? name)
    {
        var category = new DomainEntity.Category("Old category name", "Old category description");

        Action action = () => category.Update(name);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }
}

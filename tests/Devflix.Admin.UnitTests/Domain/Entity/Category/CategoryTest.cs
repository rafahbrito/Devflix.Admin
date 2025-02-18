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
}

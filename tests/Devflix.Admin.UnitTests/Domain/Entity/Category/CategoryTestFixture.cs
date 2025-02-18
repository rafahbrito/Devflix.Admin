using DomainEntity = Devflix.Admin.Domain.Entity;

namespace Devflix.Admin.UnitTests.Domain.Entity.Category;
public class CategoryTestFixture
{
    public DomainEntity.Category GetValidCategory()
    {
        return new("Category name", "Category description");
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureColletion : ICollectionFixture<CategoryTestFixture> { }

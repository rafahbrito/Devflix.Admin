using Devflix.Admin.Application.Interfaces;
using Devflix.Admin.Application.UseCases.Category.CreateCategory;
using Devflix.Admin.Domain.Repository;
using Devflix.Admin.UnitTests.Common;
using Moq;

namespace Devflix.Admin.UnitTests.Application.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }

public class CreateCategoryTestFixture : BaseFixture
{
    private readonly Random _random = new();

    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }

    public bool GetRandomBoolean() => _random.Next(2) == 0;

    public CreateCategoryRequest GetValidRequest() =>
        new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());

    public CreateCategoryRequest GetInvalidRequestWithNullName() =>
        new(null, GetValidCategoryDescription(), GetRandomBoolean());

    public CreateCategoryRequest GetInvalidRequestWithShortName()
    {
        string name = Faker.Commerce.Categories(1)[0];
        string invalidName = name[..2];
        return new(invalidName, GetValidCategoryDescription(), GetRandomBoolean());
    }

    public CreateCategoryRequest GetInvalidRequestWithLongName()
    {
        string longName = Faker.Lorem.Letter(256);
        return new(longName, GetValidCategoryDescription(), GetRandomBoolean());
    }

    public CreateCategoryRequest GetInvalidRequestWithLongDescription()
    {
        string longDescription = Faker.Lorem.Paragraphs(1_001);
        return new(GetValidCategoryName(), longDescription, GetRandomBoolean());
    }

    public Mock<ICategoryRepository> GetRepositoryMock() => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
}

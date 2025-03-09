using Devflix.Admin.Application.UseCases.Category.CreateCategory;
using Devflix.Admin.Domain.Exceptions;
using FluentAssertions;
using Moq;
using DomainEntity = Devflix.Admin.Domain.Entity;
using UseCases = Devflix.Admin.Application.UseCases.Category.CreateCategory;

namespace Devflix.Admin.UnitTests.Application.Category.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - UseCases")]
    public async Task CreateCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategoryHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = _fixture.GetValidRequest();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository =>
                repository.Insert(It.IsAny<DomainEntity.Category>(), It.IsAny<CancellationToken>()),
            Times.Once
        );

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(CreateCategoryShouldThrowExceptionWhenRequestIsInvalid))]
    [Trait("Application", "CreateCategory - UseCases")]
    [MemberData(
        nameof(CreateCategoryTestDataGenerator.GetInvalidRequests),
        parameters: 12,
        MemberType = typeof(CreateCategoryTestDataGenerator)
    )]
    public async Task CreateCategoryShouldThrowExceptionWhenRequestIsInvalid(
        CreateCategoryRequest input,
        string exceptionMessage
    )
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategoryHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
    }

    [Fact(DisplayName = nameof(CreateCategoryShouldSetEmptyDescriptionWhenNotProvided))]
    [Trait("Application", "CreateCategory - UseCases")]
    public async Task CreateCategoryShouldSetEmptyDescriptionWhenNotProvided()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategoryHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var validName = _fixture.GetValidCategoryName();
        var input = new CreateCategoryRequest(validName);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository =>
                repository.Insert(It.IsAny<DomainEntity.Category>(), It.IsAny<CancellationToken>()),
            Times.Once
        );

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(String.Empty);
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
}

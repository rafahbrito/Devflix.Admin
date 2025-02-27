﻿using Devflix.Admin.Application.UseCases.Category.CreateCategory;
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

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidRequestsList = new List<object[]>();

        var nameNullRequest = fixture.GetValidRequest();
        nameNullRequest.Name = null;
        invalidRequestsList.Add([nameNullRequest, "Name should not be empty or null"]);

        var shortNameRequest = fixture.GetValidRequest();
        shortNameRequest.Name = shortNameRequest.Name.Substring(0,2);
        invalidRequestsList.Add([shortNameRequest, "Name should be at leats 3 characters long"]);

        var longName = fixture.Faker.Lorem.Letter(256);
        var tooLongNameRequest = fixture.GetValidRequest();
        tooLongNameRequest.Name = longName;
        invalidRequestsList.Add([tooLongNameRequest, "Name should be less or equal 255 characters long"]);

        var longDescription = fixture.Faker.Lorem.Paragraphs(1_001);
        var longDescriptionRequest = fixture.GetValidRequest();
        longDescriptionRequest.Description = longDescription;
        invalidRequestsList.Add([longDescriptionRequest, "Description should be less or equal 10000 characters long"]);


        return invalidRequestsList;
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
            repository => repository.Insert(
                It.IsAny<DomainEntity.Category>(),
                It.IsAny<CancellationToken>()
            ),
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
    [MemberData(nameof(GetInvalidRequests))]
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

        await task.Should()
                  .ThrowAsync<EntityValidationException>()
                  .WithMessage(exceptionMessage);
    }
}

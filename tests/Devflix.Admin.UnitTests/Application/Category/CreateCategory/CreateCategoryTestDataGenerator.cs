namespace Devflix.Admin.UnitTests.Application.Category.CreateCategory;

public class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidRequests(int times)
    {
        var totalInvalidCases = 4;
        var fixture = new CreateCategoryTestFixture();
        var invalidRequestsList = new List<object[]>();

        for (int i = 0; i < times; i++)
        {
            switch (i % totalInvalidCases)
            {
                case 0:
                    invalidRequestsList.Add(
                        [
                            fixture.GetInvalidRequestWithNullName(),
                            "Name should not be empty or null",
                        ]
                    );
                    break;
                case 1:
                    invalidRequestsList.Add(
                        [
                            fixture.GetInvalidRequestWithShortName(),
                            "Name should be at leats 3 characters long",
                        ]
                    );
                    break;
                case 2:
                    invalidRequestsList.Add(
                        [
                            fixture.GetInvalidRequestWithLongName(),
                            "Name should be less or equal 255 characters long",
                        ]
                    );
                    break;
                case 3:
                    invalidRequestsList.Add(
                        [
                            fixture.GetInvalidRequestWithLongDescription(),
                            "Description should be less or equal 10000 characters long",
                        ]
                    );
                    break;
                default:
                    break;
            }
        }

        return invalidRequestsList;
    }
}

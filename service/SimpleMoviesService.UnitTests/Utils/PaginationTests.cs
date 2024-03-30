using MockQueryable.Moq;
using SimpleMoviesService.Utils;

namespace SimpleMoviesService.UnitTests.Utils;

public class PaginationTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(51)]
    public async void ToPagedListAsyncShouldTakeNRecordsAsync(int pageSize)
    {
        // Arrange
        var testList = new List<string>();
        for (var i = 0; i < 100; i++)
        {
            testList.Add((new Guid()).ToString());
        }

        var queryable = testList.AsQueryable().BuildMock();

        // Act
        var pagedList = await queryable.ToPagedListAsync(1, pageSize);

        // Assert
        Assert.NotEmpty(pagedList.Items);
        Assert.Equal(pageSize, pagedList.PageSize);
        Assert.Equal(pageSize, pagedList.Items.Count);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(5, 1)]
    [InlineData(10, 5)]
    [InlineData(21, 2)]
    public async void ToPagedListAsyncShouldTakeNthRecords(int pageSize, int currentPage)
    {
        // Arrange
        var testList = new List<string>();
        for (var i = 0; i < 100; i++)
        {
            testList.Add((new Guid()).ToString());
        }
        var expectedList = testList.Skip(pageSize * (currentPage - 1)).Take(pageSize);

        var queryable = testList.AsQueryable().BuildMock();

        // Act
        var pagedList = await queryable.ToPagedListAsync<string>(currentPage, pageSize);

        // Assert
        Assert.Equal(currentPage, pagedList.Page);
        Assert.Equal(expectedList, pagedList.Items);
    }

    [Theory]
    [InlineData(100, 10, 10, 10)]
    [InlineData(10, 1, 10, 1)]
    [InlineData(96, 10, 10, 6)]
    [InlineData(52, 7, 8, 3)]
    public async void ToPagedListAsyncShouldHaveNElementsOnLastPage(int numberOfRecords, int pageSize, int lastPage, int lastPageSize)
    {
        // Arrange
        var testList = new List<string>();
        for (var i = 0; i < numberOfRecords; i++)
        {
            testList.Add((new Guid()).ToString());
        }
        var queryable = testList.AsQueryable().BuildMock();

        // Act
        var pagedList = await queryable.ToPagedListAsync<string>(lastPage, pageSize);

        // Assert
        Assert.Equal(lastPage, pagedList.Page);
        Assert.Equal(lastPageSize, pagedList.Items.Count);
    }

    [Theory]
    [InlineData(100, 10, 10)]
    [InlineData(7, 10, 1)]
    [InlineData(51, 7, 8)]
    public async void ToPagedListAsyncShouldHaveNPagesWithNRows(int numberOfRecords, int pageSize, int numberOfPages)
    {
        // Arrange
        var testList = new List<string>();
        for (var i = 0; i < numberOfRecords; i++)
        {
            testList.Add((new Guid()).ToString());
        }
        var queryable = testList.AsQueryable().BuildMock();

        // Act
        var pagedList = await queryable.ToPagedListAsync<string>(1, pageSize);

        // Assert
        Assert.Equal(numberOfRecords, pagedList.TotalItems);
        Assert.Equal(numberOfPages, pagedList.TotalPages);
    }
}

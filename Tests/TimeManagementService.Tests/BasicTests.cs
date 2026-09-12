using Xunit;

namespace TimeManagementService.Tests;

public class BasicTests
{
    [Fact]
    public void Addition_TwoNumbers_ReturnsCorrectResult()
    {
        // Arrange
        var a = 2;
        var b = 3;

        // Act
        var result = a + b;

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void String_ShouldContainExpectedText()
    {
        // Arrange
        var text = "TimeManagementService";

        // Act
        var result = text.Contains("Management");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void List_ShouldContainExpectedNumberOfItems()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3 };

        // Act
        var count = items.Count;

        // Assert
        Assert.Equal(3, count);
    }
}
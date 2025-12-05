using FieldUp.Domain.ValueObjects;

namespace FieldUp.Domain.UnitTests.ValueObjects;

public class NameTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesName()
    {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";

        // Act
        var name = new Name(firstName, lastName);

        // Assert
        Assert.Equal(firstName, name.FirstName);
        Assert.Equal(lastName, name.LastName);
    }

    [Theory]
    [InlineData("  John  ", "Doe", "John", "Doe")]
    [InlineData("Jane", "  Smith  ", "Jane", "Smith")]
    [InlineData("  Alice  ", "  Johnson  ", "Alice", "Johnson")]
    public void Constructor_WithWhitespace_TrimsValues(string firstName, string lastName, 
        string expectedFirstName, string expectedLastName)
    {
        // Act
        var name = new Name(firstName, lastName);

        // Assert
        Assert.Equal(expectedFirstName, name.FirstName);
        Assert.Equal(expectedLastName, name.LastName);
    }

    [Theory]
    [InlineData(null, "Doe")]
    [InlineData("", "Doe")]
    [InlineData("   ", "Doe")]
    public void Constructor_WithInvalidFirstName_ThrowsArgumentNullException(string? firstName, string lastName)
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Name(firstName!, lastName));
    }

    [Theory]
    [InlineData("John", null)]
    [InlineData("John", "")]
    [InlineData("John", "   ")]
    public void Constructor_WithInvalidLastName_ThrowsArgumentNullException(string firstName, string? lastName)
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Name(firstName, lastName!));
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        // Arrange
        var name1 = new Name("John", "Doe");
        var name2 = new Name("John", "Doe");

        // Act & Assert
        Assert.Equal(name1, name2);
        Assert.True(name1.Equals(name2));
        Assert.True(name1 == name2);
    }

    [Theory]
    [InlineData("John", "Doe", "Jane", "Doe")]
    [InlineData("John", "Doe", "John", "Smith")]
    [InlineData("John", "Doe", "Jane", "Smith")]
    public void Equals_WithDifferentValues_ReturnsFalse(string firstName1, string lastName1, 
        string firstName2, string lastName2)
    {
        // Arrange
        var name1 = new Name(firstName1, lastName1);
        var name2 = new Name(firstName2, lastName2);

        // Act & Assert
        Assert.NotEqual(name1, name2);
        Assert.False(name1.Equals(name2));
        Assert.True(name1 != name2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ReturnsSameHashCode()
    {
        // Arrange
        var name1 = new Name("John", "Doe");
        var name2 = new Name("John", "Doe");

        // Act & Assert
        Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        // Arrange
        var name = new Name("John", "Doe");

        // Act & Assert
        Assert.False(name.Equals(null));
        Assert.True(name != null!);
    }
}
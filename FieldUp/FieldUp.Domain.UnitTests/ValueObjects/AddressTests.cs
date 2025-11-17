using FieldUp.Domain.ValueObjects;

namespace FieldUp.Domain.UnitTests.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesAddress()
    {
        // Arrange
        const string line = "123 Main Street";
        const string city = "Madrid";
        const string postalCode = "1000-001";
        const string country = "Spain";

        // Act
        var address = new Address(line, city, postalCode, country);

        // Assert
        Assert.Equal(line, address.Line);
        Assert.Equal(city, address.City);
        Assert.Equal(postalCode, address.PostalCode);
        Assert.Equal(country, address.Country);
    }

    [Fact]
    public void Constructor_WithoutCountry_UsesDefaultPortugal()
    {
        // Arrange & Act
        var address = new Address("123 Main Street", "Lisbon", "1000-001");

        // Assert
        Assert.Equal("Portugal", address.Country);
    }

    [Theory]
    [InlineData("  123 Main Street  ", "Lisbon", "1000-001", "Portugal", "123 Main Street", "Lisbon", "1000-001", "Portugal")]
    [InlineData("456 Other Street  ", "  Porto", "4000-001  ", "  Spain", "456 Other Street", "Porto", "4000-001", "Spain")]
    public void Constructor_WithWhitespace_TrimsValues(string line, string city, string postalCode, string country, 
        string expectedLine, string expectedCity, string expectedPostalCode, string expectedCountry)
    {
        // Act
        var address = new Address(line, city, postalCode, country);

        // Assert
        Assert.Equal(expectedLine, address.Line);
        Assert.Equal(expectedCity, address.City);
        Assert.Equal(expectedPostalCode, address.PostalCode);
        Assert.Equal(expectedCountry, address.Country);
    }

    [Theory]
    [InlineData(null, "Lisbon", "1000-001", "Portugal")]
    [InlineData("", "Lisbon", "1000-001", "Portugal")]
    [InlineData("   ", "Lisbon", "1000-001", "Portugal")]
    public void Constructor_WithInvalidLine_ThrowsArgumentException(string? line, string city, string postalCode, string country)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Address(line!, city, postalCode, country));
    }

    [Theory]
    [InlineData("123 Main Street", null, "1000-001", "Portugal")]
    [InlineData("123 Main Street", "", "1000-001", "Portugal")]
    [InlineData("123 Main Street", "   ", "1000-001", "Portugal")]
    public void Constructor_WithInvalidCity_ThrowsArgumentException(string line, string? city, string postalCode, string country)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Address(line, city!, postalCode, country));
    }

    [Theory]
    [InlineData("123 Main Street", "Lisbon", null, "Portugal")]
    [InlineData("123 Main Street", "Lisbon", "", "Portugal")]
    [InlineData("123 Main Street", "Lisbon", "   ", "Portugal")]
    public void Constructor_WithInvalidPostalCode_ThrowsArgumentException(string line, string city, string? postalCode, string country)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Address(line, city, postalCode, country));
    }
    
    [Theory]
    [InlineData("123 Main Street", "Lisbon", "1000-001", null)]
    [InlineData("123 Main Street", "Lisbon", "1000-001", "")]
    [InlineData("123 Main Street", "Lisbon", "1000-001", "   ")]
    public void Constructor_WithInvalidCountry_ThrowsArgumentException(string line, string city, string postalCode, string? country)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Address(line, city, postalCode, country!));
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        // Arrange
        var address1 = new Address("123 Main Street", "Lisbon", "1000-001");
        var address2 = new Address("123 Main Street", "Lisbon", "1000-001");

        // Act & Assert
        Assert.Equal(address1, address2);
        Assert.True(address1.Equals(address2));
        Assert.True(address1 == address2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var address1 = new Address("123 Main Street", "Lisbon", "1000-001");
        var address2 = new Address("456 Other Street", "Porto", "4000-001");

        // Act & Assert
        Assert.NotEqual(address1, address2);
        Assert.False(address1.Equals(address2));
        Assert.True(address1 != address2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ReturnsSameHashCode()
    {
        // Arrange
        var address1 = new Address("123 Main Street", "Lisbon", "1000-001");
        var address2 = new Address("123 Main Street", "Lisbon", "1000-001");

        // Act & Assert
        Assert.Equal(address1.GetHashCode(), address2.GetHashCode());
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        // Arrange
        var address = new Address("123 Main Street", "Lisbon", "1000-001");

        // Act & Assert
        Assert.False(address.Equals(null));
        Assert.True(address != null!);
    }
}
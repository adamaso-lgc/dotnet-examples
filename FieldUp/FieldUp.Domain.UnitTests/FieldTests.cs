using FieldUp.Domain.ValueObjects;

namespace FieldUp.Domain.UnitTests;

public class FieldTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesField()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        const string name = "Football Field A";
        var address = new Address("123 Stadium Road", "Lisbon", "1000-001");
        const SportType type = SportType.Football;

        // Act
        var field = new Field(fieldId, name, address, type);

        // Assert
        Assert.Equal(fieldId, field.FieldId);
        Assert.Equal(name, field.Name);
        Assert.Equal(address, field.Address);
        Assert.Equal(type, field.Type);
        Assert.False(field.IsActive);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        const string name = "Football Field A";
        var address = new Address("123 Stadium Road", "Lisbon", "1000-001", "Portugal");
        const SportType type = SportType.Football;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Field(Guid.Empty, name, address, type));
        Assert.Equal("fieldId", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ThrowsArgumentException(string? name)
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var address = new Address("123 Stadium Road", "Lisbon", "1000-001");
        const SportType type = SportType.Football;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Field(fieldId, name!, address, type));
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullAddress_ThrowsArgumentNullException()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        const string name = "Football Field A";
        const SportType type = SportType.Football;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new Field(fieldId, name, null!, type));
    }

    [Theory]
    [InlineData(SportType.Football)]
    [InlineData(SportType.Padel)]
    [InlineData(SportType.Tennis)]
    public void Constructor_WithDifferentFieldTypes_CreatesField(SportType type)
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        const string name = "Test Field";
        var address = new Address("123 Stadium Road", "Lisbon", "1000-001", "Portugal");

        // Act
        var field = new Field(fieldId, name, address, type);

        // Assert
        Assert.Equal(type, field.Type);
    }

    [Fact]
    public void Constructor_SetsIsActiveToFalse()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        const string name = "Football Field A";
        var address = new Address("123 Stadium Road", "Lisbon", "1000-001");
        const SportType type = SportType.Football;

        // Act
        var field = new Field(fieldId, name, address, type);

        // Assert
        Assert.False(field.IsActive);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetStatus_ChangesIsActive(bool status)
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        const string name = "Football Field A";
        var address = new Address("123 Stadium Road", "Lisbon", "1000-001");
        const SportType type = SportType.Football;
        var field = new Field(fieldId, name, address, type);

        // Act
        field.SetStatus(status);

        // Assert
        Assert.Equal(status, field.IsActive);
    }

    [Fact]
    public void SetStatus_CanToggleMultipleTimes()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        const string name = "Football Field A";
        var address = new Address("123 Stadium Road", "Lisbon", "1000-001");
        const SportType type = SportType.Football;
        var field = new Field(fieldId, name, address, type);

        // Act & Assert
        field.SetStatus(true);
        Assert.True(field.IsActive);

        field.SetStatus(false);
        Assert.False(field.IsActive);

        field.SetStatus(true);
        Assert.True(field.IsActive);
    }
}
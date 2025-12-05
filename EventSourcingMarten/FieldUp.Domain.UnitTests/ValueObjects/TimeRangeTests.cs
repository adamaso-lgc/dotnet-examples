using FieldUp.Domain.ValueObjects;

namespace FieldUp.Domain.UnitTests.ValueObjects;

public class TimeRangeTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesTimeRange()
    {
        // Arrange
        var start = DateTimeOffset.UtcNow;
        var end = start.AddHours(2);

        // Act
        var timeRange = new TimeRange(start, end);

        // Assert
        Assert.Equal(start, timeRange.Start);
        Assert.Equal(end, timeRange.End);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(24)]
    [InlineData(168)]
    public void Constructor_WithEndAfterStart_CreatesTimeRange(int hoursAfter)
    {
        // Arrange
        var start = DateTimeOffset.UtcNow;
        var end = start.AddHours(hoursAfter);

        // Act
        var timeRange = new TimeRange(start, end);

        // Assert
        Assert.Equal(start, timeRange.Start);
        Assert.Equal(end, timeRange.End);
    }

    [Fact]
    public void Constructor_WithEndBeforeStart_ThrowsArgumentException()
    {
        // Arrange
        var start = DateTimeOffset.UtcNow;
        var end = start.AddHours(-1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new TimeRange(start, end));
        Assert.Equal("End must be after start.", exception.Message);
    }

    [Fact]
    public void Constructor_WithEndEqualToStart_ThrowsArgumentException()
    {
        // Arrange
        var start = DateTimeOffset.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new TimeRange(start, start));
        Assert.Equal("End must be after start.", exception.Message);
    }

    [Theory]
    [InlineData(10, 12, 11, 13)]  // Overlaps in middle
    [InlineData(10, 12, 9, 11)]   // Overlaps at start
    [InlineData(10, 15, 12, 17)]  // Overlaps at end
    [InlineData(10, 15, 11, 14)]  // Completely contained
    [InlineData(11, 14, 10, 15)]  // Contains other
    public void Overlaps_WithOverlappingRanges_ReturnsTrue(int start1Hour, int end1Hour, int start2Hour, int end2Hour)
    {
        // Arrange
        var baseDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var range1 = new TimeRange(baseDate.AddHours(start1Hour), baseDate.AddHours(end1Hour));
        var range2 = new TimeRange(baseDate.AddHours(start2Hour), baseDate.AddHours(end2Hour));

        // Act & Assert
        Assert.True(range1.Overlaps(range2));
        Assert.True(range2.Overlaps(range1));
    }

    [Theory]
    [InlineData(10, 12, 12, 14)]  // End equals next start
    [InlineData(10, 12, 13, 15)]  // Completely separate
    [InlineData(10, 12, 8, 10)]   // Start equals previous end
    public void Overlaps_WithNonOverlappingRanges_ReturnsFalse(int start1Hour, int end1Hour, int start2Hour, int end2Hour)
    {
        // Arrange
        var baseDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var range1 = new TimeRange(baseDate.AddHours(start1Hour), baseDate.AddHours(end1Hour));
        var range2 = new TimeRange(baseDate.AddHours(start2Hour), baseDate.AddHours(end2Hour));

        // Act & Assert
        Assert.False(range1.Overlaps(range2));
        Assert.False(range2.Overlaps(range1));
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        // Arrange
        var start = DateTimeOffset.UtcNow;
        var end = start.AddHours(2);
        var range1 = new TimeRange(start, end);
        var range2 = new TimeRange(start, end);

        // Act & Assert
        Assert.Equal(range1, range2);
        Assert.True(range1.Equals(range2));
        Assert.True(range1 == range2);
    }

    [Theory]
    [InlineData(10, 12, 10, 13)]  // Different end
    [InlineData(10, 12, 11, 12)]  // Different start
    [InlineData(10, 12, 11, 13)]  // Different both
    public void Equals_WithDifferentValues_ReturnsFalse(int start1Hour, int end1Hour, int start2Hour, int end2Hour)
    {
        // Arrange
        var baseDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var range1 = new TimeRange(baseDate.AddHours(start1Hour), baseDate.AddHours(end1Hour));
        var range2 = new TimeRange(baseDate.AddHours(start2Hour), baseDate.AddHours(end2Hour));

        // Act & Assert
        Assert.NotEqual(range1, range2);
        Assert.False(range1.Equals(range2));
        Assert.True(range1 != range2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ReturnsSameHashCode()
    {
        // Arrange
        var start = DateTimeOffset.UtcNow;
        var end = start.AddHours(2);
        var range1 = new TimeRange(start, end);
        var range2 = new TimeRange(start, end);

        // Act & Assert
        Assert.Equal(range1.GetHashCode(), range2.GetHashCode());
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        // Arrange
        var start = DateTimeOffset.UtcNow;
        var end = start.AddHours(2);
        var range = new TimeRange(start, end);

        // Act & Assert
        Assert.False(range.Equals(null));
        Assert.True(range != null!);
    }
}
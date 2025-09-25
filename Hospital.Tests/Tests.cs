namespace Hospital.Tests;

/// <summary>
/// Unit tests.
/// </summary>
public class QueriesTests : IClassFixture<TestData>
{
    private readonly TestData _data;

    public QueriesTests(TestData data)
    {
        _data = data;
    }

    /// <summary>
    /// Verifies that only doctors with >= 10 years experience are returned.
    /// </summary>
    [Fact]
    public void DoctorsWith10PlusReturnsOnlyThoseWithExperienceAtLeast10()
    {
        var result = Queries.DoctorsWith10Plus(_data.Doctors);
        Assert.NotEmpty(result);
        Assert.All(result, d => Assert.True(d.ExperienceYears >= 10));
        Assert.DoesNotContain(result, d => d.ExperienceYears < 10);
    }

    /// <summary>
    /// Verifies that only follow-ups in the last month are counted.
    /// </summary>
    [Fact]
    public void FollowUpsLastMonthCountsOnlyFollowUpsInWindow()
    {
        var count = Queries.FollowUpsLastMonth(_data.Appointments, _data.Now);
        Assert.Equal(2, count);
    }

    /// <summary>
    /// Verifies that patients older than 30 with 2+ doctors are returned,
    /// and that they are sorted by birth date.
    /// </summary>
    [Fact]
    public void Patients30PlusMultiDoctorsFiltersAndOrdersByBirthDate()
    {
        var expectedNames = new[] { "Podtyagina Anastasia", "Yemets Timofey" };
        var result = Queries.Patients30PlusMultiDoctors(_data.Appointments, _data.Today);
        var names = result.Select(p => p.FullName).ToList();
        Assert.Equal(expectedNames, names);
    }

    /// <summary>
    /// Verifies that appointments are correctly filtered by room and month,
    /// and sorted by start time.
    /// </summary>
    [Fact]
    public void ThisMonthInRoomFiltersByRoomAndCurrentMonthAndSortsByStart()
    {
        var result = Queries.ThisMonthInRoom(_data.Appointments, "301", _data.Now);
        Assert.Equal(3, result.Count);
        Assert.True(result.Select(a => a.StartAt).SequenceEqual(result.Select(a => a.StartAt).OrderBy(t => t)));
    }
}

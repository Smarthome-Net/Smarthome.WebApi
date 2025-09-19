using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.Dto.Charts;
using SmartHome.Common.Models.Dto.Requests;
using SmartHome.Common.Models.Dto.Responses;
using SmartHome.Webservice.EndpointHandlers;

namespace SmartHome.Tests.UnitTests;

public class ChartTests
{
    private readonly Scope _scope = new()
    {
        ScopeType = ScopeType.All,
        Value = "r/t"
    };

    private readonly DateTimeOffset actualTime = DateTimeOffset.Now;

    private Mock<ITemperatureService>? _temperatureServiceMock;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _temperatureServiceMock = new Mock<ITemperatureService>();

        _temperatureServiceMock
            .Setup(s => s.GetTemperature(It.IsAny<Scope>()))
            .Returns(() => [
                new TemperatureDto()
                {
                    Id = "2",
                    RecordDateTime = actualTime,
                    Value = 42.5f,
                    Device = new DeviceDto {
                        Id = "2",
                        Name = "t",
                        Room = "r",
                        Topic = "r/t"
                    }
                }]
            );  
    }

    [Test]
    public void TestGetTemperatureWithValidData()
    {
        var pagination = new Pagination()
        {
            Length = 10,
            PageIndex = 0,
            PageSize = 10
        };

        var request = new TemperatureRequest()
        {
            Scope = _scope,
            Pagination = pagination
        };

        var expected = new TemperatureResponse()
        {
            Scope = _scope,
            Pagination = pagination,
            Temperatures = [
                new Chart<DateTimeOffset, float> {
                    Name = "r",
                    Series = {
                        { actualTime, 42.5f },
                    }
                },
            ]
        };

        var result = Charts.GetTemperature(request, _temperatureServiceMock!.Object);

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expected);
    }

    [Test]
    public void TestGetStatisticWithValidData()
    {
        var logger = NullLogger<Charts>.Instance;
        var request = new StatisticRequest
        { 
            Scope = _scope
        };

        var expected = new StatisticResponse
        {
            Scope = _scope,
            Statistic = new Chart<string, float>
            {
                Name = "r/t",
                Series = {
                    { "min", 42.5f },
                    { "average", 42.5f },
                    { "max", 42.5f },
                }
            }
        };


        var result = Charts.GetStatistic(request, logger, _temperatureServiceMock!.Object);

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expected);
    }
}

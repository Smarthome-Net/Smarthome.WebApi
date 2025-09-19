using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Bson;
using Moq;
using SmartHome.Common.Extensions.Mapping;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.MqttMessages;
using SmartHome.Webservice.EndpointHandlers;

namespace SmartHome.Tests.UnitTests;

public class DeviceTests
{
    private readonly NullLogger<Devices> _logger = NullLogger<Devices>.Instance;

    private readonly IEnumerable<Device> _devices = [
            new() { Id = ObjectId.GenerateNewId(), Name = "n", Room = "t", Topic = "t/n" },
            new() { Id = ObjectId.GenerateNewId(), Name = "n", Room = "t", Topic = "t/n" },
            new() { Id = ObjectId.GenerateNewId(), Name = "n", Room = "r", Topic = "r/n" }
    ];


    [Test]
    public async Task TestGetAllDevicesWithProblemResponse()
    {
        var expected = TypedResults.Problem();

        var deviceServiceMock = new Mock<IDeviceService>();
        deviceServiceMock
            .Setup(s => s.GetAllDevices(It.IsAny<CancellationToken>()))
            .Throws<NullReferenceException>();

        var result = await Devices.GetAllDevices(_logger, deviceServiceMock!.Object);

        result.Result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task TestGetAllDevicesWithOkResponse()
    {
        var expected = TypedResults.Ok(_devices.ToDto());

        var deviceServiceMock = new Mock<IDeviceService>();
        deviceServiceMock
            .Setup(s => s.GetAllDevices(It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(_devices));

        var result = await Devices.GetAllDevices(_logger, deviceServiceMock!.Object);

        result.Result.Should().BeEquivalentTo(expected);
    }

    [TestCase("t")]
    [TestCase("r")]
    public async Task TestGetListOfDevicesWithOkResponse(string room)
    {
        var devices = _devices.Where(d => string.Equals(room, d.Room));

        var expected = TypedResults.Ok(devices.ToDto());

        var deviceServiceMock = new Mock<IDeviceService>();
        deviceServiceMock
            .Setup(s => s.GetDevices(room, It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(devices));

        var result = await Devices.GetListOfDevices(room, _logger, deviceServiceMock.Object);

        result.Result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task TestGetDeviceStatusWithProblemResponse()
    {
        var expected = TypedResults.Problem();

        var deviceServiceMock = new Mock<IDeviceService>();
        var deviceManagerMock = new Mock<IDeviceManager>();

        deviceServiceMock
            .Setup(s => s.GetDeviceById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Throws<NullReferenceException>();

        var result = await Devices.GetDeviceStatus("", _logger, deviceServiceMock.Object, deviceManagerMock.Object);

        result.Result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task TestGetDeviceStatusWithOkResponse()
    {
        var id = ObjectId.GenerateNewId();
        DeviceStatus? status = new()
        {
            BatteryStatus = 0.9f,
            CurrentTemperature = 42.5f,
            MqttConnectedStatus = ConnectionStatus.Connected,
            WifiConnectedStatus = ConnectionStatus.Connected,
        };
        Device? device = new()
        {
            Id = id,
            Name = "r",
            Room = "t",
            Topic = "t/r"
        };
        var expected = TypedResults.Ok(status);

        var deviceServiceMock = new Mock<IDeviceService>();
        var deviceManagerMock = new Mock<IDeviceManager>();

        deviceServiceMock
            .Setup(s => s.GetDeviceById(id.ToString(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult<Device?>(device));

        deviceManagerMock
            .Setup(s => s.GetStatus(device.Topic))
            .Returns(() => Task.FromResult<DeviceStatus?>(status));

        var result = await Devices.GetDeviceStatus(id.ToString(), _logger, deviceServiceMock.Object, deviceManagerMock.Object);

        result.Result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task DevicesGetDeviceConfigWithProblemResponse()
    {
        var id = ObjectId.GenerateNewId();

        var expected = TypedResults.Problem();

        var deviceServiceMock = new Mock<IDeviceService>();
        var deviceManagerMock = new Mock<IDeviceManager>();

        var result = await Devices.GetDeviceConfig(id.ToString(), _logger, deviceServiceMock.Object, deviceManagerMock.Object);

        result.Result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task DevicesGetDeviceConfigWithOkResponse()
    {
        var id = ObjectId.GenerateNewId();

        var expected = TypedResults.Ok();

        var deviceServiceMock = new Mock<IDeviceService>();
        var deviceManagerMock = new Mock<IDeviceManager>();

        var result = await Devices.GetDeviceConfig(id.ToString(), _logger, deviceServiceMock.Object, deviceManagerMock.Object);

        result.Result.Should().BeEquivalentTo(expected);
    }
}

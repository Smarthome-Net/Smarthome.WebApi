using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models;
using SmartHome.Common.Models.Db;
using SmartHome.MqttService.Services;

namespace SmartHome.Webservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> _logger;
    private readonly IDeviceService _deviceService;
    private readonly IDeviceManager _deviceManager;

    public DeviceController(ILogger<DeviceController> logger, IDeviceService deviceService, IDeviceManager deviceManager)
    {
        _logger = logger;
        _deviceService = deviceService;
        _deviceManager = deviceManager;
    }

    [HttpGet("rooms")]
    public async Task<ActionResult<IEnumerable<Device>>> GetListOfRooms()
    {
        try
        {
            var rooms = await _deviceService.GetRooms();
            return Ok(rooms);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception on get rooms");
            return Problem();
        }
    }

    [HttpGet]

    public async Task<ActionResult<IEnumerable<Device>>> GetAllDevices() 
    {
        try
        {
            var devices = await _deviceService.GetAllDevices();
            return Ok(devices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepion on get rooms");
            return Problem();
        }
    }

    [HttpGet("{room}")]
    public async Task<ActionResult<IEnumerable<Device>>> GetListOfDevices(string room)
    {
        try
        {
            var devices = await _deviceService.GetDevices(room);
            return Ok(devices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepion on get rooms");
            return Problem();
        }
    }

    [HttpGet("{deviceId}/status")]
    public async Task<ActionResult<DeviceStatus>> GetDeviceConfig(string deviceId)
    {
        try
        {
            var device = await _deviceService.GetDeviceById(deviceId);
            var status = await _deviceManager.GetStatus(device.Topic);
            return Ok(status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepion on get device status");
            return Problem();
        }
    }

    [HttpGet("{deviceId}/config")]
    public async Task<ActionResult<Device>> GetDeviceStatus(string deviceId)
    {
        try
        {
            var device = await _deviceService.GetDeviceById(deviceId);
            device.Configuration = await _deviceManager.GetConfiguration(device.Topic);
            return Ok(device);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepion on get device status");
            return Problem();
        }
    }

    [HttpPost("config")]
    public async Task<ActionResult<Device>> UpdateDeviceConfig(Device device)
    {
        try
        {
            var config = await _deviceManager.PopulateConfiguration(device.Topic, device.Configuration);
            await _deviceService.UpdateDevice(device);
            device.Configuration = config;
            return Ok(device);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepion on get device status");
            return Problem();
        }
    }
}

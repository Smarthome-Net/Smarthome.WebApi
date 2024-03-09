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

    /// <summary>
    /// Get a list of all devices
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// Get a list of all devices in one room
    /// </summary>
    /// <param name="room">Name of the room</param>
    /// <returns></returns>
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

    /// <summary>
    /// Get the status of the spezified device id
    /// </summary>
    /// <param name="deviceId">Id of the device</param>
    /// <returns></returns>
    [HttpGet("{deviceId}/status")]
    public async Task<ActionResult<DeviceStatus>> GetDeviceStatus(string deviceId)
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

    /// <summary>
    /// Get the config of the spezified device id
    /// </summary>
    /// <param name="deviceId">Id of the device</param>
    /// <returns></returns>
    [HttpGet("{deviceId}/config")]
    public async Task<ActionResult<Device>> GetDeviceConfig(string deviceId)
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

    /// <summary>
    /// Updates the config of the spezified device id
    /// </summary>
    /// <param name="deviceId">Id of the device</param>
    /// <returns></returns>
    [HttpPost("{deviceId}/config")]
    public async Task<ActionResult<Device>> UpdateDeviceConfig(string deviceId, Device device)
    {
        if(!string.Equals(deviceId, device.Id)) 
        {
            return BadRequest("Id mismatch");
        }

        if(device.Configuration is null) 
        {
            return BadRequest("Configuartion was not set");
        }

        try
        {
            var result = await _deviceService.UpdateDevice(device);
            if(result == 0)
            {
                return Problem();
            }
            device.Configuration = await _deviceManager.PopulateConfiguration(device.Topic, device.Configuration);
            return Ok(device);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepion on get device status");
            return Problem();
        }
    }
}

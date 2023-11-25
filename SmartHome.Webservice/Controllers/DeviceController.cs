using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;

namespace SmartHome.Webservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> logger;
    private readonly IDeviceService deviceService;
    public DeviceController(ILogger<DeviceController> logger, IDeviceService deviceService)
    {
        this.logger = logger;
        this.deviceService = deviceService;
    }

    [HttpGet("rooms")]
    public async Task<ActionResult<IEnumerable<Device>>> GetListOfRooms()
    {
        try
        {
            var rooms = await deviceService.GetRooms();
            return Ok(rooms);
        }
        catch (Exception ex)
        {
            logger.LogError("Exception on get rooms", ex);
            return Problem();
        }
    }

    [HttpGet]

    public async Task<ActionResult<IEnumerable<Device>>> GetAllDevices() 
    {
        try
        {
            var devices = await deviceService.GetAllDevices();
            return Ok(devices);
        }
        catch (Exception ex)
        {
            logger.LogError("Excepion on get rooms", ex);
            return Problem();
        }
    }

    [HttpGet("{room}")]
    public async Task<ActionResult<IEnumerable<Device>>> GetListOfDevices(string room)
    {
        try
        {
            var devices = await deviceService.GetDevices(room);
            return Ok(devices);
        }
        catch (Exception ex)
        {
            logger.LogError("Excepion on get rooms", ex);
            return Problem();
        }
    }
}

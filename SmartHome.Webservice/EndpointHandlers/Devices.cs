using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using SmartHome.Common.Extensions.Mapping;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.MqttMessages;

namespace SmartHome.Webservice.EndpointHandlers;
public class Devices
{
    /// <summary>
    /// Get a list of all devices
    /// </summary>
    /// <returns></returns>
    public static async Task<Results<Ok<IEnumerable<DeviceDto>>, ProblemHttpResult>> GetAllDevices(ILogger<Devices> logger, IDeviceService deviceService)
    {
        try
        {
            var devices = await deviceService.GetAllDevices();
            return TypedResults.Ok(devices.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception on get rooms");
            return TypedResults.Problem();
        }
    }

    /// <summary>
    /// Get a list of all devices in one room
    /// </summary>
    /// <param name="room">Name of the room</param>
    /// <param name="logger"></param>
    /// <param name="deviceService"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<IEnumerable<DeviceDto>>, ProblemHttpResult>> GetListOfDevices(string room, ILogger<Devices> logger, IDeviceService deviceService)
    {
        try
        {
            var devices = await deviceService.GetDevices(room);
            return TypedResults.Ok(devices.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception on get rooms");
            return TypedResults.Problem();
        }
    }

    /// <summary>
    /// Get the status of the specified device id
    /// </summary>
    /// <param name="deviceId">Id of the device</param>
    /// <param name="logger"></param>
    /// <param name="deviceService"></param>
    /// <param name="deviceManager"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<DeviceStatus>, ProblemHttpResult>> GetDeviceStatus(string deviceId, ILogger<Devices> logger, IDeviceService deviceService, IDeviceManager deviceManager)
    {
        try
        {
            var device = await deviceService.GetDeviceById(deviceId);
            var status = await deviceManager.GetStatus(device?.Topic!);
            return TypedResults.Ok(status);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception on get device status");
            return TypedResults.Problem();
        }
    }

    /// <summary>
    /// Get the config of the specified device id
    /// </summary>
    /// <param name="deviceId">Id of the device</param>
    /// <param name="logger"></param>
    /// <param name="deviceService"></param>
    /// <param name="deviceManager"></param>
    /// <returns></returns>
    public static async Task<Results<ProblemHttpResult, Ok<DeviceDto>>> GetDeviceConfig(string deviceId, ILogger<Devices> logger, IDeviceService deviceService, IDeviceManager deviceManager)
    {
        try
        {
            var result = await deviceService.GetDeviceById(deviceId);
            var device = result?.ToDto();
            var configuration = await deviceManager.GetConfiguration(device?.Topic!);
            device!.Configuration = configuration?.ToDto();
            return  TypedResults.Ok(device);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception on get device status");
            return TypedResults.Problem();
        }
    }

    /// <summary>
    /// Updates the config of the specified device id
    /// </summary>
    /// <param name="deviceId">Id of the device</param>
    /// <param name="device"></param>
    /// <param name="logger"></param>
    /// <param name="deviceService"></param>
    /// <param name="deviceManager"></param>
    /// <returns></returns>
    public static async Task<Results<BadRequest<string>, ProblemHttpResult, Ok<DeviceDto>>> UpdateDeviceConfig(string deviceId, DeviceDto device, ILogger<Devices> logger, IDeviceService deviceService, IDeviceManager deviceManager)
    {
        if(!string.Equals(deviceId, device.Id)) 
        {
            return TypedResults.BadRequest("Id mismatch");
        }

        if(device.Configuration is null) 
        {
            return TypedResults.BadRequest("Configuartion was not set");
        }

        try
        {
            var result = await deviceService.UpdateDevice(device.ToDb()!);
            if(result == 0)
            {
                return TypedResults.Problem();
            }

            var mqttMessage = device.Configuration.ToMessage(device);
            var configuration = await deviceManager.PopulateConfiguration(device.Topic!, mqttMessage);
            device.Configuration = configuration?.ToDto();
            return TypedResults.Ok(device);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception on get device status");
            return TypedResults.Problem();
        }
    }
}

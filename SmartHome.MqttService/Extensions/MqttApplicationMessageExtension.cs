using MQTTnet;
using SmartHome.Common.Collections;

namespace SmartHome.MqttService.Extensions;

internal static class MqttApplicationMessageExtension
{
    public static string GetDeviceContext(this MqttApplicationMessage message, Segments segmentsToRemove) 
    {
        var segments = Segments.FromString(message.Topic);
        segments.Remove(segmentsToRemove);
        var deviceContext = segments.MergeSegments();
        return deviceContext;
    }
}
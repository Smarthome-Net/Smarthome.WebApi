using MQTTnet;
using SmartHome.Common.Collections;

namespace SmartHome.MqttService.Extensions;

internal static class MqttApplicationMessageExtension
{
    public static string GetDeviceContext(this MqttApplicationMessage message, string segmentsToRemove) 
    {
        var segments = Segments.FromString(message.Topic);
        segments.RemoveSegments(segmentsToRemove);
        var deviceContext = segments.MergeSegments();
        return deviceContext;
    }
}
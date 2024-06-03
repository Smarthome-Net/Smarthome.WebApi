using MQTTnet;
using SmartHome.Common.Collections;

namespace SmartHome.MqttService.Extensions;

internal static class MqttApplicationMessageExtension
{
    public static string GetDeviceContext(this MqttApplicationMessage message, string segments) 
    {
        var segmetns = Segments.FromString(message.Topic);
        segmetns.RemoveSegments(segments);
        var deviceContext = segmetns.MergeSegments();
        return deviceContext;
    }
}
using System.Collections.Generic;

namespace SmartHome.Common.Models;

public class TemperatureStatistic
{
    public Dictionary<string, float> MaxTemperatureData { get; set; }
    public Dictionary<string, float> MinTemperatureData { get; set; }
    public Dictionary<string, float> AverageTemperatureData { get; set; }

    public TemperatureStatistic() 
    {
        MaxTemperatureData = new Dictionary<string, float>();
        MinTemperatureData = new Dictionary<string, float>();
        AverageTemperatureData = new Dictionary<string, float>();
    }
}

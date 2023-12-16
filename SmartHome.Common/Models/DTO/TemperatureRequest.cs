﻿namespace SmartHome.Common.Models.DTO;

public class TemperatureRequest 
{
    public ScopeType Scope { get; set; }
    public string ScopeValue { get; set; }
    public PageSetting PageSetting { get; set; }
}

﻿using System;

namespace SmartHome.Common.Models.Dto.Charts;

public class TimeSeries
{
    public DateTimeOffset Name { get; set; }
    public float Value { get; set; }
}

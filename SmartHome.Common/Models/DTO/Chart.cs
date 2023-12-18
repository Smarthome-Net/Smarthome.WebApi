using System.Collections.Generic;

namespace SmartHome.Common.Models.DTO;

public class Chart<TSeries>
{
    public string Name { get; set; }
    public IEnumerable<TSeries> Series { get; set; }
}

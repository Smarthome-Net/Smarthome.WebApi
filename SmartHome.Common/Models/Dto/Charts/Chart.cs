using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Common.Models.Dto.Charts;

public class Chart<TSeries>
{
    public string? Name { get; set; }
    public IEnumerable<TSeries> Series { get; set; } = [];
}

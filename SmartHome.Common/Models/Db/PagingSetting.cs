namespace SmartHome.Common.Models.Db;


public class PagingSetting : Setting
{
    public int Length { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

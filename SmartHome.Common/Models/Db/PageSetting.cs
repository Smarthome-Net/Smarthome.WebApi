namespace SmartHome.Common.Models.Db;


public class PageSetting : Setting
{
    public int Length { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

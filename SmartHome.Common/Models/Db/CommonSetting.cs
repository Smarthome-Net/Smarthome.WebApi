namespace SmartHome.Common.Models.Db;

public class CommonSetting : Setting
{
    public string? Title { get; set; }
    public string? Theme  { get; set; }
    public int PageLength { get; set; }
}
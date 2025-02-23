namespace SmartHome.Common.Models.Dto;

public class CommonSettingDto : SettingDto
{
    public string? Title { get; set; }
    public string? Theme  { get; set; }
    
    public int PageLength { get; set; }
}
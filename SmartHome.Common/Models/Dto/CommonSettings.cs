namespace SmartHome.Common.Models.Dto;

public class CommonSettingDto : SettingDto
{
    public string? Title { get; set; }
    public ColorScheme? ColorScheme { get; set; }
    
    public int PageLength { get; set; }
}
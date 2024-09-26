namespace SmartHome.Common.Models.Dto;

public class PageSettingDto : SettingDto
{
    public int Length { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

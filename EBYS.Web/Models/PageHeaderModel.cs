namespace EBYS.Web.Models
{
    public class PageHeaderModel
    {
        public string? Title { get; set; }
        public string? BreadcrumbParent { get; set; }
        public string? BreadcrumbCurrent { get; set; }
        public string? FilterJsFunction { get; set; } 
        public string? NewActionUrl { get; set; }
        public string? NewActionText { get; set; } = "Yeni Kayıt"; 
        public bool ShowFilterButton => !string.IsNullOrEmpty(FilterJsFunction);
        public bool ShowNewActionButton => !string.IsNullOrEmpty(NewActionUrl);
    }
}

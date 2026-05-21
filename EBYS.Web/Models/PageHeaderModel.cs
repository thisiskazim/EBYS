namespace EBYS.Web.Models
{
    public class PageHeaderModel
    {
        public string? Title { get; set; }
        public string? BreadcrumbParent { get; set; }
        public string? BreadcrumbCurrent { get; set; }
        public string? FilterJsFunction { get; set; } // Örn: "GelenEvrakListModule.openFilterWindow()"
        public string? NewActionUrl { get; set; } // Örn: "/GelenEvrak/EvrakOlustur"
        public string? NewActionText { get; set; } = "Yeni Kayıt"; // Default değer // Örn: "Yeni Gelen Evrak"
        public bool ShowFilterButton => !string.IsNullOrEmpty(FilterJsFunction);
        public bool ShowNewActionButton => !string.IsNullOrEmpty(NewActionUrl);
    }
}

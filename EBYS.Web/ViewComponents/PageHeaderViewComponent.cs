using EBYS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.ViewComponents
{
    public class PageHeaderViewComponent: ViewComponent
    {

        // Parametre olarak senin her yerde düzelttiğin modeli alıyor abi
        public IViewComponentResult Invoke(PageHeaderModel model)
        {
            return View(model);
        }
    }
}

using EBYS.Web.Models.Ui;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.ViewComponents;

public class ActionButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ActionButtonModel model)
    {
        return View(model);
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IMDB.Admin
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var config = context.HttpContext.RequestServices.GetService<IConfiguration>();

            var secret = config.GetSection("AdminPanel:Secret").Value;
            var model = context.ActionArguments.Values.FirstOrDefault();

            if (model != null)
            {
                var propertyInfo = model.GetType().GetProperty("AdminSecret");
                if (propertyInfo != null)
                {
                    var propertyValue = propertyInfo.GetValue(model);

                    if (propertyValue != null && propertyValue.ToString() == secret)
                    {
                        base.OnActionExecuting(context);
                        return;
                    }
                }

            }

            context.Result = new BadRequestObjectResult("Invalid input model");
        }

    }
}

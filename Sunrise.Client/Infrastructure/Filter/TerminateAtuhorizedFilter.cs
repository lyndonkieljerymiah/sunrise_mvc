using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Sunrise.Client.Infrastructure.Filter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TerminateAtuhorizedFilter : ActionFilterAttribute 
    {
        
        
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }

      




    }
}

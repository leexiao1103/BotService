using BotService.Model.Line;
using BotService.Service.Line;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotService.Filters.LINE
{
    public class LineWebhookAttribute : IActionFilter
    {
        private readonly ILineService _lineService;

        public LineWebhookAttribute(ILineService lineService)
        {
            _lineService = lineService;
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //header signature驗證做在這            
            //StringValues lineSignuter = string.Empty;
            //context.HttpContext.Request.Headers.TryGetValue("X-Line-Request-Id", out lineSignuter);

            var hookEvent = context.ActionArguments["hookEvent"] as LineWebhookEvent;
            if (hookEvent != null)
            {
                //LINE說要處理每個事件，不知道啥時會有這種例子，預先都處理第一個
                _lineService.InitServiceData(hookEvent.Events[0]);
            }
            else
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}

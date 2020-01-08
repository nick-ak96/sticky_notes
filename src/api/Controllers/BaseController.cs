using System;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
	public abstract class BaseController : ControllerBase
	{
		protected long CurrentUserId
		{
			get {
				var userId = Request.HttpContext.Items["user"] as long?;
				if (userId == null || !userId.HasValue)
					throw new Exception("Unable to get user ID from request");
				return userId.Value;
			}
		}
	}
}

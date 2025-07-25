using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace KitapApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class ApiAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _role;
        public ApiAuthorizeAttribute(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(role) || !string.Equals(role, _role, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new ForbidResult();
            }
        }
    }
} 
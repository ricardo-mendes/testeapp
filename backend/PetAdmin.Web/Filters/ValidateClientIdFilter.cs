using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using PetAdmin.Web.Services;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace PetAdmin.Web.Filters
{
    public class ValidateClientIdFilter : ActionFilterAttribute
    {
        private const string _tokenClientIdName = "ClientId";
        private string _clientIdName;

        public ValidateClientIdFilter()
        {
            _clientIdName = _tokenClientIdName;
        }

        public ValidateClientIdFilter(string clientIdName)
        {
            _clientIdName = clientIdName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _tokenManager = ResolveTokenManager(context);
            var authorized = true;

            object clientId = null;

            clientId = GetLoggedClientId(context, _tokenManager, clientId);

            if (IsValidRequestMethod(context))
            {
                foreach (ControllerParameterDescriptor param in context.ActionDescriptor.Parameters)
                {
                    context.ActionArguments.TryGetValue(param.Name, out var modelValue);

                    if (modelValue == null)
                        continue;

                    if (_clientIdName == null || clientId == null)
                    {
                        authorized = false;
                        break;
                    }

                    if (param.Name.ToLower() == _clientIdName.ToLower())
                    {
                        if (!IsValid(modelValue.ToString(), clientId.ToString()))
                            authorized = false;
                    }
                    else
                    {
                        var type = modelValue.GetType();

                        var clientIdList = type.GetProperties().Where(x => x.Name.ToLower().Equals(_clientIdName.ToLower()));

                        foreach (var prop in clientIdList)
                        {
                            var value = prop.GetValue(modelValue);

                            if (value == null)
                            {
                                authorized = false;
                                break;
                            }
                                
                            if (!IsValid(value.ToString(), clientId.ToString()))
                                authorized = false;
                        }
                    }
                }

                if (!authorized)
                    context.Result = new UnauthorizedResult();
            }

        }

        private bool IsValid(string valueModel, string valueInToken)
        {
            return valueModel == valueInToken;
        }

        private bool IsValidRequestMethod(ActionExecutingContext context)
        {
            var methods = new string[] { "GET", "POST", "DELETE", "PATCH", "PUT" }.ToList();
            var request = context.HttpContext.Request;
            var currentMethod = request.Method.ToUpper();

            var isValidMethod = methods.Contains(currentMethod);
            return isValidMethod;
        }

        private object GetLoggedClientId(ActionExecutingContext context, TokenService _tokenManager, object clientId)
        {
            var accessToken = context.HttpContext.GetBearerToken();

            var validToken = _tokenManager.VerifyToken(accessToken);

            if (validToken != null)
                validToken.Payload.TryGetValue(_tokenClientIdName, out clientId);
            return clientId;
        }

        private TokenService ResolveTokenManager(ActionExecutingContext context)
        {
            return context.HttpContext.RequestServices.GetService<TokenService>();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using PetAdmin.Web.Services;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace PetAdmin.Web.Filters
{
    public class ValidateScheduleFilter : ActionFilterAttribute
    {
        private const string _tokenPetLoverIdName = "PetLoverId";
        private const string _tokenClientIdName = "ClientId";
        private string _petLoverIdName;
        private string _clientIdName;

        public ValidateScheduleFilter()
        {
            _petLoverIdName = _tokenPetLoverIdName;
            _clientIdName = _tokenClientIdName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _tokenManager = ResolveTokenManager(context);
            var authorized = true;

            object petLoverId = null;
            object clientId = null;

            petLoverId = GetLoggedPetLoverId(context, _tokenManager, petLoverId);
            clientId = GetLoggedClientId(context, _tokenManager, clientId);

            if (IsValidRequestMethod(context))
            {
                foreach (ControllerParameterDescriptor param in context.ActionDescriptor.Parameters)
                {
                    context.ActionArguments.TryGetValue(param.Name, out var modelValue);

                    if (modelValue == null)
                        continue;

                    if (!string.IsNullOrEmpty(petLoverId.ToString()) &&  !string.IsNullOrEmpty(_petLoverIdName.ToString()))
                    {
                        if (param.Name.ToLower() == _petLoverIdName.ToLower())
                        {
                            if (!IsValid(modelValue.ToString(), petLoverId.ToString()))
                                authorized = false;
                        }
                        else
                        {
                            var type = modelValue.GetType();

                            var petLoverIdList = type.GetProperties().Where(x => x.Name.ToLower().Equals("petloverid"));

                            foreach (var prop in petLoverIdList)
                            {
                                var value = prop.GetValue(modelValue);

                                if (value == null)
                                    continue;

                                if (!IsValid(value.ToString(), petLoverId.ToString()))
                                    authorized = false;
                            }
                        }
                    }
                }

                if (!authorized)
                    context.Result = new UnauthorizedResult();
            }

        }

        private bool IsValid(string valueModel, string valueInToken)
        {
            return valueModel == valueInToken || valueModel == "0";
        }

        private bool IsValidRequestMethod(ActionExecutingContext context)
        {
            var methods = new string[] { "GET", "POST", "DELETE", "PATCH", "PUT" }.ToList();
            var request = context.HttpContext.Request;
            var currentMethod = request.Method.ToUpper();

            var isValidMethod = methods.Contains(currentMethod);
            return isValidMethod;
        }

        private object GetLoggedPetLoverId(ActionExecutingContext context, TokenService _tokenManager, object petLoverId)
        {
            var accessToken = context.HttpContext.GetBearerToken();

            var validToken = _tokenManager.VerifyToken(accessToken);

            if (validToken != null)
                validToken.Payload.TryGetValue(_tokenPetLoverIdName, out petLoverId);
            return petLoverId;
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

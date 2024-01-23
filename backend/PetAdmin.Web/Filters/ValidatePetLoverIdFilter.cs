using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using PetAdmin.Web.Services;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace PetAdmin.Web.Filters
{
    public class ValidatePetLoverIdFilter : ActionFilterAttribute
    {
        private const string _tokenPetLoverIdName = "PetLoverId";
        private string _petLoverIdName = _tokenPetLoverIdName;

        public ValidatePetLoverIdFilter() { }

        public ValidatePetLoverIdFilter(string petLoverIdName) { _petLoverIdName = petLoverIdName; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _tokenManager = ResolveTokenManager(context);
            var authorized = true;

            object petLoverId = null;

            petLoverId = GetLoggedPetLoverId(context, _tokenManager, petLoverId);

            if (IsValidRequestMethod(context))
            {
                foreach (ControllerParameterDescriptor param in context.ActionDescriptor.Parameters)
                {
                    context.ActionArguments.TryGetValue(param.Name, out var modelValue);

                    if (modelValue == null)
                        continue;

                    if (_petLoverIdName == null || petLoverId == null)
                    {
                        authorized = false;
                        break;
                    }

                    if (param.Name.ToLower() == _petLoverIdName.ToLower())
                    {
                        if (!IsValid(modelValue.ToString(), petLoverId.ToString()))
                            authorized = false;
                    }
                    else
                    {
                        var type = modelValue.GetType();

                        var petLoverIdList = type.GetProperties().Where(x => x.Name.ToLower().Equals(_petLoverIdName.ToLower()));

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

        private object GetLoggedPetLoverId(ActionExecutingContext context, TokenService _tokenManager, object petLoverId)
        {
            var accessToken = context.HttpContext.GetBearerToken();

            var validToken = _tokenManager.VerifyToken(accessToken);

            if (validToken != null)
                validToken.Payload.TryGetValue(_tokenPetLoverIdName, out petLoverId);
            return petLoverId;
        }

        private TokenService ResolveTokenManager(ActionExecutingContext context)
        {
            return context.HttpContext.RequestServices.GetService<TokenService>();
        }
    }
}

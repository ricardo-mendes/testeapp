using FluentValidator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetAdmin.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly NotificationHandler _notificationHandler;

        public BaseController(
            UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BaseController(
            UnitOfWork unitOfWork,
            NotificationHandler notificationHandler)
        {
            _unitOfWork = unitOfWork;
            _notificationHandler = notificationHandler;
        }

        public async Task<IActionResult> Response(object result, IEnumerable<Notification> notifications)
        {
            if ((notifications != null && !notifications.Any()) || notifications == null)
            {
                try
                {
                    _unitOfWork.Commit();
                    if (result == null)
                        return StatusCode(StatusCodes.Status200OK);
                    else
                        return StatusCode(StatusCodes.Status200OK, result);
                }
                catch(Exception ex)
                {
                    // Pode Logar o erro no Elmah
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        errors = new[] { ex.Message }
                    });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    errors = notifications
                });
            }
        }

        public async Task<IActionResult> Response(object result)
        {
            if (!_notificationHandler.HasNotification())
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    errors = _notificationHandler.GetNotifications()
                });
            }
        }

        protected void RaisError(string message)
        {
            _notificationHandler.Raise(message);
        }
    }
}
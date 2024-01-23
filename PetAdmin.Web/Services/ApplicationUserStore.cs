//using PetAdmin.Web.Models;
//using System;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace PetAdmin.Web.Services.Notifications
//{
//    public class ApplicationUserStore : UserStore<User, Roles, Guid, UserLogin, UserRole, UserClaim>
//    {
//    }

//    public class ApplicationRoleStore : RoleStore<ApplicationRole, string, ApplicationUserRole>
//    {
//        public ApplicationRoleStore(MyAppDb context)
//            : base(context)
//        {
//        }
//    }

//    public class ApplicationRoleManager : RoleManager<ApplicationRole>
//    {
//        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store)
//            : base(store)
//        {
//        }

//    }
//}

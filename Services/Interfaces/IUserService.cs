using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Model.Models;

namespace Services.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        User Authenticate(string login,string mdp);
        User Me(ClaimsPrincipal claimsPrincipal);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Model.Models;

namespace Services.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        User Authenticate(string login,string mdp);
    }
}

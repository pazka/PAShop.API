using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Models;
using Services.Interfaces;

namespace PAShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CORS")]
    [Authorize(Roles = "Admin")]
    public class TransactionsController : GenericController<Transaction>
    {
        public TransactionsController(IGenericService<Transaction> service, IHttpContextAccessor httpContextAccessor) : base(service,httpContextAccessor)
        {

        }
    }
}
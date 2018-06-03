using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiralWorks.Interfaces;
using SpiralWorks.Model;

namespace SpiralWorks.Web.Helpers
{

    public class BaseController : Controller
    {
        internal ISession _session;
        internal User _currentUser;
        internal IUnitOfWork _uow;
        protected BaseController(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _session = httpContextAccessor.HttpContext.Session;
            var user = _session.Get<User>("CurrentUser");
            _currentUser = user ?? null;



        }
    }
}
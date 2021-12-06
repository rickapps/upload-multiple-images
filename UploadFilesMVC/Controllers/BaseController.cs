using Microsoft.AspNetCore.Mvc;
using RickApps.UploadFilesMVC.Interfaces;

namespace RickApps.UploadFilesMVC.Controllers
{
    // This controller contains the uow class containing
    // our EFContext db connection. 
    public abstract class BaseController:Controller
    {
        protected readonly IUnitOfWork _repository;
        public BaseController(IUnitOfWork uow)
        {
            _repository = uow;
        }
    }
}

using ShopApp.Models;
using ShopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.Controllers
{
    [Authorize(Roles = "Staff")]
    public class ShopController : BaseController
    {
        // GET: Shop
        public ActionResult Index()
        {
            IsUserLoggedSession();

            return View();
        }

        public ActionResult MyProfile()
        {
            var user = _userManager.CreateOrRetrieve(User.Identity.Name, ref ErrorMessage);

            return View(user);
        }
        [HttpPost]
        public ActionResult MyProfile(UserInformation userInf)
        {
            if (_userManager.UpdateUserInformation(userInf, ref ErrorMessage) == Utils.ErrorCode.Error)
            {
                //
                ModelState.AddModelError(String.Empty, ErrorMessage);
                //
                return View(userInf);
            }
            TempData["Message"] = $"User Information {ErrorMessage}!";
            return View(userInf);
        }
        public ActionResult MyStore()
        {
            var store = _storeManager.CreateOrRetrieve(User.Identity.Name, ref ErrorMessage);
            return View(store);
        }
        [HttpPost]
        public ActionResult MyStore(Store store)
        {
            if(_storeManager.UpdateStore(store.storeId, store, ref ErrorMessage) == Utils.ErrorCode.Error)
            {
                ModelState.AddModelError(String.Empty, ErrorMessage);
                return View(store);
            }

            TempData["Message"] = $"Store {ErrorMessage}!";

            return View(store);
        }

        public ActionResult Brand()
        {
            return View(_branManager.ListBrand(Username));
        }
        [HttpGet]
        public JsonResult BrandGetById(int? id)
        {
            return Json(_branManager.GetBrandById(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult BrandCreate(Brand brand)
        {
            var res = new Response();
            brand.userId = UserId;
            brand.dateCreated = DateTime.Now;
            brand.status = (Int32)Status.Active;
            //
            res.code = (Int32)_branManager.CreateBrand(brand, ref ErrorMessage);
            res.message = ErrorMessage;

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BrandUpdate(Brand brand)
        {
            var res = new Response();

            res.code = (Int32)_branManager.UpdateBrand(brand, ref ErrorMessage);
            res.message = ErrorMessage;

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BrandDelete(int? id)
        {
            var res = new Response();
            res.code = (Int32)_branManager.DeleteBrand(id, ref ErrorMessage);
            res.message = ErrorMessage;

            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}
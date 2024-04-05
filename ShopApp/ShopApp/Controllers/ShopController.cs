using ShopApp.Models;
using ShopApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
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
        #region Store and Staff Management
        public ActionResult MyProfile()
        {
            IsUserLoggedSession();
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
        #endregion
        #region Brand Management
        public ActionResult Brand()
        {
            IsUserLoggedSession();
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
            brand.dateCreated = DateTime.Now;
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
        #endregion
        #region Category Management
        public ActionResult Category()
        {
            IsUserLoggedSession();
            return View(_categoryManager.ListCategory(Username));
        }
        [HttpGet]
        public JsonResult CategoryGetById(int? id)
        {
            return Json(_categoryManager.GetCategoryById(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CategoryCreate(Category category)
        {
            var res = new Response();
            category.userId = UserId;
            category.dateCreated = DateTime.Now;
            //
            res.code = (Int32)_categoryManager.CreateCategory(category, ref ErrorMessage);
            res.message = ErrorMessage;

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CategoryUpdate(Category category)
        {
            var res = new Response();
            category.dateCreated = DateTime.Now;
            res.code = (Int32)_categoryManager.UpdateCategory(category, ref ErrorMessage);
            res.message = ErrorMessage;

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CategoryDelete(int? id)
        {
            var res = new Response();
            res.code = (Int32)_categoryManager.DeleteCategory(id, ref ErrorMessage);
            res.message = ErrorMessage;

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Product management
        public ActionResult Product()
        {
            IsUserLoggedSession();
            return View(_productManager.ListProduct(Username));
        }
        public ActionResult CreateProduct()
        {
            ViewBag.Brand = Utilities.SelectListItemBrandByUser(Username);
            ViewBag.Category = Utilities.SelectListItemCategoryByUser(Username);
            return View();
        }
        [HttpPost]
        public ActionResult CreateProduct(Product product, HttpPostedFileBase[] files)
        {
            var prodgUid = $"Item-{Utilities.gUid}";
            //
            product.productgUId = prodgUid;
            product.userId = UserId;
            product.dateCreated = DateTime.Now;
            product.status = ProductStatus.OK;

            if (_productManager.CreateProduct(product, ref ErrorMessage) == ErrorCode.Error)
            {
                ModelState.AddModelError(String.Empty, ErrorMessage);
                return View(product);
            }

            product = _productManager.GetProductBygUId(prodgUid);

            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        if (!Directory.Exists(Server.MapPath("~/UploadedFiles/")))
                            Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/"));

                        var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);
                        //
                        Image img = new Image();
                        img.imageFile = InputFileName;
                        img.productId = product.productId;

                        _imgMgr.CreateImg(img, ref ErrorMessage); 

                    }

                }
            }
            return View(product);
        }
        #endregion
    }
}
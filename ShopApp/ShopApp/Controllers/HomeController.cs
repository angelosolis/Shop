using ShopApp.Models;
using ShopApp.Repository;
using ShopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShopApp.Controllers
{
    [Authorize(Roles ="Customer")]
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            IsUserLoggedSession();

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(String ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            ViewBag.Error = String.Empty;
            ViewBag.ReturnUrl = ReturnUrl;

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(String username, String password, String ReturnUrl)
        {
            if (_userManager.SignIn(username, password, ref ErrorMessage) == ErrorCode.Success)
            {
                var user = _userManager.GetUserByUsername(username);

                if (user.status != (Int32)Status.Active)
                {
                    TempData["username"] = username;
                    return RedirectToAction("Verify");
                }
                //
                FormsAuthentication.SetAuthCookie(username, false);
                //
                if (!String.IsNullOrEmpty(ReturnUrl))
                    return Redirect(ReturnUrl);

                switch (user.Role.roleName)
                {
                    case Constant.Role_Customer:
                        return RedirectToAction("Index");
                    case Constant.Role_Staff:
                        return RedirectToAction("Index", "Shop");
                    default:
                        return RedirectToAction("Index");
                }
            }
            ViewBag.Error = ErrorMessage;

            return View();
        }
        [AllowAnonymous]
        public ActionResult Verify()
        {
            if (String.IsNullOrEmpty(TempData["username"] as String))
                return RedirectToAction("Login");
            
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Verify(String code, String username)
        {
            if (String.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            TempData["username"] = username;

            var user = _userManager.GetUserByUsername(username);

            if (!user.code.Equals(code))
            {
                TempData["error"] = "Incorrect Code";
                return View();
            }

            user.status = (Int32)Status.Active;
            _userManager.UpdateUser(user, ref ErrorMessage);

            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            ViewBag.Role = Utilities.ListRole;

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SignUp(UserAccount ua, String ConfirmPass)
        {
            if (!ua.password.Equals(ConfirmPass))
            {
                ModelState.AddModelError(String.Empty, "Password not match");
                ViewBag.Role = Utilities.ListRole;
                return View(ua);
            }

            if (_userManager.SignUp(ua, ref ErrorMessage) != ErrorCode.Success)
            {
                ModelState.AddModelError(String.Empty, ErrorMessage);

                ViewBag.Role = Utilities.ListRole;
                return View(ua);
            }
            TempData["username"] = ua.username; 
            return RedirectToAction("Verify");
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [Authorize]
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
        public ActionResult Shop()
        {
            var products = _productManager.ListActiveProduct();
            return View(products);
        }

        public JsonResult AddCart(int prodId, int qty)
        {
            var res = new Response();

            if (_orderMgr.AddCart(UserId, prodId, qty, ref ErrorMessage) == ErrorCode.Error)
            {
                res.code = (Int32)ErrorCode.Error;
                res.message = ErrorMessage;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

            res.code = (Int32)ErrorCode.Success;
            res.message = "Item Added!";

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null || id == 0)
                return RedirectToAction("PageNotFound");

            var productInfo = _productManager.GetProductById(id);

            return View(productInfo);

        }

        public ActionResult Cart()
        {
            return View(_orderMgr.GetOrderByUserId(UserId));
        }
        // handle increment qty
        [HttpPost]
        public ActionResult Cart(int qty, int orderDtId, String action)
        {
            switch (action)
            {
                case Constant.PLUS:
                    qty++;
                    break;
                case Constant.MINUS:
                    qty--;
                    break;
                case Constant.X:
                    _orderMgr.DeleteOrderDetail(orderDtId, ref ErrorMessage);
                    //
                    return View(_orderMgr.GetOrderByUserId(UserId));
            }

            // remove item
            if (qty <= 0)
            {
                _orderMgr.DeleteOrderDetail(orderDtId, ref ErrorMessage);
                //
                return View(_orderMgr.GetOrderByUserId(UserId));
            }
            //

            var orderDt = _orderMgr.GetOrderDetailById(orderDtId);
            orderDt.quantity = qty;

            _orderMgr.UpdateOrderDetail(orderDt.id, orderDt, ref ErrorMessage);


            return View(_orderMgr.GetOrderByUserId(UserId));
        }

        public JsonResult GetCartCount()
        {
            var res = new { count = _orderMgr.GetCartCountByUserId(UserId) };

            return Json(res, JsonRequestBehavior.AllowGet);
        } 

        [AllowAnonymous]
        public ActionResult PageNotFound()
        {
            return Content("Not Found Error 404");
        }
    }
}
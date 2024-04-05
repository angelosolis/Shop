using ShopApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.Controllers
{
    public class BaseController : Controller
    {
        public String ErrorMessage;
        public UserManager _userManager;
        public StoreManager _storeManager;
        public BrandManager _branManager;
        public CategoryManager _categoryManager;
        public ProductManager _productManager;
        public ImageAttachManager _imgMgr;

        public String Username { get { return User.Identity.Name; } }
        public String UserId { get { return _userManager.GetUserByUsername(Username).userId; } }

        public BaseController()
        {
            ErrorMessage = String.Empty;
            _userManager = new UserManager();
            _storeManager = new StoreManager();
            _branManager = new BrandManager();
            _categoryManager = new CategoryManager();
            _productManager = new ProductManager();
            _imgMgr = new ImageAttachManager();
        }

       
        public void IsUserLoggedSession()
        {
            UserLogged userLogged = new UserLogged();
            if (User != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    userLogged.UserAccount = _userManager.GetUserByUsername(User.Identity.Name);
                    userLogged.UserInformation = _userManager.CreateOrRetrieve(userLogged.UserAccount.username, ref ErrorMessage);
                    userLogged.Store = userLogged.UserInformation.storeId != null ? _storeManager.GetStoreById(userLogged.UserInformation.storeId) : null;
                }
            }
            Session["User"] = userLogged;
        }
    }
}
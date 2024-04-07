using ShopApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.Utils
{
    public enum ErrorCode
    {
        Success,
        Error
    }
    public enum Status
    {
        InActive,
        Active
    }

    public enum RoleType
    {
        Customer,
        Staff
    }

    public enum ProductStatus
    {
        NoStock,
        HasStock
    }

    public class Constant
    {
        public const string Role_Customer = "Customer";
        public const string Role_Staff = "Staff";

        public const int ERROR = 1;
        public const int SUCCESS = 0;
    }
   
    public class Utilities
    {
        public static String gUid {
            get {
                return Guid.NewGuid().ToString();
            }
        } 
        // Return random number for OTP
        public static int code {
            get {
                Random r = new Random();
                return r.Next(100000, 999999);
            }
        }

        public static List<SelectListItem> ListRole
        {
            get {
                BaseRepository<Role> role = new BaseRepository<Role>();
                var list = new List<SelectListItem>();
                foreach (var item in role.GetAll())
                {
                    var r = new SelectListItem
                    {
                        Text = item.roleName,
                        Value = item.roleId.ToString()
                    };

                    list.Add(r);
                }

                return list;
            }
        }

        public static List<SelectListItem> SelectListItemBrandByUser(String username)
        {
            BrandManager _brandMgr = new BrandManager();
                var list = new List<SelectListItem>();
                foreach (var item in _brandMgr.ListBrand(username))
                {
                    var r = new SelectListItem
                    {
                        Text = item.brandName,
                        Value = item.brandId.ToString()
                    };
                    list.Add(r);
                }

                return list;
        }

        public static List<SelectListItem> SelectListItemCategoryByUser(String username)
        {
            CategoryManager _categoryMgr = new CategoryManager();
            var list = new List<SelectListItem>();
            foreach (var item in _categoryMgr.ListCategory(username))
            {
                var r = new SelectListItem
                {
                    Text = item.categoryName,
                    Value = item.categoryId.ToString()
                };
                list.Add(r);
            }

            return list;
        }
    }
}
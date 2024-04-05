using ShopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.Repository
{
    public class BrandManager
    {
        private BaseRepository<Brand> _brand;
        private UserManager _userMgr;
        
        public BrandManager()
        {
            _brand = new BaseRepository<Brand>();
            _userMgr = new UserManager();
        }
        public List<Brand> ListBrand(String username)
        {
            var user = _userMgr.GetUserByUsername(username);
            return _brand._table.Where(m=>m.userId == user.userId).ToList();
        }

        public Brand GetBrandById(int? id)
        {
            return _brand.Get(id);
        }
        public ErrorCode CreateBrand(Brand brand, ref String err)
        {
            return _brand.Create(brand, out err);
        }
        public ErrorCode UpdateBrand(Brand brand, ref String err)
        {
            return _brand.Update(brand.brandId, brand, out err);
        }
        public ErrorCode DeleteBrand(int? id, ref String err)
        {
            return _brand.Delete(id, out err);
        }
    }
}
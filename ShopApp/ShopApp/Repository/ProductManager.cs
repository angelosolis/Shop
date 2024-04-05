using ShopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.Repository
{
    public class ProductManager
    {
        UserManager _userMgr;
        BaseRepository<Product> _product;
        public ProductManager()
        {
            _userMgr = new UserManager();
            _product = new BaseRepository<Product>();
        }

        public List<Product> ListProduct(String username)
        {
            var user = _userMgr.GetUserByUsername(username);
            return _product._table.Where(m => m.userId == user.userId).ToList();
        }

        public Product GetProductById(int? id)
        {
            return _product.Get(id);
        }

        public Product GetProductBygUId(String gUid)
        {
            return _product._table.Where(m => m.productgUId == gUid).FirstOrDefault();
        }

        public ErrorCode CreateProduct(Product prod, ref String err)
        {
            return _product.Create(prod, out err);
        }
    }
}
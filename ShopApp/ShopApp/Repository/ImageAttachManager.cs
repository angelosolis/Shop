using ShopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.Repository
{
    public class ImageAttachManager
    {
        BaseRepository<Image> _img;
        
        public ImageAttachManager()
        {
            _img = new BaseRepository<Image>();
        }

        public List<Image> ListImgAttachByProdId(int? id)
        {
            return _img._table.Where(m => m.productId == id).ToList();
        }
        public ErrorCode CreateImg(Image img, ref String err)
        {
            return _img.Create(img, out err);
        }
        public ErrorCode UpdateImg(Image img, ref String err)
        {
            return _img.Update(img.id,img, out err);
        }
        public ErrorCode DeleteImg(int? id, ref String err)
        {
            return _img.Delete(id, out err);
        }
    }
}
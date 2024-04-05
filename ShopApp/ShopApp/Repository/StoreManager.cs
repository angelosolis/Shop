using ShopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.Repository
{
    public class StoreManager
    {
        BaseRepository<Store> _store;
        BaseRepository<UserInformation> _userInfo;
        UserManager _userMgr;

       

        public StoreManager()
        {
            _store = new BaseRepository<Store>();
            _userInfo = new BaseRepository<UserInformation>();
            _userMgr = new UserManager();
        }
        public Store GetStoreById(int? id)
        {
            return _store.Get(id);
        }
        public Store GetStoreByGuId(String id)
        {
            return _store.GetAll().Where(m=>m.storeGuid == id).FirstOrDefault(); 
        }

        public ErrorCode UpdateStore(int id, Store store, ref String err)
        {
            return _store.Update(id, store, out err);
        }
        public Store CreateOrRetrieve(String username, ref String err)
        {
            var user = _userMgr.GetUserByUsername(username);
            var userInf = _userMgr.GetUserInfoByUserId(user.userId);
           
            if (userInf.storeId != null)
                return _store.Get(userInf.storeId);

            var store = new Store();
            store.storeGuid = Utilities.gUid;
            store.storeName = $"{user.username}.Store";

            if (_store.Create(store, out err) != ErrorCode.Success)
            {
                // Return Error
                return null;
            }
            store = GetStoreByGuId(store.storeGuid);
            // Update user information assign store id
            userInf.storeId = store.storeId;
            //
            _userInfo.Update(userInf.id, userInf, out err);

            return store;
        }
    }
}
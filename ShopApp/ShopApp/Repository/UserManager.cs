using ShopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.Repository
{
    public class UserManager
    {
        private BaseRepository<UserAccount> _userAcc;
        private BaseRepository<UserInformation> _userInf;
        public UserManager()
        {
            _userAcc = new BaseRepository<UserAccount>();
            _userInf = new BaseRepository<UserInformation>();
        }

        #region Get User By ---
        public UserAccount GetUserById(int Id)
        {
            return _userAcc.Get(Id);
        }
        public UserAccount GetUserByUserId(String userId)
        {
            return _userAcc._table.Where(m => m.userId == userId).FirstOrDefault();
        }
        public UserAccount GetUserByUsername(String username)
        {
            return _userAcc._table.Where(m => m.username == username).FirstOrDefault();
        }
        public UserAccount GetUserByEmail(String email)
        {
            return _userAcc._table.Where(m => m.email == email).FirstOrDefault();
        }
        #endregion
        public ErrorCode SignIn(String username, String password, ref String errMsg)
        {
            var userSignIn = GetUserByUsername(username);
            if (userSignIn == null)
            {
                errMsg = "User not exist!";
                return ErrorCode.Error;
            }

            if (!userSignIn.password.Equals(password))
            {
                errMsg = "Password is Incorrect";
                return ErrorCode.Error;
            }

            // user exist
            errMsg = "Login Successful";
            return ErrorCode.Success;
        }

        public ErrorCode SignUp(UserAccount ua, ref String errMsg)
        {
            ua.userId = Utilities.gUid;
            ua.code = Utilities.code.ToString();
            ua.date_created = DateTime.Now;
            ua.status = (Int32)Status.InActive;

            if (GetUserByUsername(ua.username) != null)
            {
                errMsg = "Username Already Exist";
                return ErrorCode.Error;
            }

            if (GetUserByEmail(ua.email) != null)
            {
                errMsg = "Email Already Exist";
                return ErrorCode.Error;
            }

            if (_userAcc.Create(ua, out errMsg) != ErrorCode.Success)
            {
                return ErrorCode.Error;
            }

            // use the generated code for OTP "ua.code"
            // send email or sms here...........

            return ErrorCode.Success;
        }

        public ErrorCode UpdateUser(UserAccount ua, ref String errMsg)
        {
            return _userAcc.Update(ua.id, ua, out errMsg);
        }
        public ErrorCode UpdateUserInformation(UserInformation ua, ref String errMsg)
        {
            return _userInf.Update(ua.id, ua, out errMsg);
        }
        public UserInformation GetUserInfoById(int id)
        {
            return _userInf.Get(id);
        }
        public UserInformation GetUserInfoByUsername(String username)
        {
            var userAcc = GetUserByUsername(username);
            return _userInf._table.Where(m => m.userId == userAcc.userId).FirstOrDefault();
        }
        public UserInformation GetUserInfoByUserId(String userId)
        {
            return _userInf._table.Where(m => m.userId == userId).FirstOrDefault();
        }
        public UserInformation CreateOrRetrieve(String username, ref String err)
        {
            var User = GetUserByUsername(username);
            var UserInfo = GetUserInfoByUserId(User.userId);
            if (UserInfo != null)
                return UserInfo;

            UserInfo = new UserInformation();
            UserInfo.userId = User.userId;
            UserInfo.active = (Int32)Status.Active;

            _userInf.Create(UserInfo,out err);

            return GetUserInfoByUserId(User.userId);
        }
    }
}
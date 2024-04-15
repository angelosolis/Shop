using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp;
using ShopApp.Utils;

namespace ShopApp.Repository
{
    public class OrderManager
    {
        public String Message = String.Empty;
        //
        shopEntities _db;
        BaseRepository<Order> _order;
        BaseRepository<OrderDetail> _orderDetail;
        UserManager _userMgr;
        ProductManager _productMgr;
        public OrderManager()
        {
            _db = new shopEntities();
            _order = new BaseRepository<Order>();
            _userMgr = new UserManager();
            _productMgr = new ProductManager();
            _orderDetail = new BaseRepository<OrderDetail>();
        }

        public Order GetOrCreateOrderByUserId(String userId, Product prod, ref String err)
        {
            String orderNo = String.Empty;

            var user = _userMgr.GetUserInfoByUserId(prod.userId);

            var order = _order._table.Where(m => m.userId == userId && m.storeId == user.storeId).FirstOrDefault();
            if (order == null || order.orderStatus != (Int32)OrderStatus.Open)
            {
                order = new Order();
                order.userId = userId; 
                order.orderStatus = (Int32)OrderStatus.Open;
                order.storeId = user.storeId;
                order.orderCreated = DateTime.Now;

                _order.Create(order, out err);

                return order;
            }

            return order;
        }

        public ErrorCode AddCart(String userId, int productId, int qty, ref String error)
        {
            var product = _productMgr.GetProductById(productId);
            if (product == null)
            {
                error = "Not Found";
                return ErrorCode.Error;
            }

            var order = GetOrCreateOrderByUserId(userId, product, ref error);
            var orDetail = new OrderDetail();
            orDetail.orderId = order.orderId;
            orDetail.productId = productId;
            orDetail.quantity = qty;
            orDetail.listPrice = product.listPrice;

            if (AddUpdateCartQty(orDetail, order) == ErrorCode.Error)
            {
                error = Message;
                return ErrorCode.Error;
            }


            return ErrorCode.Success;
        }

        public ErrorCode AddUpdateCartQty(OrderDetail orderItem, Order order)
        {
            try
            {
                String err = String.Empty;
                var lproduct = _productMgr.GetProductById(orderItem.productId);
                var lOrderItem = _order.Get(order.orderId).OrderDetail.Where(m=>m.productId == orderItem.productId).FirstOrDefault();
                if (lOrderItem == null)
                {

                    return _orderDetail.Create(orderItem, out Message);
                }
                // retrieve the order detail to update qty
                var orDt = _orderDetail.Get(lOrderItem.id);
                orDt.quantity += orderItem.quantity;
                
                return _orderDetail.Update(orDt.id, orDt, out Message);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return ErrorCode.Error;
            }
            
        }

        public List<Order> GetOrderByUserId(String userId)
        {
            return _order._table.Where(m => m.userId == userId && m.orderStatus == (Int32)OrderStatus.Open).ToList();
        }

        public int GetCartCountByUserId(String userId)
        {
            var count = _db.sp_getCartCountByUserId(userId).FirstOrDefault();
            return (Int32)count;
        }
        public OrderDetail GetOrderDetailById(int id)
        {
            return _orderDetail.Get(id);
        }

        public ErrorCode UpdateOrderDetail(int id, OrderDetail orderDt, ref String err)
        {
            return _orderDetail.Update(id, orderDt, out err);
        }

        public ErrorCode DeleteOrderDetail(int id, ref String err)
        {
            return _orderDetail.Delete(id, out err);
        }
        
    }
}
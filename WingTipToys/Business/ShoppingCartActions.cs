using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WingTipToys.Models;

namespace WingTipToys.Business
{
    public class ShoppingCartActions : IDisposable
    {

        #region Properties and fields
        public const string CartSessionkey = "CART_ID";

        public string ShoppingCartId { get; set; }

        private decimal TotalCart;

        private ProductContext productContext;
        #endregion


        #region Methods for manipulation shopping cart

        public void AddToCart(int productId)
        {
            ShoppingCartId = GetCartId();
            var context = GetContext();

            var cartItem = productContext
                             .ShoppingCartItems
                             .SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == productId);


            if (cartItem == null)
            {

                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    CartId = ShoppingCartId,
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };

                var product = productContext.Products.SingleOrDefault(i => i.ProductID == productId);
                cartItem.ProductId = product.ProductID;
                cartItem.Product = product;

                productContext.ShoppingCartItems.Add(cartItem);

            }
            else
            {
                cartItem.Quantity++;
            }


            SaveChangesModel();
        }

        public IEnumerable<CartItem> GetCartItems()
        {

            ShoppingCartId = GetCartId();
            var context = GetContext();

            return productContext
                     .ShoppingCartItems
                     .Include(p => p.Product)
                     .Where(c=>c.CartId == ShoppingCartId)
                     .AsEnumerable();

        }

        private string GetCartId()
        {
           
            if (HttpContext.Current.Session[CartSessionkey] == null)
            {
                // If the user is logged in the app we recupered the user name of the session
                if (!String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                    HttpContext.Current.Session[CartSessionkey] = HttpContext.Current.User.Identity.Name;


                // If the user isn't logged in the app we created an guid id temporary
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionkey] = tempCartId.ToString();
                }

            }

            return HttpContext.Current.Session[CartSessionkey].ToString();
        }

        public decimal GetTotal()
        {

            if (TotalCart != 0)
                return TotalCart;
            else
            {

                ShoppingCartId = GetCartId();
                var context = GetContext();
                decimal? total = decimal.Zero;

                total = (decimal?)(from cartItem in context.ShoppingCartItems
                                   join productItem in context.Products on cartItem.ProductId equals productItem.ProductID
                                   where cartItem.CartId == ShoppingCartId
                                   select cartItem.Quantity * cartItem.Product.UnitPrice).Sum();

                return TotalCart = total ?? decimal.Zero;
            }
            
          
        }

        #endregion


        #region Disposibles for shopping cart and methods auxiliaries

        public void Dispose()
        {
            ShoppingCartId = null;
            DisposibleContext();
        }

        private ProductContext GetContext()
        {
            if (productContext == null)
                productContext = new ProductContext();

            return productContext;

        }

        private void DisposibleContext() => productContext.Dispose();

        private void SaveChangesModel()  => productContext.SaveChanges();
        
        #endregion

    }
}
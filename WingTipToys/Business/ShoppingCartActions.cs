using System;
using System.Collections.Generic;
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

        private ProductContext productContext { get { return GetContext(); } set { productContext = value; } }
        #endregion


        #region Methods for manipulation shopping cart

        public void AddToCart(int productId)
        {
            ShoppingCartId = GetCartId();

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

        }

        public IEnumerable<CartItem> GetCartItems()
        {

            ShoppingCartId = GetCartId();

            return productContext
                     .ShoppingCartItems
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

        #endregion



        #region Disposibles for shopping cart
        public void Dispose()
        {
            DisposibleContext();
        }

        private ProductContext GetContext()
        {
            if (productContext == null)
                productContext = new ProductContext();

            return productContext;

        }

        private void DisposibleContext() => productContext.Dispose();

        #endregion

    }
}
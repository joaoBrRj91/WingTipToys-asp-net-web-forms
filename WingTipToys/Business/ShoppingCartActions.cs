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
        private const string cartSessionkey = "CART_ID";

        private string shoppingCartId;

        private decimal totalCart;

        private int totalCartItems;

        private ProductContext productContext;

        private bool isChangeItemsOfTheShoppingCart = false;
        #endregion


        #region Methods for manipulation shopping cart

        public void AddToCart(int productId)
        {
            shoppingCartId = GetCartId();
            var context = GetContext();

            var cartItem = productContext
                             .ShoppingCartItems
                             .SingleOrDefault(c => c.CartId == shoppingCartId && c.ProductId == productId);


            if (cartItem == null)
            {

                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    CartId = shoppingCartId,
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

            shoppingCartId = GetCartId();
            var context = GetContext();

            return productContext
                     .ShoppingCartItems
                     .Include(p => p.Product)
                     .Where(c => c.CartId == shoppingCartId)
                     .AsEnumerable();

        }

        public CartItem GetItemToCart()
        {
            var cartId = GetCartId();

            return GetContext().ShoppingCartItems.Find(cartId);
        }

        public CartItem GetCartItemByCartIdAndProductId(string cartId, int productId)
        {
            return productContext.ShoppingCartItems.Where(i => i.CartId == cartId && i.ProductId == productId).FirstOrDefault();
        }

        public string GetCartId()
        {

            if (HttpContext.Current.Session[cartSessionkey] == null)
            {
                // If the user is logged in the app we recupered the user name of the session
                if (!String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                    HttpContext.Current.Session[cartSessionkey] = HttpContext.Current.User.Identity.Name;


                // If the user isn't logged in the app we created an guid id temporary
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[cartSessionkey] = tempCartId.ToString();
                }

            }

            return HttpContext.Current.Session[cartSessionkey].ToString();
        }

        public decimal GetTotal()
        {

            if (totalCart > 0 && !isChangeItemsOfTheShoppingCart)
                return totalCart;
            else
            {
                decimal? total = decimal.Zero;
                var context = GetContext();
                var cartId = GetCartId();

                total = (decimal?)(from cartItem in context.ShoppingCartItems
                                   join productItem in context.Products on cartItem.ProductId equals productItem.ProductID
                                   where cartItem.CartId == cartId
                                   select cartItem.Quantity * cartItem.Product.UnitPrice).Sum();


                return totalCart = total ?? decimal.Zero;

            }


        }

        public int GetCountCartItems()
        {
            shoppingCartId = GetCartId();
            GetContext();

            int? count = (from cartItem in productContext.ShoppingCartItems
                          join productItem in productContext.Products on cartItem.ProductId equals productItem.ProductID
                          where cartItem.CartId == shoppingCartId
                          select (int?)cartItem.Quantity).Sum();

            return totalCartItems = count ?? 0;

        }

        public void UpdateShoppingCartDatabase(ShoppinCartUpdates[] cartItemsUpdate)
        {

            try
            {

                foreach (var cartItem in cartItemsUpdate)
                {

                    if (cartItem.purchaseQuantity < 1 || cartItem.isRemovedItem)
                        RemoveItem(cartItem);
                    else
                        UpdateItem(cartItem);
                }

                isChangeItemsOfTheShoppingCart = true;
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: Unable to Update Cart in Database - Please contact the administrator of system" + ex.Message, ex);
            }


        }

        public void UpdateItem(ShoppinCartUpdates cartItem)
        {

            try
            {
                GetContext();
                shoppingCartId = GetCartId();

                var itemUpdate = GetCartItemByCartIdAndProductId(shoppingCartId, cartItem.produtoId);

                if (itemUpdate != null)
                {
                    itemUpdate.Quantity = cartItem.purchaseQuantity;
                    SaveChangesModel();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: Unable to Update item  of the Cart in Database - Please contact the administrator of system" + ex.Message, ex);

            }
        }

        public void RemoveItem(ShoppinCartUpdates cartItem)
        {

            try
            {
                var context = GetContext();

                var itemRemove = GetCartItemByCartIdAndProductId(shoppingCartId, cartItem.produtoId);

                if (itemRemove != null) { context.ShoppingCartItems.Remove(itemRemove); SaveChangesModel(); }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: Unable to Remove item  of the Cart in Database - Please contact the administrator of system" + ex.Message, ex);

            }

        }

        public void EmptyCart()
        {

            using (var context = GetContext())
            {

                var cartItems = context.ShoppingCartItems.Where(c => c.CartId == shoppingCartId).AsEnumerable();

                foreach (var item in cartItems)
                {
                    context.ShoppingCartItems.Remove(item);
                }

                SaveChangesModel();
            }

        }

        #endregion


        #region Disposibles for shopping cart and methods auxiliaries

        public void Dispose()
        {
            shoppingCartId = null;
            DisposibleContext();
        }

        private ProductContext GetContext()
        {
            if (productContext == null)
                productContext = new ProductContext();

            return productContext;

        }

        private void DisposibleContext() => productContext.Dispose();

        private void SaveChangesModel() => productContext.SaveChanges();

        #endregion

    }


    public struct ShoppinCartUpdates
    {
        public int produtoId;
        public int purchaseQuantity;
        public bool isRemovedItem;

    }

}
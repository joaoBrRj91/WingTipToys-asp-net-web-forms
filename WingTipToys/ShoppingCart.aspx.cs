using System;
using System.Collections.Generic;
using System.Linq;
using WingTipToys.Business;
using WingTipToys.Models;

namespace WingTipToys
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
            {

                decimal cartTotal = 0;
                cartTotal = usersShoppingCart.GetTotal();

                if (cartTotal > 0)
                    lblTotal.Text = String.Format("{0:c}", cartTotal);
                else
                {
                    LabelTotalText.Text = "";
                    lblTotal.Text = "";
                    ShoppingCartTitle.InnerText = "Shopping Cart is Empty";
                }

            }

        }


        public List<CartItem> GetShoppingCartItems()
        {

            using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
            {

                return usersShoppingCart.GetCartItems().ToList();
            }


        }

    }
}
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingTipToys.Business;

namespace WingTipToys
{

    //This class was created for a processing page soon this page don't have UI
    public partial class AddToCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string rawId = Request.QueryString["productID"];
            int productId;

            if (!String.IsNullOrEmpty(rawId) && int.TryParse(rawId, out productId))
            {

                using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
                {

                    usersShoppingCart.AddToCart(productId);
                }
            }
            else
            {
                Debug.Fail("ERROR : We should never get to AddToCart.aspx without a ProductId.");
                throw new Exception("ERROR : It is illegal to load AddToCart.aspx without setting a ProductId.");
            }

            Response.Redirect("ShoppingCart.aspx");

        }
    }
}
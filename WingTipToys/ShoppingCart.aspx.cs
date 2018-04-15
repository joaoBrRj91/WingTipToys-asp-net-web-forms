using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;
using WingTipToys.Business;
using WingTipToys.Models;

namespace WingTipToys
{
    public partial class ShoppingCart : System.Web.UI.Page
    {

        #region Events Handler

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
                    UpdateBtn.Visible = false;
                }

            }

        }


        protected void UpdateBtn_Click(Object sender, EventArgs e)
        {

            UpdateCartItems();
        }

 
        #endregion


        #region Selects Methods

        public List<CartItem> GetShoppingCartItems()
        {

            using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
            {

                return usersShoppingCart.GetCartItems().ToList();
            }


        }

        #endregion


        #region Private Methods

        private List<CartItem> UpdateCartItems()
        {
            using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
            {

                ShoppinCartUpdates[] cartUpdates = new ShoppinCartUpdates[CartList.Rows.Count];

                for (int i = 0; i < CartList.Rows.Count; i++)
                {

                    IOrderedDictionary rowValues = new OrderedDictionary();
                    rowValues = GetValues(CartList.Rows[i]);
                    cartUpdates[i].produtoId = Convert.ToInt32(rowValues["ProductId"]);

                    CheckBox cbRemove = new CheckBox();
                    cbRemove = (CheckBox)CartList.Rows[i].FindControl("Remove");
                    cartUpdates[i].isRemovedItem = cbRemove.Checked;


                    TextBox TbQuantity = new TextBox();
                    TbQuantity = (TextBox)CartList.Rows[i].FindControl("PurchaseQuantity");
                    cartUpdates[i].purchaseQuantity = Convert.ToInt32(TbQuantity.Text);

                }

                usersShoppingCart.UpdateShoppingCartDatabase(cartUpdates);
                CartList.DataBind();
                lblTotal.Text = String.Format("{0:c}", usersShoppingCart.GetTotal());
                return usersShoppingCart.GetCartItems().ToList();

            }

           
        }

        private IOrderedDictionary GetValues(GridViewRow row)
        {

            IOrderedDictionary values = new OrderedDictionary();

            foreach (DataControlFieldCell cell  in row.Cells)
            {

                if (cell.Visible)
                    // Extract values from the cell.
                    cell.ContainingField.ExtractValuesFromCell(values, cell,row.RowState, true);

            }
            return values;
        }

        #endregion

    }
}
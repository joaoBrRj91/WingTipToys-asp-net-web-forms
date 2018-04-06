using System;
using System.Linq;
using WingTipToys.Models;
using System.Web.ModelBinding;

namespace WingTipToys
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Product GetProduct([QueryString("id")] int? productId)
        {
            var db = new ProductContext();
            Product product = null;
            if (productId.HasValue && productId > 0)
                product = db.Products.Find(productId);

            return product;
        }
    }
}
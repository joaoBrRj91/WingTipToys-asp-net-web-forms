<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="WingTipToys.ShoppingCart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

     <div id="ShoppingCartTitle" runat="server" class="ContentHead">
        <h1>Shopping Cart</h1>
    </div>

    <asp:GridView
        ID="CartList"
        runat="server"
        AutoGenerateColumns="false"
        ShowFooter="true"
        GridLines="Vertical"
        CellPadding="4"
        ItemType=" WingTipToys.Models.CartItem"
        SelectMethod="GetShoppingCartItems"
        CssClass="table table-striped table-bordered table-hover table-condensed">
     

         <EmptyDataTemplate>
                <table>
                    <tr>
                        <td>No data was returned.</td>
                    </tr>
                </table>
          </EmptyDataTemplate>

        <Columns>
            <asp:BoundField DataField="ProductId" HeaderText="ID" SortExpression="ProductId" />
            <asp:BoundField DataField="Product.ProductName" HeaderText="Name" />
            <asp:BoundField DataField="Product.UnitPrice" HeaderText="Price" DataFormatString="{0:c}" />

            <asp:TemplateField HeaderText="Quantity">

                <ItemTemplate>
                    <asp:TextBox ID="PurchaseQuantity" Width="40" runat="server" Text="<%#: Item.Quantity %>"></asp:TextBox>
                </ItemTemplate>

            </asp:TemplateField>


            <asp:TemplateField HeaderText="Item Total">

                <ItemTemplate>
                    <%#: String.Format("{0:c}",(Convert.ToDouble(Item.Quantity) * Convert.ToDouble(Item.Product.UnitPrice))) %>
                </ItemTemplate>

            </asp:TemplateField>


            <asp:TemplateField HeaderText="Remove Item">

                <ItemTemplate>
                    <asp:CheckBox id="Remove"  runat="server"></asp:CheckBox>
                </ItemTemplate>

            </asp:TemplateField>

        </Columns>

    </asp:GridView>

    <div>
        <p></p>
        <strong>
            <asp:Label ID="LabelTotalText" runat="server" Text="Order Total: "></asp:Label>
            <asp:Label ID="lblTotal" runat="server" EnableViewState="false"></asp:Label>
        </strong>
    </div>
    <br/>

</asp:Content>



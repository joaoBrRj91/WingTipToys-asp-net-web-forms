<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductList.aspx.cs" Inherits="WingTipToys.ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section>

        <hgroup>
            <h2 style="margin-bottom: 15px"><%: Page.Title %></h2>
        </hgroup>


        <asp:ListView
            ID="productList"
            runat="server"
            DataKeyNames="ProductID"
            GroupItemCount="4"
            ItemType="WingTipToys.Models.Product"
            SelectMethod="GetProducts">

            <EmptyDataTemplate>
                <table>
                    <tr>
                        <td>No data was returned.</td>
                    </tr>
                </table>
            </EmptyDataTemplate>


            <GroupTemplate>
                <tr id="itemPlaceholderContainer" runat="server">
                    <td id="itemPlaceholder" runat="server"></td>
                </tr>
            </GroupTemplate>


            <ItemTemplate>
                <td runat="server">
                    <table>
                        <tr>
                            <td>
                                <a href="/ProductDetails.aspx?productID="<%#:Item.ProductID %>">
                                    <img src="Catalog/Images/Thumbs/<%#:Item.ImagePath %>" width="100" height="75" style="border:solid" />
                                </a>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <a href="/ProductDetails.aspx?id=<%#: Item.ProductID %>">
                                    <%#: Item.ProductName %>
                               </a>
                                <br/>
                                <span>
                                    <b>Price:</b><%#: String.Format("{0:c}", Item.UnitPrice) %>
                                </span>
                                <br/>
                                <a href="/AddToCart.aspx?productID=<%#:Item.ProductID %>">               
                                    <span class="ProductListItem"> <b>Add To Cart<b> </span>           
                                 </a>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                  <td>&nbsp;</td>
                            </td>
                        </tr>
                        
                       
                    </table>
                    <p></p>
                </td>
            </ItemTemplate>


            <LayoutTemplate>
               <table style="width:100%;">
                     <tbody>
                      <tr>
                          <td>
                             <table id="groupPlaceholderContainer" runat="server" style="width:100%;margin-right:auto;margin-left:auto">
                                  <tr id="groupPlaceholder"></tr>
                              </table>
                            </td>
                       </tr>
                        <tr>
                           <td></td>
                        </tr>
                          <tr></tr>
                     </tbody>
               </table>
            </LayoutTemplate>


        </asp:ListView>


    </section>
</asp:Content>

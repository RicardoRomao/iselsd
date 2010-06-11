<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ClientApplication.BlockBusterCinema.Movie>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ListMoviesByTitle
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        List Movies By Title</h2>
    <form action="/BlockBuster/ListMoviesByTitle" method="post">
        Enter keywords:&nbsp;
        <input type="text" name="keywords" /><br />
        <input type="submit" value="send" />
    </form>
    
    <% if (Model != null)
       {%>
        <table>
            <tr>
                <th>
                </th>
                <th>
                    Id
                </th>
                <th>
                    Title
                </th>
                <th>
                    Desc
                </th>
            </tr>
            <% foreach (var item in Model)
               { %>
                <tr>
                    <td>
                        Sessions
                    </td>
                    <td>
                        <%= Html.Encode(item.Id) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Title) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Desc) %>
                    </td>
                </tr>
            <% }%>
        </table>
    <% }%>
</asp:Content>

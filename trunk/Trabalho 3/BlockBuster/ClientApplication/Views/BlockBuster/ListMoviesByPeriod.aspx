<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ClientApplication.BlockBusterCinema.Movie>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ListMoviesByPeriod
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        List Movies By Period</h2>
    <form action="/BlockBuster/ListMoviesByPeriod" method="post">
        Enter start time (hh:mm:ss):&nbsp;
        <input type="text" name="start" /><br />
        Enter end time (hh:mm:ss):&nbsp;
        <input type="text" name="end" /><br />
        <input type="submit" value="send" />
    </form>
    
    <%if (Model != null)
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
            <%foreach (var item in Model)
              { %>
                <tr>
                    <td>
                        Sessions
                    </td>
                    <td>
                        <%= Html.Encode(item.Id)%>
                    </td>
                    <td>
                        <%= Html.Encode(item.Title)%>
                    </td>
                    <td>
                        <%= Html.Encode(item.Desc)%>
                    </td>
                </tr>
            <% } %>
        </table>
     <% } %>
</asp:Content>

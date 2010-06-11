<%@ Import Namespace="ClientApplication.BlockBusterCinema" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<ClientApplication.BlockBusterCinema.Movie>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ListMovies
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ListMovies</h2>
    <%foreach (Movie m in Model)
      {
          Response.Write(String.Format("{0} - {1} <br/> {2} <br/><br/>",
              m.Id, m.Title, m.Desc));
      }
     %>
</asp:Content>

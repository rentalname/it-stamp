﻿@ModelType SearchEventsViewModel

@For Each e In Model.Results
    @Html.Partial("_EventCard", e)
Next

<ul class="pagination">
    <li class="@(If(Model.CurrentPage = 1, "disabled", ""))">@Html.ActionLink("«", "Index", "Events", New With {.past = Model.Past}, Nothing)</li>
    @For i = Model.StartPage To Model.EndPage
        If i = Model.CurrentPage Then
        @<li class="active"><a href="@Request.RawUrl">@i<span class="sr-only">(current)</span></a></li>
        Else
        @<li>@Html.ActionLink(i, "Index", "Events", New With {.page = i, .past = Model.Past}, Nothing)</li>
        End If
    Next
    <li class="@(If(Model.CurrentPage = Model.TotalPages, "disabled", ""))">@Html.ActionLink("»", "Index", "Events", New With {.page = Model.TotalPages, .past = Model.Past}, Nothing)</li>
</ul>
﻿@ModelType Community
@Imports Microsoft.AspNet.Identity
@Code

    Dim icon = Href(Model.GetIconPath)

End Code

<div class="media">
    <div class="pull-left">
        @If ViewData("InternalLink") = True Then
            @<a href="@Href("/Communities/")@Model.Id">
                <img class="media-object img-rounded" src="@icon" alt="@Model.Name">
            </a>
        Else
            @If Model.Url <> "" Then
                @<a href="@Model.Url" target="_blank">
                    <img class="media-object img-rounded" src="@icon" alt="@Model.Name">
                </a>
            Else
                @<img class="media-object img-rounded" src="@icon" alt="@Model.Name">
            End If
        End If

        @If Request.IsAuthenticated AndAlso ViewData("Details") AndAlso Not Model.IsHidden Then
            @<div class="text-center">
                @Using Ajax.BeginForm("Follow", "Communities", New With {.id = Model.Id}, New AjaxOptions With {.HttpMethod = "POST", .OnSuccess = "onSuccess", .OnBegin = "onBegin"}, New With {.class = "form-horizontal", .role = "form"})
                    @Html.AntiForgeryToken()
                    @Html.HiddenFor(Function(m) m.Id)
                    @Html.HiddenFor(Function(m) m.Name)

                    @<div class="form-group">
                        <div class="form-inline">
                            <input id="follow-btn" type="submit" value="@(If(ViewBag.Followed, "フォロー中", "フォロー"))" class="btn btn-default" style="min-width:96px;width:96px;font-size:14px;" />
                        </div>
                    </div>
                End Using
            </div>
        End If
    </div>
    <div class="media-body">
        @If ViewData("Details") Then
            @If Model.Description <> "" Then
                @<p>@Html.Raw(Model.Description.TextWithUrl.Replace(vbCrLf, "<br />"))</p>
            End If
        Else
            @<h3><a href="@Href("/Communities/")@Model.Id">@Model.Name</a></h3>
            @<p class="small">@Html.Raw(Model.Description.Excerpt)</p>
        End If
        @If Model.Url <> "" Then
            @<a href="@Model.Url" target="_blank">@Model.Url</a>
        End If
    </div>
</div>

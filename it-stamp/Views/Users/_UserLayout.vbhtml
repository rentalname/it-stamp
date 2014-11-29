﻿@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@code
    Layout = "~/Views/Shared/_Layout.vbhtml"
    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")
    Dim action = ViewContext.RouteData.GetRequiredString("action")

End Code
<div class="row">
    <div class="col-md-8">

        @If User.Identity.GetUserId <> Model.Id OrElse action = "Details" Then
            @<div class="media">
                <div class="pull-left">
                    @If Model.Url <> "" Then
                        @<a href="@Model.Url" target="_blank">
                            <img class="media-object img-rounded" src="@icon" alt="@Model.FriendlyName">
                        </a>
                    Else
                        @<img class="media-object img-rounded" src="@icon" alt="@Model.FriendlyName">
                    End If

                    @If Request.IsAuthenticated AndAlso User.Identity.GetUserName <> Model.UserName Then
                        @<div class="text-center">
                            @Using Ajax.BeginForm("Follow", "Users", New With {.userName = Model.UserName}, New AjaxOptions With {.HttpMethod = "POST", .OnSuccess = "onSuccess", .OnBegin = "onBegin"}, New With {.class = "form-horizontal", .role = "form"})
                                @Html.AntiForgeryToken()

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
                    <p>@Html.Raw(Html.Encode(Model.Description).Replace(vbCrLf, "<br />"))</p>
                    @If Model.Url <> "" Then
                        @<p><a href="@Model.Url" target="_blank">@Model.Url</a></p>
                    End If
                </div>
            </div>
        End If

        <h1>@ViewBag.Title</h1>

        @RenderBody()

        @If action <> "My" Then
            @Html.Partial("_SocialButtons")
        End If
    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
@If Request.IsAuthenticated Then

    @section Styles
        <link href="@Href("~/Content/animate.css")" rel="stylesheet" />
    End Section
    @section Scripts
        @Scripts.Render("~/bundles/jqueryval")
        <script>
            var followed = @(If(ViewBag.Followed, "true", "false"));
            function onSuccess(result) {
                if (result) {
                    followed = result.followed;
                    $("#follow-btn")
                        .val(result.followed ? "フォロー中" : "フォロー")
                        .addClass("animated bounceIn")
                        .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend", function() {
                            $(this).removeClass("animated bounceIn");
                        });
                }
            }

            function onBegin() {
            }

            (function ($) {
                $("#follow-btn").hover(function () {
                    if (followed) {
                        $(this).val("解除").removeClass("btn-default").addClass("btn-primary");
                    } else {
                        $(this).removeClass("btn-default").addClass("btn-primary");
                    }
                },function () {
                    if (followed) {
                        $(this).val("フォロー中").removeClass("btn-primary").addClass("btn-default");
                    } else {
                        $(this).removeClass("btn-primary").addClass("btn-default");
                    }
                });
            })(jQuery);
        </script>
    End Section
End If

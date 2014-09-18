﻿Imports System.Net

Public Class Atnd

    Private Const EventIdUriFormat As String = "http://api.atnd.org/events/?event_id={0}"
    Private Const EventsUriFormat As String = "http://api.atnd.org/events/?keyword={0}&ymd={1}"

    Async Function GetEvent(id As String) As Threading.Tasks.Task(Of [Event])
        Dim content As String
        Using client = New WebClient
            client.Encoding = Text.Encoding.UTF8
            content = Await client.DownloadStringTaskAsync(New Uri(String.Format(EventIdUriFormat, id)))
        End Using

        Dim d = XDocument.Parse(content)
        Dim count As Integer
        If d.<results_returned> Is Nothing OrElse Not Integer.TryParse(d.<results_returned>.Value, count) Then
            Return Nothing
        End If

        Return ParseContent(d.<event>.First)
    End Function

    Async Function GetEvents(prefecture As Prefecture, startDate As DateTime) As Threading.Tasks.Task(Of IList(Of [Event]))
        Dim content As String
        Using client = New WebClient
            client.Encoding = Text.Encoding.UTF8
            content = Await client.DownloadStringTaskAsync(New Uri(String.Format(EventsUriFormat, prefecture.Name, startDate.ToString("yyyyMMdd"))))
        End Using

        Dim list = New List(Of [Event])

        Dim d = XDocument.Parse(content)
        Dim count As Integer
        If d.<results_returned> Is Nothing OrElse Not Integer.TryParse(d.<results_returned>.Value, count) Then
            Return list
        End If

        For Each e In d...<event>
            Dim ev = ParseContent(e)
            If ev IsNot Nothing Then
                list.Add(ev)
            End If
        Next

        Return list
    End Function


    Private Function ParseContent(e As XElement) As [Event]

        If e Is Nothing Then
            Return Nothing
        End If

        Dim st, ed As DateTime
        If Not DateTime.TryParse(e.<started_at>.Value, st) OrElse
           Not DateTime.TryParse(e.<ended_at>.Value, ed) Then
            ' Do nothing
        End If

        Dim address = If(e.<address>.Value, "")

        Dim m = New [Event] With {
            .Name = e.<title>.Value,
            .Description = "",
            .Url = e.<event_url>.Value,
            .Prefecture = address.GetPrefecture,
            .Address = address.RemovePrefecture,
            .Place = e.<place>.Value,
            .StartDateTime = st,
            .EndDateTime = ed
            }

        Return m
    End Function

End Class

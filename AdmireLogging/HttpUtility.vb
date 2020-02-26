Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Class RequestState
    Public request As HttpWebRequest = Nothing
    Public response As HttpWebResponse = Nothing
    Public json As String = Nothing
End Class

Public Module HttpUtility

    Public Sub sendPostRequestAsync(Of TRequest)(url As String, requestObj As TRequest)
        Try
            Dim req As HttpWebRequest = WebRequest.Create(url)
            req.Method = "POST"
            req.ContentType = "application/json"

            Dim rs = New RequestState
            Dim jsonSettings As JsonSerializerSettings = New JsonSerializerSettings
            jsonSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fff"
            jsonSettings.Formatting = Formatting.Indented
            Dim jsonData As String = JsonConvert.SerializeObject(requestObj, jsonSettings)
            rs.json = JsonData
            rs.request = req

            req.BeginGetRequestStream(New AsyncCallback(AddressOf finishRequestStream), rs)
        Catch ex As WebException
            handleWebException(ex)
        End Try
    End Sub

    Public Sub finishRequestStream(asyncResult As IAsyncResult)
        Try
            Dim rs = CType(asyncResult.AsyncState, RequestState)
            Dim req As HttpWebRequest = rs.request

            Dim postBytes As Byte() = Text.Encoding.UTF8.GetBytes(rs.json)
            Dim stream As Stream = req.EndGetRequestStream(asyncResult)
            stream.Write(postBytes, 0, postBytes.Length)
            stream.Close()

            req.BeginGetResponse(New AsyncCallback(AddressOf finishPostRequestAsync), rs)
        Catch ex As WebException
            handleWebException(ex)
        End Try
    End Sub

    Public Sub finishPostRequestAsync(asyncResult As IAsyncResult)
        Try
            Dim rs = CType(asyncResult.AsyncState, RequestState)
            Dim req As HttpWebRequest = rs.request
            rs.response = req.EndGetResponse(asyncResult)
            Dim responseBody = readAndCloseResponse(rs.response)
        Catch ex As WebException
            handleWebException(ex)
        End Try
    End Sub

    Private Function readAndCloseResponse(response As HttpWebResponse)
        Dim dataStream As Stream = response.GetResponseStream()
        Dim reader As New StreamReader(dataStream)
        readAndCloseResponse = reader.ReadToEnd()
        reader.Close()
        response.Close()
    End Function

    Private Sub handleWebException(ex As WebException)
        readAndCloseResponse(ex.Response)
    End Sub

End Module

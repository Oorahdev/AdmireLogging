Public Class ElasticsearchLogger

    Public Shared Sub LogException(ex As Exception)
        Dim url As String = "http://172.31.22.131:9200/admire-error-log/_doc"
        Dim log As LogItem = New LogItem
        Dim trace = New StackTrace(ex, True)

        log.PluginName = Reflection.Assembly.GetCallingAssembly.GetName.Name
        Dim pathParts As String() = trace.GetFrame(0).GetFileName.Split("\\")
        log.FileName = pathParts(pathParts.Length - 1)
        log.Method = trace.GetFrame(0).GetMethod.Name
        log.Line = trace.GetFrame(0).GetFileLineNumber().ToString
        log.Exception = ex.Message
        log.Details = trace.GetFrame(0).ToString
        log.DateTime = Now
        sendPostRequestAsync(url, log)

    End Sub

End Class

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Security.Cryptography.X509Certificates
Imports System.Text

Public Class Form1

    Private Sub testHttpClient()
        Dim PrivateKeyPath = Application.StartupPath + "\SANDBOXSITE_API.pfx"
        Dim CertificatePassword As String = "Zk7XD3FG5ycW2suc"
        Dim cert As New X509Certificate2(PrivateKeyPath, CertificatePassword, X509KeyStorageFlags.Exportable)

        Dim handler As New WebRequestHandler()
        handler.ClientCertificates.Add(cert)

        Dim client As New HttpClient(handler)

        client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded")
        client.DefaultRequestHeaders.Accept.Add(New Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"))

        client.DefaultRequestHeaders.Add("x-fapi-financial-id", "12345678")
        client.DefaultRequestHeaders.Add("Authorization", "Basic S212SGs1bERZaUIxUlB1cVNVODVZOFRVR25rWTE1OUE6TUMxZDdSQWRoUkdLQ2JTTw==")

        Dim formData As New FormUrlEncodedContent(New Dictionary(Of String, String) From {{"grant_type", "client_credentials"}})

        Dim response As HttpResponseMessage = client.PostAsync("https://card-sandbox.api.fiservapps.com/cs/oauth2/v1/token?scope=/cs/api", formData).Result

        If response.IsSuccessStatusCode Then
            Dim responseContent As String = response.Content.ReadAsStringAsync().Result
            '  Do something with the response
        Else
            'Handle error
        End If
    End Sub

    Private Sub testHttpRequest()


        Dim PrivateKeyPath = Application.StartupPath + "\SANDBOXSITE_API.pfx"
        Dim CertificatePassword As String = "Zk7XD3FG5ycW2suc"
        Dim cert As New X509Certificate2(PrivateKeyPath, CertificatePassword, X509KeyStorageFlags.Exportable)
        Dim certData As Byte() = cert.Export(X509ContentType.Pkcs12, CertificatePassword)
        Dim keyString As String = Convert.ToBase64String(certData)

        ServicePointManager.Expect100Continue = True

        Dim httpWebRequest = CType(WebRequest.Create("https://card-sandbox.api.fiservapps.com/cs/oauth2/v1/token?scope=/cs/api"), HttpWebRequest)
        httpWebRequest.ClientCertificates.Add(cert)
        httpWebRequest.ContentType = "application/x-www-form-urlencoded"
        httpWebRequest.Method = "POST"
        httpWebRequest.Headers("x-fapi-financial-id") = "12345678"
        ' httpWebRequest.Headers("x-fapi-financial-id") = "99990075"
        Dim AppKey As String = "KmvHk5lDYiB1RPuqSU85Y8TUGnkY159A"
        Dim secretvalue As String = "MC1d7RAdhRGKCbSO"

        Dim svcCredentials As String = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(AppKey + ":" + secretvalue))

        Dim body As String = "grant_type=client_credentials"
        Dim byteArray As Byte() = ASCIIEncoding.ASCII.GetBytes(body)
        httpWebRequest.ContentLength = byteArray.Length
        ' httpWebRequest.Headers("Authorization") = "Basic S212SGs1bERZaUIxUlB1cVNVODVZOFRVR25rWTE1OUE6TUMxZDdSQWRoUkdLQ2JTTw=="
        httpWebRequest.Headers.Add("Authorization", "Basic " + svcCredentials + "")
        Using reqStream = httpWebRequest.GetRequestStream()
            reqStream.Write(byteArray, 0, byteArray.Length)
        End Using

        Dim responseContent As String = String.Empty
        Using response = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse),
          responseStream = response.GetResponseStream(),
          reader = New StreamReader(responseStream)
            responseContent = reader.ReadToEnd()
            reader.Close()
        End Using

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        testHttpRequest()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        testHttpClient()
    End Sub
End Class

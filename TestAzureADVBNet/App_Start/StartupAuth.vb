Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.Owin.Extensions
Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.OpenIdConnect
Imports Owin

Partial Public Class Startup


    Private Shared clientId As String = ConfigurationManager.AppSettings("ida:ClientId")
        Private Shared aadInstance As String = EnsureTrailingSlash(ConfigurationManager.AppSettings("ida:AADInstance"))
        Private Shared tenantId As String = ConfigurationManager.AppSettings("ida:TenantId")
        Private Shared postLogoutRedirectUri As String = ConfigurationManager.AppSettings("ida:PostLogoutRedirectUri")
        Private authority As String = aadInstance & tenantId

        Public Sub ConfigureAuth(ByVal app As IAppBuilder)
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType)
            app.UseCookieAuthentication(New CookieAuthenticationOptions())
            app.UseOpenIdConnectAuthentication(New OpenIdConnectAuthenticationOptions With {
                .ClientId = clientId,
                .Authority = authority,
                .PostLogoutRedirectUri = postLogoutRedirectUri,
                .Notifications = New OpenIdConnectAuthenticationNotifications() With {
                    .AuthenticationFailed = Function(context) System.Threading.Tasks.Task.FromResult(0),
                    .SecurityTokenValidated = Function(context)
                                                  Dim claims = context.AuthenticationTicket.Identity.Claims
                                                  Dim groups = From c In claims Where c.Type = "groups" Select c

                                                  For Each group In groups
                                                      Dim groupStringValue = System.Configuration.ConfigurationManager.AppSettings(group.Value)

                                                      If groupStringValue IsNot Nothing Then
                                                          context.AuthenticationTicket.Identity.AddClaim(New Claim(ClaimTypes.Role, groupStringValue))
                                                      End If
                                                  Next

                                                  Return Task.FromResult(0)
                                              End Function
                }
            })
            app.UseStageMarker(PipelineStage.Authenticate)
        End Sub

        Private Shared Function EnsureTrailingSlash(ByVal value As String) As String
            If value Is Nothing Then
                value = String.Empty
            End If

            If Not value.EndsWith("/", StringComparison.Ordinal) Then
                Return value & "/"
            End If

            Return value
        End Function
    End Class


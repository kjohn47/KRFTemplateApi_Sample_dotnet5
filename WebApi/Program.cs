namespace KRFTemplateApi.WebApi
{
    using System.Net;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;

    using KRFTemplateApi.App.Constants;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)                
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel((c, o) =>
                    {
                        int httpPort = c.Configuration.GetValue(AppConstants.KestrelConfigurationHttpPort, 5051);
                        int httpsPort = c.Configuration.GetValue(AppConstants.KestrelConfigurationHttpsPort, 15051);
                        if (c.HostingEnvironment.IsDevelopment())
                        {
                            o.ListenLocalhost(httpsPort, l => l.UseHttps( h => {
                                h.AllowAnyClientCertificate();
                                h.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.NoCertificate;
                            }));
                            o.ListenLocalhost(httpPort);
                        }
                        else
                        {
                            o.Listen(IPAddress.Any, httpsPort, l => l.UseHttps(h => {
                                h.AllowAnyClientCertificate();
                                h.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.NoCertificate;
                            }));
                            o.Listen(IPAddress.Any, httpPort);
                        }
                    })
                    .UseStartup<Startup>();
                });
    }
}

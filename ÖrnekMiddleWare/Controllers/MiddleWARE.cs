using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

public class MiddleWARE
{
    private  RequestDelegate _next;

    public MiddleWARE(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // İstek öncesi işlemler
        string ip = context.Connection.RemoteIpAddress.ToString();
        string logMessage = $"[{DateTime.UtcNow.ToString()}] - IP: {ip}\n";

        // Dosyaya yazma işlemi için dosya yolunu oluşturma


        StreamWriter wri = new StreamWriter("c:\\ip\\ip.txt", true);
        wri.WriteLine(ip+" isimli ip apiye istek attı ");
        wri.Close();

        // Log mesajını dosyaya yazma
        await File.AppendAllTextAsync(ip,logMessage);

        // Sonraki middleware'e devretme
        await _next(context);
    }
}

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseIpLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MiddleWARE>();
    }
}

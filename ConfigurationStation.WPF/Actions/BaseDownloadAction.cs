using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationStation.WPF.Actions
{
    public abstract class BaseDownloadAction
    {
        protected async Task<Stream> DownloadAsync(string url, ActionContext context, CancellationToken cancellationToken)
        {
            var client = HttpClientFactory.Create();
            client.Timeout = new TimeSpan(0, 30, 0);
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

            if (cancellationToken.IsCancellationRequested) return null;

            var length = response.Content.Headers.ContentLength;

            context.SetProgressMax(length.GetValueOrDefault(0));

            var buffer = new byte[1024];
            var memoryStream = new MemoryStream();
            using (var data = await response.Content.ReadAsStreamAsync())
            {
                var totalBytesRead = 0;
                while (true)
                {
                    var bytesRead = await data.ReadAsync(buffer, 0, 1024);
                    if (bytesRead == 0) break;
                    await memoryStream.WriteAsync(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;

                    context.SetProgressValue(totalBytesRead);

                    if (cancellationToken.IsCancellationRequested) return null;
                }
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
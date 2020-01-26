using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationStation.WPF.Actions
{

    public class PpssppAssetsDownloadAction : BaseDownloadAction, IAction
    {
        const string ppsspp_source_url = "https://github.com/hrydgard/ppsspp/archive/master.zip";

        public async Task Execute(ActionContext context, CancellationToken cancellationToken)
        {
            try
            {
                var client = HttpClientFactory.Create();

                if (cancellationToken.IsCancellationRequested) return;

                var ppssppPath = Path.Combine(context.RetroArchPath, "system", "PPSSPP");
                if (true || !Directory.Exists(ppssppPath))
                {
                    context.UpdateMessage("Downloading ppsspp assets");

                    using (var memoryStream = await DownloadAsync(ppsspp_source_url, context, cancellationToken))
                    {
                        context.UpdateMessage($"Unpacking PPSSPP assets");

                        Compression.UnzipFromStream(memoryStream, "ppsspp-master/assets", ppssppPath, (s) =>
                        {
                            context.UpdateMessage($"Unpacking {s}");
                        });

                        context.UpdateMessage($"PPSSPP assets complete");
                    }

                }
            }
            catch (Exception ex)
            {
                context.UpdateMessage($"Downloading PPSSPP assets failed! {ex.Message}");
            }

        }




    }
}

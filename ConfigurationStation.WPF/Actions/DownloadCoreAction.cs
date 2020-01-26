using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationStation.WPF.Actions
{
    public class DownloadCoreAction : BaseDownloadAction, IAction
    {
        private string _coreUrl;
        private readonly bool _overwriteCores;

        public DownloadCoreAction(string coreUrl, bool overwriteCores)
        {
            _coreUrl = coreUrl;
            _overwriteCores = overwriteCores;
        }

        public async Task Execute(ActionContext context, CancellationToken cancellationToken)
        {
            try
            {
                var coreFile = Path.Combine(context.RetroArchPath, "cores", $"{context.Core.Name}.dll");
                if (_overwriteCores || !File.Exists(coreFile))
                {
                    context.UpdateMessage($"Downloading core {context.Core.Name}");
                    using (var memoryStream = await DownloadAsync($"{_coreUrl}/{context.Core.Name}.dll.zip", context, cancellationToken))
                    {
                        context.UpdateMessage($"Unpacking core {context.Core.Name}");
                        Compression.UnzipFileFromStream(memoryStream, $"{context.Core.Name}.dll", coreFile);
                        context.UpdateMessage($"{context.Core.Name} complete");
                    }
                }
            }
            catch (Exception ex)
            {
                context.UpdateMessage($"Downloading {context.Core.Name} failed! {ex.Message}");
            }

        }

        private string GetSize(int size)
        {
            if (size < 1048576)
            {
                size = size / 1024;
                return $"{size}KB";
            }
            else
            {
                size = size / 1048576;
                return $"{size}MB";
            }
        }
    }
}

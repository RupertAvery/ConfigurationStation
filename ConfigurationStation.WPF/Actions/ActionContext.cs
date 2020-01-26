using Config.RetroArch;
using System;

namespace ConfigurationStation.WPF.Actions
{
    public class ActionContext
    {
        public string RetroArchPath { get; set; }
        public RetroArchCore Core { get; set; }
        public Action<string> AddLog { get; set; }
        public Action<long> SetProgressMax { get; set; }
        public Action<long> SetProgressValue { get; set; }
        public Action<string> SetMessage { get; set; }
        public void UpdateMessage(string message)
        {
            message = message.Replace("_","__");
            //var us = message.IndexOf("_");
            //if(us > -1)
            //{
            //    message = message.Substring(0, us + 1) + "_" + message.Substring(us + 1);
            //}
            SetMessage?.Invoke(message);
        }
    }
}

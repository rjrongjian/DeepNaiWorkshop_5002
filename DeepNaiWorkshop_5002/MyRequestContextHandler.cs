using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_5002
{
    class MyRequestContextHandler : IRequestContextHandler
    {
        public ICookieManager GetCookieManager()
        {
            return null;
        }

        public bool OnBeforePluginLoad(string mimeType, string url, bool isMainFrame, string topOriginUrl, WebPluginInfo pluginInfo, ref PluginPolicy pluginPolicy)
        {

            bool blockPluginLoad = pluginInfo.Name.ToLower().Contains("flash");
            if (blockPluginLoad)
            {
                pluginPolicy = PluginPolicy.Disable;
            }
            return blockPluginLoad;
        }
    }
}

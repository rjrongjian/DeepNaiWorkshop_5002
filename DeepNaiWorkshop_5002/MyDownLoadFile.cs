using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefTest
{
    /// <summary>
    /// cefsharp下载到指定位置
    /// </summary>
    class MyDownLoadFile : IDownloadHandler
    {
        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    //showDialog 下载弹窗是否显示
                    /*
                    callback.Continue(@"D:\downloadCef\" +
                            System.Security.Principal.WindowsIdentity.GetCurrent().Name +
                            @"\Downloads\" +
                            downloadItem.SuggestedFileName,
                        showDialog: false);
                        */
                    callback.Continue(@"D:\downloadCef\" + downloadItem.SuggestedFileName,
                    showDialog: false);
                }
            }
        }

        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            //downloadItem.IsCancelled = false;  
        }
        public bool OnDownloadUpdated(CefSharp.DownloadItem downloadItem)
        {
            return false;
        }
    }
}

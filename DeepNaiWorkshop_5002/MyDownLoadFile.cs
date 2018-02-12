using CefSharp;
using HttpCodeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
                    //callback.Continue(@"D:\downloadCef\" + downloadItem.SuggestedFileName,showDialog: false);

                    XJHTTP xj = new XJHTTP();
                    string filepath = System.Windows.Forms.Application.StartupPath + "\\download";//当前项目目录
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    filepath = filepath + "\\" + xj.EncryptMD5String(DateTime.Now.ToString()) + "_"+ downloadItem.SuggestedFileName;
                    callback.Continue(filepath, showDialog: false);

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

using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using DeepNaiWorkshop_5002.MyTool;

namespace DeepNaiWorkshop_5002
{
    class MyRequestHandler: DefaultRequestHandler
    {
        private SourceFromData sourceFromData;

        public MyRequestHandler(SourceFromData sourceFromData)
        {
            this.sourceFromData = sourceFromData;
        }

        public override CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            String userAgent = UserAgent.randomUserAgent();
            //Console.WriteLine("设置的userAgent:" + userAgent);
            //Console.WriteLine("访问的url："+request.Url);
            //自定义header
            var headers = request.Headers;
            //headers["Custom-Header"] = userAgent; //自定义添加任意头
            headers["User-Agent"] = userAgent;
            //headers["Referer"] = "http://www.baidu.com";//无效
            request.Headers = headers;
            if (sourceFromData.WangPanType == 1)//城通网盘
            {
                if (!string.IsNullOrWhiteSpace(sourceFromData.actualAccessDownloadPageUrl)&&request.Url.Contains(sourceFromData.actualAccessDownloadPageUrl))//说明要访问具体下载页面了，需要设置来源
                {
                    Console.WriteLine("进入具体下载页面："+ sourceFromData.actualAccessDownloadPageUrl + "，设置的reffer为："+ sourceFromData.FromSource);
                    request.SetReferrer(sourceFromData.FromSource, ReferrerPolicy.Always);
                }
            }
            
            return CefReturnValue.Continue;
        }
    }
}

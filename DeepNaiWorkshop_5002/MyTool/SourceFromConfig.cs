using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_5002.MyTool
{
    /// <summary>
    /// 由于城通网盘中记录了网盘链接上一级来路，顾我在新浪微博中写了一篇博客，并且在博客中附加了一些网盘地址，这样仿造的，后面这个源要多加，来伪造
    /// </summary>
    class SourceFromConfig
    {
        public static int ROUTING_SINA_BLOG_TYPE = 1;//伪装路由是新浪微博的
        private static List<SourceFromData> sourceFromList;
        static SourceFromConfig()
        {
            sourceFromList = new List<SourceFromData>();

            sourceFromList.Add(new SourceFromData("http://blog.sina.com.cn/s/blog_1472ab5210102x8fe.html", 1,1,""));//新浪博客地址 不能用小说
        }


        public static SourceFromData randomSourceFromData()
        {
            Random r = new Random();
            int i = r.Next(sourceFromList.Count);
            return sourceFromList[i];
        }
    }
}

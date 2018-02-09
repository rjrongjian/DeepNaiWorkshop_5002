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
        private static List<SourceFromData> sourceFromList;
        static SourceFromConfig()
        {
            sourceFromList = new List<SourceFromData>();

            sourceFromList.Add(new SourceFromData("http://blog.sina.com.cn/u/5488948513", 1));//新浪博客地址
        }


        public static SourceFromData randomSourceFromData()
        {
            Random r = new Random();
            int i = r.Next(sourceFromList.Count);
            return sourceFromList[i];
        }
    }
}

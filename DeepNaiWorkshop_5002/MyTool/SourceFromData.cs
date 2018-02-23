using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_5002.MyTool
{
    class SourceFromData
    {
        private String fromSource;//含有城通url的
        private int type;//来源网站类别，不同的类别，识别出里面的链接的方式不同 1 新浪博客
        private int wangPanType;//1 城通网盘
        public String actualAccessDownloadPageUrl;//实际访问的下载链接的url

        public string FromSource { get => fromSource; set => fromSource = value; }
        public int Type { get => type; set => type = value; }
        public int WangPanType { get => wangPanType; set => wangPanType = value; }
       

        public SourceFromData()
        {

        }

        public SourceFromData(String fromSource, int type,int wangPanType,String actualAccessDownloadPageUrl)
        {
            this.fromSource = fromSource;
            this.type = type;
            this.wangPanType = wangPanType;
            this.actualAccessDownloadPageUrl = actualAccessDownloadPageUrl;
        }
    }
}

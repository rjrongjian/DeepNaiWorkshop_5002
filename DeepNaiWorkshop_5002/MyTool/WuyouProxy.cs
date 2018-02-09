using HttpCodeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefTest.MyTool
{
    /// <summary>
    /// 无忧代理设置
    /// </summary>
    class WuyouProxy
    {
        public static String getProxyIpAndPort()
        {
            Wininet wininet = new Wininet();
            String result = wininet.GetData("http://api.ip.data5u.com/dynamic/get.html?order=61fbc26a3a7cd3f62bf8be2e7a60c83a&ttl=1&sep=3");
            //返回的格式 222.185.146.193:39441,16361
            if (result.Contains(","))
            {
                return result.Split(',')[0];
            }
            else
            {
                return result;
            }

        }
    }
}

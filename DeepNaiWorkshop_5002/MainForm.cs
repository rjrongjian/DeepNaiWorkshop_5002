using CefSharp;
using CefSharp.WinForms;
using CefTest;
using CefTest.MyTool;
using DeepNaiWorkshop_5002.MyTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepNaiWorkshop_5002
{
    public partial class MainForm : Form
    {
        private ChromiumWebBrowser browser;
        //实例化一个timer  
        private System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        private SourceFromData sourceFromData;
        private String userAgent;
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            oneThreadChengTongService();
        }

        /// <summary>
        /// 单线程业务逻辑
        /// </summary>
        private void oneThreadChengTongService()
        {
            //TODO 如果不能动态更改userAgent，那就多启动几个客户端程序，谁抢到了下载线程谁就执行，或者开通城通的多线程下载


            //获取要伪装的上级页面地址
            sourceFromData = SourceFromConfig.randomSourceFromData();
            //随机的userAgent信息
            userAgent = UserAgent.randomUserAgent();
            if (!Cef.IsInitialized)
            {
                CefSettings settings = new CefSettings() {
                    CachePath = Directory.GetCurrentDirectory() + @"\Cache",
                };
                settings.UserAgent = userAgent;
                settings.PersistSessionCookies = true;//支持cookie
                settings.CefCommandLineArgs.Add("ppapi-flash-path", @"Plugins\pepflashplayer.dll");//cef 支持flash http://blog.csdn.net/xxhongdev/article/details/77195339
                settings.AcceptLanguageList = "zh-CN,zh;q=0.8";

                //settings.CefCommandLineArgs.Add("proxy-server", ipAndPort);
                //settings.CefCommandLineArgs.Add("no-proxy-server", "1")
                //settings.UserAgent = "Hello!";

                Cef.Initialize(settings);
            }

            //browser = new ChromiumWebBrowser("https://www.baidu.com/s?wd=%E6%88%91%E7%9A%84ip&rsv_spt=1&rsv_iqid=0x914838db0001e715&issp=1&f=8&rsv_bp=0&rsv_idx=2&ie=utf-8&tn=94789287_hao_pg&rsv_enter=1&rsv_sug3=5&rsv_sug1=3&rsv_sug7=100&rsv_t=2fec3e53%2FvqkD9VS1c4ogr84NRknqB%2FGqrZrxz5Cxm5EsGDivYD6hdnRTYE7%2BQZBJN4p7tl%2B");
            String testIp = "https://www.baidu.com/s?wd=%E6%88%91%E7%9A%84ip&rsv_spt=1&rsv_iqid=0x914838db0001e715&issp=1&f=8&rsv_bp=0&rsv_idx=2&ie=utf-8&tn=94789287_hao_pg&rsv_enter=1&rsv_sug3=5&rsv_sug1=3&rsv_sug7=100&rsv_t=2fec3e53%2FvqkD9VS1c4ogr84NRknqB%2FGqrZrxz5Cxm5EsGDivYD6hdnRTYE7%2BQZBJN4p7tl%2B";
            browser = new ChromiumWebBrowser(sourceFromData.FromSource) {
            //browser = new ChromiumWebBrowser(testIp){
                BrowserSettings = new BrowserSettings()
                {
                    Plugins = CefState.Enabled
                },
                Dock = DockStyle.Fill
            };

            browser.FrameLoadEnd += browser_FrameLoadEnd;//网页加载完成
            browser.DownloadHandler = new MyDownLoadFile();//下载器配置
            browser.LifeSpanHandler = new MyLifeSpanHandler();//在同一窗口打开链接
            browser.RequestHandler = new MyRequestHandler(sourceFromData);//动态更改 header
            //TODO: 在官方demo里CefSharp.Example中->Handlers->RequestHandler中93行说明了如何让每次请求都更改user-agent,可以参考
            //browser.RequestHandler = new MyRequestHandler();//每次请求都更换User-Agent 注意IRequestHandler在CefSharp.IRequestHandler



            browser.Dock = DockStyle.Fill;

            //js调用c#
            //browser表示你的CefSharp对象使用它的RegisterJsObject来绑定你的.net类  
            browser.RegisterJsObject("sourceFromData", sourceFromData);
            

            //Console.WriteLine("browser是否初始化：" + browser);
            //由于在下载之头启用了代理ip，此处要禁止代理ip
            Cef.UIThreadTaskFactory.StartNew(delegate {
                //Console.WriteLine("获取的ip:" + ipAndPort);
                //Console.WriteLine("browser2是否初始化：" + browser);
                var rc = Cef.GetGlobalRequestContext();
                var v = new Dictionary<string,object>();
                v["mode"] = "fixed_servers";
                //v["server"] = WuyouProxy.getProxyIpAndPort();
                v["User-Agent"] = userAgent;
                string error;
                bool success = rc.SetPreference("proxy", v, out error);
                if (success)
                {

                }
            });

            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(browser);
            
        }

        /// <summary>
        /// 网页加载完成操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)//单次加载页面会调用此事件多次，需加这个判断
            {
                //直接转换成ChromiumWebBrowser browserFor 不能获取Address
                var browserFor = (ChromiumWebBrowser)sender;
                //Console.WriteLine("获取的地址："+ browserFor.Address);
                var result = await browser.GetSourceAsync();
                if (sourceFromData.FromSource.Contains(browserFor.Address))//说明进到了伪装的一级路由页面
                {
                    if (SourceFromConfig.ROUTING_SINA_BLOG_TYPE == sourceFromData.Type)
                    {

                        //设置ip
                        //进入下载页面之前将ip更改
                        await Cef.UIThreadTaskFactory.StartNew(delegate
                        {
                            var rc = Cef.GetGlobalRequestContext();
                            var v = new Dictionary<string,object>();
                            v["mode"] = "fixed_servers";
//测试屏蔽2018-02-23 21:19:00 代理ip
                            v["server"] = WuyouProxy.getProxyIpAndPort();
                            string error;
                            bool success = rc.SetPreference("proxy", v, out error);
                            if (success)
                            {

                            }
                        });
    

                        //Random rd = new Random();
                        //int randomSleepSecond = rd.Next(1,6);//1-5秒
                        Console.WriteLine("进入的伪装路由连接：" + sourceFromData.FromSource + "，且类型为新浪博客");
                        //Console.WriteLine("从伪装页面进入下载页面的随机睡眠时间："+randomSleepSecond+"s");
                        //Thread.Sleep(randomSleepSecond*1000);
                        
                        StringBuilder scriptCode = new StringBuilder();
                        scriptCode.Append("var aElements = new Array();");
                        scriptCode.Append("var x = document.getElementById('sina_keyword_ad_area2').getElementsByTagName(\"div\");");
                        scriptCode.Append("for(var i = 0;i<x.length;i++){");
                        scriptCode.Append("    var sonDiv = x[i];");
                        scriptCode.Append("    var aInSonDiv = sonDiv.getElementsByTagName(\"a\");");
                        scriptCode.Append("    if(aInSonDiv&&aInSonDiv.length>0){");
                        scriptCode.Append("        aElements[aElements.length] = aInSonDiv[0];");
                        scriptCode.Append("    }else{");
                        scriptCode.Append("        console.log(\"不包含子标签\");");
                        scriptCode.Append("    }");
                        scriptCode.Append("}");
                        scriptCode.Append("var randomA = aElements[Math.floor(Math.random()*aElements.length)];");
                        scriptCode.Append("sourceFromData.actualAccessDownloadPageUrl=randomA.getAttribute(\"href\");");//js调用c#
                        scriptCode.Append("randomA.click();");
                        browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(scriptCode.ToString());

                        //Console.WriteLine("实际点击的下载页面的url:"+ sourceFromData.actualAccessDownloadPageUrl); //估计是异步的，这里还没获取这个值呢，但在RequestHandler中已经获取此值
                        //Console.WriteLine("测试。。。。。。");
                        //browser.Load("https://www.baidu.com/s?wd=%E6%88%91%E7%9A%84ip&rsv_spt=1&rsv_iqid=0x914838db0001e715&issp=1&f=8&rsv_bp=0&rsv_idx=2&ie=utf-8&tn=94789287_hao_pg&rsv_enter=1&rsv_sug3=5&rsv_sug1=3&rsv_sug7=100&rsv_t=2fec3e53%2FvqkD9VS1c4ogr84NRknqB%2FGqrZrxz5Cxm5EsGDivYD6hdnRTYE7%2BQZBJN4p7tl%2B");
                      

                        //Console.WriteLine("测试结束-------------");
                        //Console.WriteLine("执行到这"+ scriptCode.ToString());
                    }
                    else
                    {
                        throw new Exception("不能识别的伪装路由类型：" + sourceFromData.Type);
                    }
                }
                else//说明已经到了具体下载页面了
                {

                    //每次请求更换ip 下载之头无需切换ip
                    /*
                    await Cef.UIThreadTaskFactory.StartNew(delegate
                    {
                        //Console.WriteLine("获取的ip:" + ipAndPort);
                        //Console.WriteLine("browser2是否初始化：" + browser);
                        var rc = Cef.GetGlobalRequestContext();
                        var v = new Dictionary<string,
                            object>();
                        v["mode"] = "fixed_servers";
//                        v["server"] = WuyouProxy.getProxyIpAndPort();
                        v["User-Agent"] = userAgent;
//                        v["Referer"] = sourceFromData.FromSource;//设置来源信息
                        string error;
                        bool success = rc.SetPreference("proxy", v, out error);
                        if (success)
                        {

                        }
                    });
                    */
                    //Console.WriteLine("测试load");
                    //browser.Load("https://www.baidu.com/s?wd=%E6%88%91%E7%9A%84ip&rsv_spt=1&rsv_iqid=0x914838db0001e715&issp=1&f=8&rsv_bp=0&rsv_idx=2&ie=utf-8&tn=94789287_hao_pg&rsv_enter=1&rsv_sug3=5&rsv_sug1=3&rsv_sug7=100&rsv_t=2fec3e53%2FvqkD9VS1c4ogr84NRknqB%2FGqrZrxz5Cxm5EsGDivYD6hdnRTYE7%2BQZBJN4p7tl%2B");
                    Random rd = new Random();
                    int randomSleepSecond = rd.Next(1000, 6000);//1000-5999毫秒
                    Console.WriteLine("点击下载之头，随机睡眠：" + randomSleepSecond + "s");
                    Thread.Sleep(randomSleepSecond);

                    Console.WriteLine("城通网盘下载页面，开始下载...");
                    browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('free_down_link').click();");
                }




                // Console.WriteLine("页面加载完成：" + result);
                Console.WriteLine("sender:" + sender);
                //
            }
            else
            {
                Console.WriteLine("非主窗口加载完毕，不执行任何逻辑！");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //给timer挂起事件  
            myTimer.Tick += new EventHandler(Callback);
            //使timer可用  
            myTimer.Enabled = true;
            //获取时间间隔
            String time = this.textBox1.Text;
            Console.WriteLine("设置的时间间隔："+ (int.Parse(time) * 1000)+"毫秒");
            //设置时间间隔，以毫秒为单位  
            myTimer.Interval = int.Parse(time) * 1000;
        }

        //回调函数  
        private void Callback(object sender, EventArgs e)
        {
            //获取系统时间 20:16:16  
            //textBox1.Text = DateTime.Now.ToLongTimeString().ToString();
            this.label2.Text = DateTime.Now.ToLongTimeString().ToString();
            //执行业务逻辑
            oneThreadChengTongService();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //计时开始  
            myTimer.Stop();
        }
    }
}

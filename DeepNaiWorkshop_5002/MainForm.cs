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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepNaiWorkshop_5002
{
    public partial class MainForm : Form
    {
        private ChromiumWebBrowser browser;
        private SourceFromData sourceFromData;
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
            String userAgent = UserAgent.randomUserAgent();
            if (!Cef.IsInitialized)
            {
                CefSettings settings = new CefSettings();
                settings.UserAgent = userAgent;

                //settings.CefCommandLineArgs.Add("proxy-server", ipAndPort);
                //settings.UserAgent = "Hello!";

                Cef.Initialize(settings);
            }

            //browser = new ChromiumWebBrowser("https://www.baidu.com/s?wd=%E6%88%91%E7%9A%84ip&rsv_spt=1&rsv_iqid=0x914838db0001e715&issp=1&f=8&rsv_bp=0&rsv_idx=2&ie=utf-8&tn=94789287_hao_pg&rsv_enter=1&rsv_sug3=5&rsv_sug1=3&rsv_sug7=100&rsv_t=2fec3e53%2FvqkD9VS1c4ogr84NRknqB%2FGqrZrxz5Cxm5EsGDivYD6hdnRTYE7%2BQZBJN4p7tl%2B");
            browser = new ChromiumWebBrowser(sourceFromData.FromSource);
            browser.FrameLoadEnd += browser_FrameLoadEnd;//网页加载完成
            browser.DownloadHandler = new MyDownLoadFile();//下载器配置
            browser.Dock = DockStyle.Fill;
            //Console.WriteLine("browser是否初始化：" + browser);
            Cef.UIThreadTaskFactory.StartNew(delegate {
                String ipAndPort = WuyouProxy.getProxyIpAndPort();
                //Console.WriteLine("获取的ip:" + ipAndPort);
                Console.WriteLine("browser2是否初始化：" + browser);
                var rc = Cef.GetGlobalRequestContext();
                var v = new Dictionary<string,
                    object>();
                v["mode"] = "fixed_servers";
                v["server"] = ipAndPort;
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
            //直接转换成ChromiumWebBrowser browserFor 不能获取Address
            var browserFor = (ChromiumWebBrowser)sender;
            //Console.WriteLine("获取的地址："+ browserFor.Address);
            if (sourceFromData.FromSource.Contains(browserFor.Address))//说明进到了伪装的一级路由页面
            {

            }
            
            

            var result = await browser.GetSourceAsync();
            // Console.WriteLine("页面加载完成：" + result);
            Console.WriteLine("sender:"+ sender);
            //browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('free_down_link').click();");
        }
    }
}

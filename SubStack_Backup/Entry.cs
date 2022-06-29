using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace SubStack_Backup
{
    class Entry
    {
        static async Task Main(string[] args)
        {
            #region Access Config
            string
                _account = string.Empty,
                _pwd = string.Empty
                ;
            string rootPath = Path.GetDirectoryName(GetDebugFilePath());
            string relativePath = @"static\config.json";
            string fullPath = Path.Combine(rootPath, relativePath);
            if (File.Exists(fullPath))
            {
                var _config = JObject.Parse(File.ReadAllText(fullPath));
                _account = (string)_config["SubStack"]["_ACCOUNT"];
                _pwd = (string)_config["SubStack"]["_PWD"];
            }
            else
            {
                // 配置檔不存在，謝謝再聯絡
            }
            #endregion

            #region 常數
            const string BACKUP_URL = "https://substack.com/";
            const string ARTICLE_URL = "https://blocktrend.substack.com/archive?sort=new";
            const string DEST_PATH = "C:\\Users\\waynejin\\Downloads\\區塊勢舊文章\\TEMP";
            const int SLEEP = 2000;
            #endregion

            #region Selenium env
            DriverManager manager = new DriverManager();
            manager.SetUpDriver(new ChromeConfig());

            ChromeOptions pdfOption = new ChromeOptions();
            pdfOption.PageLoadStrategy = PageLoadStrategy.Normal; // 整個page load進來再開始處理其他事情
            pdfOption.AddUserProfilePreference("printing.print_preview_sticky_settings.appState", "{\"recentDestinations\": [{\"id\": \"Save as PDF\", \"origin\": \"local\", \"account\": \"\" }],\"version\":2,\"isGcpPromoDismissed\":false,\"selectedDestinationId\":\"Save as PDF\"}");
            pdfOption.AddUserProfilePreference("savefile.default_directory", DEST_PATH);
            pdfOption.AddUserProfilePreference("useAutomationExtension", false);
            pdfOption.AddExcludedArgument("enable-automation");
            pdfOption.AddArgument("--kiosk-printing"); // 自動按儲存

            ChromeDriver driver = new ChromeDriver(pdfOption);
            #endregion

            #region 公共變數
            IWebElement
                    signInBtn = null,
                    signInByPwdBtn = null,
                    signInEmailInput = null,
                    signInPwdInput = null
                    ;
            #endregion

            #region 登入substack
            try
            {
                driver.Navigate().GoToUrl(BACKUP_URL);
                signInBtn = driver.FindElement(By.ClassName("sign-in-link"));
            }
            catch
            {
                driver.Quit();
            }

            signInBtn.Click(); // 進入登入畫面

            if (driver.FindElements(By.Name("password")).Count == 0) // 切換成輸入email、pwd的登入方式
            {
                signInByPwdBtn = driver.FindElement(By.ClassName("substack-login__login-option"));
                signInByPwdBtn.Click();
            }

            signInEmailInput = driver.FindElement(By.Name("email")); // 輸入帳密
            signInEmailInput.SendKeys(_account);
            signInPwdInput = driver.FindElement(By.Name("password"));
            signInPwdInput.SendKeys(_pwd);

            signInBtn = driver.FindElement(By.CssSelector("[type='submit']"));
            signInBtn.Click();
            #endregion

            #region 進入文章清單
            try
            {
                driver.Navigate().GoToUrl(ARTICLE_URL);
                Thread.Sleep(SLEEP);
                driver.Navigate().Refresh(); // 如果不refresh，會沒有登入狀態？？？ 看cookies，確實有登入id，但載下來的pdf是非付費的內容
            }
            catch
            {
                driver.Quit();
            }

            // 迴圈下拉到底
            /* 測試不需要全部文章
            System.Int64 scrollHeight = 0;
             while (scrollHeight < (System.Int64)driver.ExecuteScript("return document.body.scrollHeight"))
            {
                scrollHeight = (System.Int64)driver.ExecuteScript("return document.body.scrollHeight");
                driver.ExecuteScript($"window.scrollTo(0, {scrollHeight})");
                Thread.Sleep(4000);
            }
            */
            #endregion

            #region 下載所有文章
            var postList = driver.FindElements(By.ClassName("post-preview"));
            int count = 1;
            foreach (var post in postList)
            {
                string postUrl = post.FindElement(By.TagName("a")).GetAttribute("href");
                try
                {
                    await downloadFileAsync(driver, postUrl, DEST_PATH);
                }
                catch (Exception err)
                {
                    Console.WriteLine($"錯誤發生:{err.Message}");
                    Console.WriteLine($"錯誤種類:{err.GetType()}");
                }
                count++;
            }
            #endregion

            Console.ReadLine();
        }

        #region debug階段，讀取config的路徑
        private static string GetDebugFilePath([CallerFilePath] string path = null)
        {
            return path;
        }
        #endregion

        #region 下載
        private static async Task downloadFileAsync(ChromeDriver driver, string url, string destPath)
        {
            // 宣告
            IWebElement podcastPlayer = null;
            string title = string.Empty;

            // 開新分頁
            driver.ExecuteScript($"window.open('{url}')");
            Thread.Sleep(1000);
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            title = driver.FindElement(By.CssSelector("h1.tw-mt-0")).Text;
            try
            {
                podcastPlayer = driver.FindElement(By.TagName("audio"));
            }
            catch
            {

            }

            // 新增TEMP
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // 下載podcast
            if (podcastPlayer != null)
            {
                await Task.Run(() =>
                {
                    downloadPodcast(destPath, title, podcastPlayer.GetAttribute("src"));
                });
            }

            // 下載pdf
            await Task.Run(() =>
            {
                driver.ExecuteScript("window.print();");
                driver.ExecuteScript("window.opener = null; window.open(' ', '_self'); window.close();");
                driver.SwitchTo().Window(driver.WindowHandles[0]);
            }).ContinueWith((Task t) =>
            {
                // 資料夾改名
                moveTempFile(destPath, title);
            });
        }
        #endregion

        #region 下載podcast
        private static void downloadPodcast(string destPath, string fileName, string podcastUrl)
        {
            fileName = $"{fileName}.mp3";
            Uri src = new Uri(podcastUrl);
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(src, Path.Combine(destPath, fileName));
            }
        }
        #endregion

        #region 資料移動

        private static void moveTempFile(string srcPath, string folderName)
        {
            string[] destPathSplit = srcPath.Split("\\");
            destPathSplit[destPathSplit.Length - 1] = folderName;
            string destPath = string.Join("\\", destPathSplit);

            threadHint("檔案移動");
            Directory.Move(srcPath, destPath);
        }

        #endregion

        #region 顯示Thread
        private static void threadHint(string message)
        {
            Console.WriteLine($"{message}:執行緒{Thread.CurrentThread.ManagedThreadId}");
        }
        #endregion

        #region Log
        private static void Log()
        {
        }
        #endregion
    }
}


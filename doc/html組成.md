< !--each post-- >
<div class= "post-preview portable-archive-post has-image">
   <div class= "post-preview-content">
      <a href = "https://blocktrend.substack.com/p/448"></a>
      <div class= "post-meta post-meta-actions-web custom post-preview-meta">
        <div title = "2022-05-18T23:30:32.349Z" class ="post-meta-item post-date"><time datetime = "2022-05-18T23:30:32.349Z"> May 19 </time></div>
      </div>
   </div>
</div>

## 分類
- 一般文章 => 存成pdf
- 純粹podcast => 下載音檔
- 文章 + podcast => 資料夾存pdf、音檔
### 存檔名稱
{序號}-{標題}-{撰寫時間}
### 爬蟲特徵點


## 學習
<!-- js 關tab -->
window.opener = null;
window.open(' ','_self');
window.close();   

<!-- js 自動捲動 -->
window.body.scrollHeight 取得最大高度
window.scrollTo(0, MAXHEIGHT) 觸發滾動api

<!-- selenium的 webdriver instance對應一個tab，如果不切換tab，單純增加一個tab，是不能對新的tab操作的 -->
driver.ExecuteScript("window.open('https://www.google.com')");
Thread.Sleep(1000);
driver.SwitchTo().Window(driver.WindowHandles[1]);
driver.ExecuteScript("window.opener = null;");
driver.ExecuteScript("window.open(' ', '_self');");
driver.ExecuteScript("window.close();");

<!-- 自動下載pdf -->
<!-- selenium3寫法 -->
ChromeOptions browserOption = new ChromeOptions();
browserOption.PageLoadStrategy = PageLoadStrategy.Normal; // 整個page load進來再開始處理其他事情
browserOption.AddUserProfilePreference("printing.print_preview_sticky_settings.appState", "{\"recentDestinations\": [{\"id\": \"Save as PDF\", \"origin\": \"local\", \"account\": \"\" }],\"version\":2,\"isGcpPromoDismissed\":false,\"selectedDestinationId\":\"Save as PDF\"}");
browserOption.AddUserProfilePreference("savefile.default_directory", FILE_PATH);
browserOption.AddUserProfilePreference("useAutomationExtension", false);
browserOption.AddExcludedArgument("enable-automation");
browserOption.AddArgument("--kiosk-printing"); // 自動按儲存

ChromeDriver driver = new ChromeDriver(browserOption);
driver.ExecuteScript("window.print()");
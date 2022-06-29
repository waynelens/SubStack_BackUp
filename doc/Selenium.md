# Selenium 規劃

## Source 
- [.Net Api](https://www.selenium.dev/selenium/docs/api/dotnet/)
- [Official Docs](https://www.selenium.dev/documentation/webdriver/capabilities/shared/)

## 建立C#環境
- Nuget下載： Selenium.WebDriver => WebDriver install_library
- Nuget下載： Selenium.WebDriver.ChromeDriver => For Chrome
- Nuget下載: WebDriverManager => 因爲瀏覽器的版本常變動，官方提供三種方法保持driver版本最新 -- WebDriverManager是其中一個。
``` C sharp
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

// 一行配置完成
new DriverManager().SetUpDriver(new ChromeConfig());
```
## Selenium初始化
```C sharp
IWebDriver _webDriver = new ChromeDriver();

// 一定要結束，不然會有chrome process一直佔著。
_webDriver.Quit();
```

## 設置Load頁面的策略
``` C sharp
var chromeOtions = new ChromeOptions();
chromeOtions.PageLoadStrategy = PageLoadStrategy.Normal;
```
- [其他策略](https://www.selenium.dev/documentation/webdriver/capabilities/shared/)



## Step by Step
- 讀取帳號配置檔案
- 登入Substack
- 進入文章
- [pdf下載](https://terrence0303-64740.medium.com/%E4%BD%BF%E7%94%A8selenium%E5%B0%87%E7%B6%B2%E9%A0%81%E5%88%97%E5%8D%B0%E6%88%90pdf-5e625e2e4131)
- podcast備份
- 檔案根據文章時間重新命名
- 回上一頁，繼續找下一個文章

### 問題
- [Debug階段，路徑存取問題](https://stackoverflow.com/questions/47841441/how-do-i-get-the-path-to-the-current-c-sharp-source-code-file)
- 進入文章下載可以分成腳本進入、打api兩種實現方式。
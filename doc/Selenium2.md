# Selenium 規劃

## Source 
- [.Net Api](https://www.selenium.dev/selenium/docs/api/dotnet/)
- [Official Docs](https://www.selenium.dev/documentation/webdriver/capabilities/shared/)

## 建立C#環境
- Nuget下載： Selenium.WebDriver => WebDriver install_library
- Nuget下載： Selenium.WebDriver.ChromeDriver => For Chrome
- Nuget下載: WebDriverManager => 因爲瀏覽器的版本常變動，官方提供三種方法保持driver版本最新 -- WebDriverManager是其中一個。
``` C#
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

// 一行配置完成
new DriverManager().SetUpDriver(new ChromeConfig());
```
## Selenium初始化
```C#
IWebDriver _webDriver = new ChromeDriver();

// 一定要結束，不然會有chrome process一直佔著。
_webDriver.Quit();
```

## Step by Step
0. 登入Substack
1. 進入文章
2. pdf下載 
- [中文python](https://terrence0303-64740.medium.com/%E4%BD%BF%E7%94%A8selenium%E5%B0%87%E7%B6%B2%E9%A0%81%E5%88%97%E5%8D%B0%E6%88%90pdf-5e625e2e4131)
- [stackoverflow](https://stackoverflow.com/questions/61798725/save-as-pdf-using-selenium-and-chrome)
3. 檔案根據文章時間重新命名
4. 回上一頁，繼續找下一個文章
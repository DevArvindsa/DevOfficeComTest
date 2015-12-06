﻿using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace TestFramework
{
    public static class Browser
    {
        static IWebDriver webDriver = new ChromeDriver(@"c:\libraries");

        public static void Initialize()
        {
            webDriver.Navigate().GoToUrl("http://dev.office.com");
        }
        
        public static void Goto(string url)
        {
            webDriver.Navigate().GoToUrl(url);
        }

        public static string Title
        {
            get { return webDriver.Title; }
        }

        public static void Close()
        {
            webDriver.Close();
        }

        public static ISearchContext Driver
        {
            get { return webDriver; }
        }
    }
}
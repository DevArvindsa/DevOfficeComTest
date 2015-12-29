﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.OfficeAddInPage
{
    public class CardBuild : BasePage
    {
        public void StartBuilding(Product product)
        {
            if (!Browser.Url.Contains("/getting-started/addins"))
            {
                Browser.Goto(Browser.BaseAddress + "/getting-started/addins#build");
            }

            switch (product)
            {
                case Product.Excel:
                case Product.Outlook:
                    {
                        var buildBtn = Browser.Driver.FindElement(By.Id("more-playground"));
                        buildBtn.Click();
                        break;
                    }
                case Product.PowerPoint:
                case Product.Word:
                    {
                        var buildBtn = Browser.Driver.FindElement(By.Id("build-downloadFromGithub"));
                        buildBtn.Click();
                        break;
                    }
            }

            // Need refactor: The waiting time is not stable
            Browser.Wait(TimeSpan.FromSeconds(10));

        }

        public bool IsShowingBuildPage(Product product)
        {
            bool canSwitchWindow = false;
            switch (product)
            {
                case Product.Excel:
                    {
                        canSwitchWindow = Browser.SwitchToWindow("index.html - Task Pane Add-in Sample - Excel - Napa");
                        break;
                    }
                case Product.Outlook:
                    {
                        canSwitchWindow = Browser.SwitchToWindow("index.html - Mail Read Sample - Napa");
                        break;
                    }
                case Product.PowerPoint:
                case Product.Word:
                    {
                        canSwitchWindow = Browser.SwitchToWindow("OfficeDev/Add-in-TaskPane-Sample");
                        break;
                    }
            }

            Browser.SwitchBack();
            return canSwitchWindow;

        }
    }
}

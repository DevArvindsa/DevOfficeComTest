﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework
{
    /// <summary>
    /// Static class for common functions of Graph site
    /// </summary>
    public static class GraphUtility
    {
        /// <summary>
        /// Clear all the cookies of the current page
        /// </summary>
        public static void ClearCookies()
        {
            GraphBrowser.webDriver.Manage().Cookies.DeleteAllCookies();
        }

        /// <summary>
        /// Verify if the toggle arrow is found on the page 
        /// </summary>
        /// <returns>Trye if yes, else no.</returns>
        public static bool IsToggleArrowDisplayed()
        {
            return GraphBrowser.FindElement(By.Id("toggleLeftPanelContainer")).Displayed;
        }

        /// <summary>
        /// Verify if the menu-content is found on the page
        /// </summary>
        /// <returns>Trye if yes, else no.</returns>
        public static bool IsMenuContentDisplayed()
        {
            return GraphBrowser.FindElement(By.CssSelector("#menu-content")).Displayed;
        }

        /// <summary>
        /// Execute the menu display toggle
        /// </summary>
        public static void ToggleMenu()
        {
            var element = GraphBrowser.FindElement(By.Id("toggleLeftPanelContainer"));
            GraphBrowser.Click(element);
            GraphBrowser.Wait(TimeSpan.FromSeconds(2));
        }

        /// <summary>
        /// Click the branding image on the page
        /// </summary>
        public static void ClickBranding()
        {
            var element = GraphBrowser.FindElement(By.CssSelector("#branding>a"));
            GraphBrowser.Click(element);
        }

        /// <summary>
        /// Get the document title in the current doc page
        /// </summary>
        /// <returns>The title of document</returns>
        public static string GetDocTitle()
        {
            string docTitle;
            var title = GraphBrowser.FindElementInFrame("docframe", By.TagName("h1"), out docTitle);
            return docTitle;
        }

        /// <summary>
        /// Get the banner image url of MS Graph site
        /// </summary>
        /// <returns>The url of the banner image</returns>
        public static string GetGraphBannerImageUrl()
        {
            var element = GraphBrowser.FindElement(By.Id("banner-image"));
            if (element == null)
            {
                element = GraphBrowser.FindElement(By.CssSelector("div#layout-featured>div>article>div>div>div>div"));
            }
            string styleString = element.GetAttribute("style");
            string[] styles = styleString.Split(';');

            string url = string.Empty;
            foreach (string style in styles)
            {
                if (style.Contains("background-image"))
                {
                    int startIndex = style.IndexOf("/");
                    //2 is the length of ")
                    url = style.Substring(startIndex).Substring(0, style.Substring(startIndex).Length - 2);
                    break;
                }
            }
            return url;
        }

        /// <summary>
        /// Find an link, a button role span or a button according to the specific text and click it
        /// </summary>
        /// <param name="text">The text of the element</param>
        public static void Click(string text)
        {
            var element = GraphBrowser.FindElement(By.LinkText(text));
            //a link
            if (element != null && element.Displayed)
            {
                GraphBrowser.Click(element);
            }
            else
            {
                bool isButton = false;
                IReadOnlyList<IWebElement> elements = GraphBrowser.webDriver.FindElements(By.TagName("button"));

                foreach (IWebElement elementToClick in elements)
                {
                    //a button
                    if (elementToClick.GetAttribute("innerHTML").Contains(text) && elementToClick.Displayed)
                    {
                        GraphBrowser.Click(elementToClick);
                        isButton = true;
                        break;
                    }
                }
                if (!isButton)
                {
                    elements = GraphBrowser.webDriver.FindElements(By.XPath("//*[@role='button']"));
                    foreach (IWebElement elementToClick in elements)
                    {
                        //a button role span
                        if (elementToClick.GetAttribute("innerHTML").Equals(text) && (elementToClick.Displayed))
                        {
                            GraphBrowser.Click(elementToClick);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Randomly find a TOC item, which has a sub content menu, at a specific layer.
        /// </summary>
        /// <param name="layerIndex">The layer index. Starts from 0.</param>
        /// <returns>A list including the names of path items</returns>
        public static List<string> FindTOCParentItems(int layerIndex)
        {
            List<string> tocPath = new List<string>();
            string xpath = @"//nav[@id='home-nav-blade']";

            for (int i = 0; i <= layerIndex; i++)
            {
                xpath += "/ul/li";
            }
            var elements = GraphBrowser.webDriver.FindElements(By.XPath(xpath + "/a[@data-target]"));
            int randomIndex = new Random().Next(elements.Count);
            var element = elements[randomIndex];
            string title = element.GetAttribute("innerHTML");
            if (element.GetAttribute("style").Contains("text-transform: uppercase"))
            {
                title = title.ToUpper();
            }

            var ancestorElements = element.FindElements(By.XPath("ancestor::li/a")); //parent relative to current element
            for (int j = 0; j < ancestorElements.Count - 1; j++)
            {
                string ancestorTitle = ancestorElements[j].GetAttribute("innerHTML");
                if (ancestorElements[j].GetAttribute("style").Contains("text-transform: uppercase"))
                {
                    ancestorTitle = ancestorTitle.ToUpper();
                }
                tocPath.Add(ancestorTitle);
            }

            tocPath.Add(title);
            return tocPath;
        }

        /// <summary>
        /// Verify whether a TOC item's related sub layer is shown 
        /// </summary>
        /// <param name="item">The TOC item</param>
        /// <returns>True if yes, else no.</returns>
        public static bool SubLayerDisplayed(string item)
        {
            string xpath = @"//nav[@id='home-nav-blade']";
            var element = GraphBrowser.FindElement(By.XPath(xpath));
            var menuItem = element.FindElement(By.LinkText(item));
            string subMenuId = menuItem.GetAttribute("data-target");
            var subMenu = element.FindElement(By.XPath("//ul[@id='" + subMenuId.Replace("#", string.Empty) + "']"));
            return subMenu.Displayed;
        }

        /// <summary>
        /// Get The layer count of TOC
        /// </summary>
        /// <returns>The layer count</returns>
        public static int GetTOCLayer()
        {
            string xpath = "//nav[@id='home-nav-blade']";
            var menuElement = GraphBrowser.FindElement(By.XPath(xpath));
            int layer = 0;
            try
            {
                do
                {
                    layer++;
                    xpath += "/ul/li";
                    var element = menuElement.FindElement(By.XPath(xpath + "/a"));
                } while (true);
            }
            catch (NoSuchElementException)
            {
            }
            return layer - 1;
        }

        /// <summary>
        /// Login on a sign-in page 
        /// </summary>
        /// <param name="userName">The userName to input</param>
        /// <param name="password">The password to input</param>
        public static void Login(string userName, string password)
        {
            var userIdElement = GraphBrowser.FindElement(By.XPath("//input[@id='cred_userid_inputtext']"));
            userIdElement.SendKeys(userName);
            var passwordElement = GraphBrowser.FindElement(By.XPath("//input[@id='cred_password_inputtext']"));
            passwordElement.SendKeys(password);
            Click("Sign in");
        }

        /// <summary>
        /// Verify whether the logged in user is correct
        /// </summary>
        /// <param name="expectedUserName">The expected logged in user</param>
        /// <returns>True if yes, else no.</returns>
        public static bool IsLoggedIn(string expectedUserName)
        {
            var element = GraphBrowser.FindElement(By.XPath("//a[@ng-show='userInfo.isAuthenticated']"));
            if (element.Displayed && element.Text.Equals(expectedUserName))
            {
            return true;
            }
            else
            {
            return false;
            }
        }
    }
}

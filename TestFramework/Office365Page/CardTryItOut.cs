﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace TestFramework.Office365Page
{
    public class CardTryItOut : BasePage
    {
        public void ChooseService(ServiceToTry serviceToTry)
        {
            if (!Browser.Url.Contains("/getting-started/office365apis"))
            {
                Browser.Goto(Browser.BaseAddress + "/getting-started/office365apis#try-it-out");
            }

            int serviceIndex = (int)serviceToTry;
            var service = Browser.Driver.FindElement(By.Id("serviceOption"+serviceIndex));
            Browser.Click(service);
        }
		
        public void ChooseServiceValue(ServiceToTry service, object value)
        {
            string serviceValue = null;
            switch (service)
            {
                case ServiceToTry.GetMessages:
                    {
                        switch ((GetMessagesValue)value)
                        {
                            case GetMessagesValue.Inbox:
                            case GetMessagesValue.Drafts:
                            case GetMessagesValue.DeletedItems:
                            case GetMessagesValue.SentItems:
                                serviceValue = value.ToString();
                                break;
                            default:
                                serviceValue = null;
                                break;
                        }
                        break;
                    }
                case ServiceToTry.GetFiles:
                    {
                        switch ((GetFilesValue)value)
                        {
                            case GetFilesValue.drive_root_children:
                                serviceValue = "drive/root/children";
                                break;
                            case GetFilesValue.me_drive:
                                serviceValue = "me/drive";
                                break;
                            default:
                                serviceValue = null;
                                break;
                        }
                        break;
                    }
                case ServiceToTry.GetUsers:
                    {
                        switch ((GetUsersValue)value)
                        {
                            case GetUsersValue.me:
                                serviceValue = "me";
                                break;
                            case GetUsersValue.me_manager:
                                serviceValue = "me?&select=skills";
                                break;
                            case GetUsersValue.me_select_skills:
                                serviceValue = "me/manager";
                                break;
                            case GetUsersValue.myOrganization_users:
                                serviceValue = "myOrganization/users";
                                break;
                            default:
                                serviceValue = null;
                                break;

                        }
                        break;
                    }
                case ServiceToTry.GetGroups:
                    {
                        switch ((GetGroupValue)value)
                        {
                            case GetGroupValue.me_memberOf:
                                serviceValue = "me/memberOf";
                                break;
                            case GetGroupValue.members:
                                serviceValue = "groups/41525360-8eca-49ce-bcee-b205cd0aa747/members";
                                break;
                            case GetGroupValue.drive_root_children:
                                serviceValue = "groups/41525360-8eca-49ce-bcee-b205cd0aa747/drive/root/children";
                                break;
                            case GetGroupValue.conversations:
                                serviceValue = " groups/41525360-8eca-49ce-bcee-b205cd0aa747/conversations";
                                break;
                            default:
                                serviceValue = null;
                                break;
                        }
                        break;
                    }
                case ServiceToTry.GetContacts:
                case ServiceToTry.GetEvents:
                default:
                    serviceValue = null;
                    break;
            }
            Browser.SelectElement(Browser.Driver.FindElement(By.Id("valueSelection"))).SelectByText(serviceValue);
            bool isValue = Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains(serviceValue);
        }

        public void ClickTry()
        {
            var tryBtn = Browser.Driver.FindElement(By.Id("invokeurlBtn"));
            Browser.Click(tryBtn);

            Browser.Wait(TimeSpan.FromSeconds(3));
           // var wait = new WebDriverWait(Browser.Driver as IWebDriver, TimeSpan.FromSeconds(5));
            //wait.Until(d => d.FindElement(By.Id("response-container")));
            //WebDriverWait wait = new WebDriverWait((Browser.Driver as IWebDriver), TimeSpan.FromSeconds(10));
            //IWebElement responseContainer = wait.Until(d =>
            //{
            //    return d.FindElement(By.Id("response-container"));
            //});

            try
            {
                var action = new Actions(Browser.Driver as IWebDriver);
                var responseContainer = Browser.Driver.FindElement(By.Id("response-container"));
                action.MoveToElement(responseContainer);
                action.Perform();
            }
            catch (Exception)
            {
                { }
                throw;
            }
        }

        public bool CanGetResponse(ServiceToTry serviceToTry)
        {
            var responseBody = Browser.Driver.FindElement(By.Id("responseBody"));
            int serviceIndex = (int)serviceToTry;
            switch (serviceIndex)
            {
                // To do
                case (0):
                case (1):
                case (2):
                case (3):
                    return true;
                case (4):
                    return responseBody.Text.ToLower().Contains(@"https://graph.microsoft.com/v1.0/$metadata#users/$entity");
                case (5):
                    return true;

                default:
                    return false;
            }
        }
    }
}
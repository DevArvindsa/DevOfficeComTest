﻿using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFramework;

namespace Tests
{
    /// <summary>
    /// Site Test class for dev.office.com
    /// </summary>
    [TestClass]
    public class SiteTest
    {
        #region Additional test attributes
        [ClassCleanup]
        public static void ClassCleanup()
        {
            Browser.Close();
        }
        #endregion

        /// <summary>
        /// Verify whether robots.txt specifies the site is accessible.
        /// </summary>
        [TestMethod]
        public void BVT_S15_TC01_CanAccessSiteRobots()
        {
            if (Browser.BaseAddress.Contains("dev.office.com"))
            {
                Assert.Inconclusive("The test site should not be the production site");
            }
            string url = Browser.BaseAddress + "/robots.txt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream);
            string readResponse = readStream.ReadToEnd();
            bool disallowed = readResponse.Contains("Disallow:");
            Assert.IsTrue(disallowed, "The site should not be allowed to access");
        }

        // <summary>
        /// Verify whether robots.txt of production site specifies the site is accessible.
        /// </summary>
        [TestMethod]
        public void BVT_S15_TC02_CanAccessProductionSiteRobots()
        {
            string url = "http://dev.office.com/robots.txt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream);
            string readResponse = readStream.ReadToEnd();
            bool disallowed = readResponse.Contains("Disallow:");
            bool allowed = readResponse.Contains("Allow:");
            Assert.IsTrue(!disallowed && allowed, "The site should be allowed to access");
        }
    }
}

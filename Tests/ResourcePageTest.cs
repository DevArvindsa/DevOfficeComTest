﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFramework;

namespace Tests
{
    /// <summary>
    /// Test class for pages which are navigated to by the links under Resources
    /// </summary>
    [TestClass]
    public class ResourcePageTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Browser.Initialize();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Browser.Close();
        }

        /// <summary>
        /// Verify whether the filters in Training page can navigate to correct results
        /// </summary>
        [TestMethod]
        public void Can_Filter_Trainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            int filterCount = Pages.Navigation.GetFilterCount();

            for (int i = 0; i < filterCount; i++)
            {
                string filterName = Pages.Navigation.SelectFilter(i);

                List<Training> resultList = Pages.Navigation.GetFilterResults();
                Assert.AreNotEqual<int>(0,
                    resultList.Count,
                    "If select the filter {0}, there should be at least one training displayed",
                    filterName);
            }
        }

        /// <summary>
        /// Verify the search function in Resources->Training
        /// </summary>
        [TestMethod]
        public void Can_Search_CorrectTrainings()
        {
            Pages.Navigation.Select("Resources", "Training");
            int filterCount = Pages.Navigation.GetFilterCount();
            string searchString = "a";
            for (int i = 0; i < filterCount; i++)
            {
                string filterName = Pages.Navigation.SelectFilter(i);

                List<Training> resultList = Pages.Navigation.GetFilterResults(searchString);
                foreach (Training resultInfo in resultList)
                {
                    bool isNameMatched = resultInfo.Name.ToLower().Contains(searchString.ToLower());
                    bool isDescriptionMatched = resultInfo.Description.ToLower().Contains(searchString.ToLower());
                    Assert.IsTrue(isNameMatched || isDescriptionMatched,
                        "Under {0} filter, the training:\n {1}:{2}\n should contain the search text: {3}",
                        filterName,
                        resultInfo.Name,
                        resultInfo.Description,
                        searchString);
                }
            }
        }

        /// <summary>
        /// Verify whether the trainings can be sorted by view count correctly
        /// </summary>
        [TestMethod]
        public void Can_Sort_Trainings_By_View_Count()
        {
            Pages.Navigation.Select("Resources", "Training");
            int filterCount = Pages.Navigation.GetFilterCount();

            for (int i = 0; i < filterCount; i++)
            {
                string filterName = Pages.Navigation.SelectFilter(i);
                
                // Set the sort order as descendent
                Pages.Navigation.SetSortOrder(TrainingSortType.ViewCount, true);
                List<Training> resultList = Pages.Navigation.GetFilterResults();
                for (int j = 0; j < resultList.Count - 1; j++)
                {
                    Assert.IsTrue(resultList[j].ViewCount >= resultList[j + 1].ViewCount,
                        @"The view count of ""{0}"" should be larger than or equal to its sibling ""{1}""'s", 
                        resultList[j].Name,
                        resultList[j+1].Name);
                }

                // Set the sort order as ascendent
                Pages.Navigation.SetSortOrder(TrainingSortType.ViewCount, false);
                resultList = Pages.Navigation.GetFilterResults();
                for (int j = 0; j < resultList.Count - 1; j++)
                {
                    Assert.IsTrue(resultList[j].ViewCount <= resultList[j + 1].ViewCount,
                        @"The view count of ""{0}"" should be smaller than or equal to its sibling ""{1}""'s",
                        resultList[j].Name,
                        resultList[j + 1].Name);
                }
            }
        }
    }
}

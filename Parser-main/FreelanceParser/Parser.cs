using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using FreelanceParser.Model;
using HtmlAgilityPack;

namespace FreelanceParser
{
    static class Parser
    {
        public static string GetTitle(string page)
        {
            var html = new HtmlDocument();
            html.LoadHtml(page);
            
            var document = html.DocumentNode;
            var title = document.QuerySelector("title");
            return title.InnerText;
        }
        public static int GetNumberOfLastPage(string page)
        {
            var html = new HtmlDocument();
            html.LoadHtml(page);

            var document = html.DocumentNode;
            var pagBox = document.QuerySelectorAll(".pagination_box").Last();
            var divs = pagBox.QuerySelectorAll("div");
            var link = divs.Last().QuerySelector("a");
            string urlOnLastPage = link.GetAttributeValue("href", null);

            int numberOfLastPage = Convert.ToInt32(
                urlOnLastPage
                    .Split("=")
                    .Last()
            );
            return numberOfLastPage;
        }

        public static IList<UserLink> GetUsersUrl(string page)
        {
            IList<UserLink> usersUrl = new List<UserLink>();
            var html = new HtmlDocument();
            html.LoadHtml(page);

            var document = html.DocumentNode;
            var mainPane = document.QuerySelector("#tab_pane-main");
            var users = mainPane.QuerySelectorAll(".row");

            foreach (var user in users)
            {
                if (user.QuerySelector(".brief") != null)
                {
                    var link = user.QuerySelector("a");
                    string userUrl = link.GetAttributeValue("href", null);
                    usersUrl.Add(new UserLink() { Url = userUrl });
                }
            }
            return usersUrl;
        }

        public static string GetUserName(string page)
        {
            var html = new HtmlDocument();
            html.LoadHtml(page);

            var document = html.DocumentNode;
            var userBrief = document.QuerySelector("div.user_brief");
            string fullName = userBrief.QuerySelector(".name").InnerText;
            return fullName;
        }

        public static string GetUserLogin(string page)
        {
            var html = new HtmlDocument();
            html.LoadHtml(page);

            var document = html.DocumentNode;
            var userBrief = document.QuerySelector("div.user_brief");
            string nickname = userBrief.QuerySelector(".nickname").InnerText;
            return nickname;
        }

        public static int GetUserAge(string page)
        {
            var html = new HtmlDocument();
            html.LoadHtml(page);

            var document = html.DocumentNode;
            var userBrief = document.QuerySelector("div.user_brief");
            var brief = userBrief.QuerySelector(".brief");
            var divs = brief.QuerySelectorAll("div");
            string strAge = divs.ToArray()[1].InnerText;

            strAge = strAge.Split(" ")[0];
            int age = Convert.ToInt32(strAge);
            return age;
        }

        public static string GetUserCountry(string page)
        {
            var html = new HtmlDocument();
            html.LoadHtml(page);

            var document = html.DocumentNode;
            var userBrief = document.QuerySelector("div.user_brief");
            var brief = userBrief.QuerySelector(".brief");
            var divs = brief.QuerySelectorAll("div");
            string country = divs.ToArray()[1].InnerText;

            country = country.Split(",")[1];
            return country;
        }

        public static UserInfo GetUserInfo(string page)
        {
            UserInfo userInfo = new UserInfo
            {
                Name = GetUserName(page),
                Login = GetUserLogin(page),
                Age = GetUserAge(page),
                Country = GetUserCountry(page),
                // TODO: We need to set fields!
                Experience = "",
                AvgPrice = 0,
                UserPicUrl = ""
            };

            return userInfo;
        }

    }
}

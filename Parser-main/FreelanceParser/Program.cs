using System;
using System.Text;
using System.Threading.Tasks;
using FreelanceParser.DB;
using FreelanceParser.Model;

namespace FreelanceParser
{
    class Program
    {
        private const string WEBLANCER = "https://www.weblancer.net";
        private const string FREELANCERS_PAGE = WEBLANCER + "/freelancers/";

        static async Task Main(string[] args)
        {
            // For use windows-1251 Encoding and etc
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            DB.DB.Create();

            DownloadData dd = new DownloadData();
            dd.Auth(WEBLANCER);

            string page = dd.GetPage(FREELANCERS_PAGE).ToString();
            int numOfLastPage = 2;//Parser.GetNumberOfLastPage(page);
            
            foreach (var currentPage in dd.GetNextPage(FREELANCERS_PAGE, numOfLastPage))
            {
                string title = Parser.GetTitle(currentPage);
                Console.WriteLine(title);
                // парсим каждую страницу на инфу о пользователе (инфа общая)
                var usersUrl = Parser.GetUsersUrl(currentPage);
                // складываем инфу о каждом пользователе в БД (его url)
                using (UserLinkContext db = new UserLinkContext())
                {
                    db.UserLinks.AddRange(usersUrl);
                    db.SaveChanges();
                }
            }

            using (UserLinkContext userLinkContext = new UserLinkContext())
            using (UserInfoContext userInfoContext = new UserInfoContext())
            {
                foreach (var userLink in userLinkContext.UserLinks)
                {
                    // загружаем инфу о пользователе через его url
                    string userPage = dd.GetPage(WEBLANCER + userLink.Url);
                    
                    string title = Parser.GetTitle(userPage);
                    Console.WriteLine(title);
                    
                    // и парсим инфу о нем
                    UserInfo userInfo = Parser.GetUserInfo(userPage);
                    // userInfo.Url = userLink;
                    // складываем инфу о каждом пользователе в БД
                    userInfoContext.UserInfos.Add(userInfo);
                    userInfoContext.SaveChanges();
                }
            }
        }

    }
}

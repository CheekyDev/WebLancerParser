using System;
using Microsoft.Data.SqlClient;

namespace FreelanceParser.DB
{
    public static class DB
    {
        public const string CONNECTION_STRING =
            "Server=(localdb)\\mssqllocaldb;Database=WeblancerDB;Trusted_Connection=True;";
        
        private const string CONNECTION_MASTER_STRING =
            "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;";

        /// <summary>
        /// Инициализация БД. Создаем пустую таблицу, если у нас ее еще нет.
        /// </summary>
        public static void Create()
        {
            #region SQL commands

            var createBD = @"
                    IF NOT EXISTS (
                       SELECT name
                       FROM sys.databases
                       WHERE name = N'WeblancerDB'
                    )
                    CREATE DATABASE[WeblancerDB];";
            
            var createTableUserLink = @"
                    IF OBJECT_ID('dbo.UserLinks', 'U') IS NULL
                    CREATE TABLE dbo.UserLinks
                    (
                        Url     [NVARCHAR](50)  NOT NULL
                            constraint PK_UserLinks
                                primary key,
                    );";

            var createTableUserInfo = @"
                    IF OBJECT_ID('dbo.UserInfos', 'U') IS NULL 
                    CREATE TABLE dbo.UserInfos
                    (
                        Login      nvarchar(450) not null
                            constraint PK_UserInfos
                                primary key,
                        Name       nvarchar(max),
                        Age        int           not null,
                        AvgPrice   int           not null,
                        Country    nvarchar(max),
                        Experience nvarchar(max),
                        UserPicUrl nvarchar(max)
                    );";
            #endregion
            
            using (SqlConnection connection = new SqlConnection(CONNECTION_MASTER_STRING))
            {
                Console.WriteLine("Init DB");
                connection.Open();
                // create BD
                SqlCommand command = new SqlCommand(createBD, connection);
                command.ExecuteNonQuery();
            }
            
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                // create Table User Link
                SqlCommand command = new SqlCommand(createTableUserLink, connection);
                command.ExecuteNonQuery();
                // create Table User Info
                command.CommandText = createTableUserInfo;
                command.ExecuteNonQuery();
            }
        }
    }
}
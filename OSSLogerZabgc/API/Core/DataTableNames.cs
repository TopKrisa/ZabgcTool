using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSLogerZabgc.API.Core
{
    public class DataTableNames
    {
        public readonly string DataTableName;
           public DataTableNames(Tables tables)
        {
            switch (tables)
            {
                case Tables.News:
                    DataTableName = "news";
                    break;
                case Tables.Main:
                    DataTableName = "main";
                    break;
                case Tables.Pages:
                    DataTableName = "pages";
                    break;
                case Tables.Newspaper:
                    DataTableName = "newspaper";
                    break;
                case Tables.Internal:
                    DataTableName = "internal";
                    break;
                case Tables.Distant:
                    DataTableName = "distant";
                    break;
                case Tables.Department:
                    DataTableName = "department";
                    break;
                case Tables.Gallery:
                    DataTableName = "gallery";
                    break;
                case Tables.Comment:
                    DataTableName = "comment";
                    break;
                case Tables.Users:
                    DataTableName = "users";
                    break;
                case Tables.Notify:
                    DataTableName = "notify";
                    break;
                case Tables.Questions:
                    DataTableName = "questions";
                    break;
                case Tables.Metrics:
                    DataTableName = "metrics";
                        break;
                case Tables.Photos:
                    DataTableName = "photo";
                        break;
                case Tables.OSSLogger:
                    DataTableName = "logger";
                    break;
                default:
                    break;
            }
        }
        public enum Tables
        {
            News,
            Main,
            Pages,
            Newspaper,
            Internal,
            Distant,
            Department,
            Gallery,
            Comment,
            Users,
            Notify,
            Questions,
            Metrics,
            Photos,
            OSSLogger,
    }
    }
}

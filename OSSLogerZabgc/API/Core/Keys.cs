using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSLogerZabgc.API.Core
{
    public class Keys
    {
        public readonly string Key;
        public Keys(Api api)
        {
            switch (api)
            {
                case Api.Admin:
                   
                    break;
                case Api.SiteRedactor:
               
                    break;
                case Api.Developer:
                   
                    break;
                default:

                    break;
            }
        }

        public enum Api
        {
          Admin = 1,
          SiteRedactor = 2,
          Developer = 3
        }
    }

}

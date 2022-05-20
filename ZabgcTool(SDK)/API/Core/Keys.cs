using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabgcTool_SDK_.APIKeys
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

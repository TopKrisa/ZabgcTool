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
                    Key = "68c7a3cfe1b86a85bad1b580b91402c3";
                    break;
                case Api.SiteRedactor:
                    Key = "6d51c9216cc8ff221151b1ae8de95600";
                    break;
                case Api.Developer:
                    Key = "8cf7ef20d12f8bf09e2653b84ee8e514";
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

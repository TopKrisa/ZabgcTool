namespace OSSLogerZabgc
{
    public class TestsCondition
    {
        bool internet;
        bool os64;
        string osVersion;
        public bool Internet
        {
            get { return internet; }
            set { internet = value; }
        }
        public string OsVersion
        {
            get { return osVersion; }
            set { osVersion = value; }
        }

        public bool Os64
        {
            get { return os64; }
            set { os64 = value; }
        }
    }
}

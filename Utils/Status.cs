namespace ReactSupply.Utils
{
    public static class Status
    {
        public enum MessageType
        {
            SUCCESS = 0,
            FAILED = 1
        }

        public enum Method
        {
            Create = 0,
            Read = 1,
            Update = 2,
            Delete = 3
        }
    }
}

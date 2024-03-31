namespace Tinder_Admin.Models
{
    public class ResponseData<DataType> where DataType : class
    {
        public IEnumerable<DataType> Data { get; set; }

        public bool Succeed { get; set; } = true;

        public IEnumerable<string> ErrorList { get; set; } = new List<string>();

        public string Message { get; set; } = string.Empty;
    }

}

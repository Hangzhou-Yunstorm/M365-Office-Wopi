
namespace OfficeWopi_YunstormLib.Models
{
    public class EASAPIModel
    {
        public string Host { get; set; }
        public string TokenId { get; set; }
        public string DocId { get; set; }
    }

    public class EASLinkAPIModel
    {
        public string Host { get; set; }
        public string Link { get; set; }
        public string Password { get; set; }
        public string DocId { get; set; }
    }
}

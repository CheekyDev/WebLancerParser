using System.ComponentModel.DataAnnotations;

namespace FreelanceParser.Model
{
    public class UserInfo
    {
        [Key]
        public string Login       { get; set; }
        public string Name        { get; set; }
        public int    Age         { get; set; }
        public int    AvgPrice    { get; set; }
        public string Country     { get; set; }
        public string Experience  { get; set; }
        public string UserPicUrl  { get; set; }
        // public UserLink Url       { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace FreelanceParser.Model
{
    public class UserLink
    {
        [Key]
        public string Url { get; set; }
    }
}
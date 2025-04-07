using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    [Table("tbl_news")]
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(1000)]
        public string title { get; set; } = String.Empty;

        [Required, StringLength(1000)]
        public string slug { get; set; } = String.Empty;


        [Required, StringLength(2000)]

        public string summary { get; set; } = String.Empty;

        [Required, StringLength(3000)]

        public string content { get; set; } = String.Empty;
        [ StringLength(1000)]
        public string? Image { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginService.App.Models
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public DateTime? Horario { get; set; }

        [Required]
        public bool Disponivel { get; set; }

        [ForeignKey("Email")]
        public virtual User? Usuario { get; set; }
    }
}
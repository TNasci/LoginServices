using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginService.App.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? Nome { get; set; }

        [Required]
        public string? Sobrenome { get; set; }

        [Key]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(80)]
        public string? Especialidade { get; set; }
        public long? CRM { get; set; }

        public virtual ICollection<Schedule>? Agendas { get; set; }
    }
}

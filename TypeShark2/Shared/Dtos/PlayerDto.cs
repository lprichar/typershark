using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TypeShark2.Shared.Dtos
{
    public class PlayerDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Player name is too long.")]
        [DisplayName("Player Name")]
        public string Name { get; set; }
    }
}
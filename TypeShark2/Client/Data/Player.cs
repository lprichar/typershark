using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TypeShark2.Client.Data
{
    public class Player
    {
        [Required]
        [StringLength(50, ErrorMessage = "Player name is too long.")]
        [DisplayName("Player Name")]
        public string PlayerName { get; set; }
    }
}
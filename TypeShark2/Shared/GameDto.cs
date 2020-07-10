using System.Collections.Generic;

namespace TypeShark2.Shared
{
    public class GameDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public List<PlayerDto> Players { get; set; }
    }

    public class PlayerDto
    {
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace FutebolManager.Models
{
    public class Jogo
    {
        
        public string Id { get; set; } = Guid.NewGuid().ToString();

        
        public string TimeCasa { get; set; } = string.Empty;
        public string TimeVisitante { get; set; } = string.Empty;

        
        public DateTime Data { get; set; }

        
        public List<string> InteressadosIds { get; set; } = new();
    }
}

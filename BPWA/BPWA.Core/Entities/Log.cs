using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPWA.Core.Entities
{
    public class Log 
    {
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Exception { get; set; }
        [Column(TypeName = "jsonb")]
        public string Properties { get; set; }
        public string MachineName { get; set; }
    }
}

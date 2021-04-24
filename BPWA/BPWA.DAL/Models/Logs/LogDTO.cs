using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using System;

namespace BPWA.DAL.Models
{
    public class LogDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public LogEventLevel Level { get; set; }
        public string LevelString => TranslationsHelper.Translate(Level.ToString());
        public DateTime CreatedAt { get; set; }
        public string CreatedAtString => CreatedAt.ToString("dd.MM.yyyy HH:mm:ss");
        public string Exception { get; set; }
        public string Properties { get; set; }
        public string MachineName { get; set; }
    }
}

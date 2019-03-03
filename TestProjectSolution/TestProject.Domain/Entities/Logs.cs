using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TestProject.Domain.Entities.Base;

namespace TestProject.Domain.Entities
{
    public class Logs : BaseEntity<int>
    {
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Exception { get; set; }
        [Column(TypeName = "xml")]
        public string Properties { get; set; }
        public string LogEvent { get; set; }
    }
}

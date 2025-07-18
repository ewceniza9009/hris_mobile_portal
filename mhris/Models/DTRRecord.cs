using System;
using System.Collections.Generic;
using System.Text;

namespace mhris.Models
{
    public class SerializeSettings
    {
        public object ContentType { get; set; }
        public object SerializerSettings { get; set; }
        public object StatusCode { get; set; }
        public List<DTRRecord> Value { get; set; }
    }

    public class DTRRecord
    {
        public int Id { get; set; }
        public string DTRNumber { get; set; }
    }
}

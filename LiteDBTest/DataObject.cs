using Silmoon.Data.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDBTest
{
    public class DataObject : IdObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserToken { get; set; }
        public bool IsDefault { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public List<SubModel> Data3 { get; set; } = [];
    }
}
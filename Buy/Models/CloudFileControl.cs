using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class CloudFileControl
    {
        public CloudFileControl() { }

        public CloudFileControl(string id)
        {
            ID = id;
        }

        public string ID { get; set; }


        public void UseByCkEdit()
        {
            ID = "cloud";
        }

    }
}
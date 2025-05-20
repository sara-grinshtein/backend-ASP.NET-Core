using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class ResponseDto
    {
         
            public int response_id { get; set; }
            public int helped_id { get; set; }
            public string context { get; set; }
            public int rating { get; set; }
        

    }
}

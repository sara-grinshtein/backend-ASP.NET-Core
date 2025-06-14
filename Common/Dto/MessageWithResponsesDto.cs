using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class MessageWithResponsesDto
    {
        public MessageDto Message { get; set; }
        public List<ResponseDto> Responses { get; set; }
    }

}

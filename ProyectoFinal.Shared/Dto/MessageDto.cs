using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class MessageDto
    {
        public string Message { get; set; }

        public MessageDto(string message)
        {
            Message = message;
        }
    }
}

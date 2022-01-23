using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Mobile.Models
{
    public class PickerItem
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public PickerItem(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}

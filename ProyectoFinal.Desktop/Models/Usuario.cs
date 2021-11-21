using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Desktop.Models
{
    /*The following code example demonstrates the how to implement the INotifyPropertyChanged interface.
     * When you run this example, you will notice the bound DataGridView control reflects the change in the data source 
     * without requiring the binding to be reset.*/
    public class Usuario : INotifyPropertyChanged
    {
        public int UsuarioID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public bool Activo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ProyectoFinal.Desktop.Infrastructure.Helpers
{
    public class Dialog
    {
        public static MessageDialog InfoMessage(string title, string description)
        {
            var messageDialog = new MessageDialog("");
            messageDialog.Title = title;
            messageDialog.Content = description;
            messageDialog.Commands.Add(new UICommand("Aceptar"));
            messageDialog.CancelCommandIndex = 0;
            return messageDialog;
        }

        public static MessageDialog ConfirmationMessage(string title, string description)
        {

            var messageDialog = new MessageDialog("");
            messageDialog.Title = title;
            messageDialog.Content = description;
            messageDialog.Commands.Add(new UICommand("Cancelar")
            {
                Id = 0
            });
            messageDialog.Commands.Add(new UICommand("Aceptar")
            {
                Id = 1
            });
            messageDialog.DefaultCommandIndex = 1;
            messageDialog.CancelCommandIndex = 0;
            return messageDialog;
        }


    }



}

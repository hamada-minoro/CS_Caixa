using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassStatusChangedEventArgs : EventArgs
    {
        // Estamos interessados na mensagem descrevendo o evento
        private string EventMsg;

        // Propriedade para retornar e definir um mensagem do evento
        public string EventMessage
        {
            get { return EventMsg; }
            set { EventMsg = value; }
        }

        // Construtor para definir a mensagem do evento
        public ClassStatusChangedEventArgs(string strEventMsg)
        {
            EventMsg = strEventMsg;
        }
    }
}

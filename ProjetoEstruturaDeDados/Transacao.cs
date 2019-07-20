using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEstruturaDeDados
{
    public class Transacao
    {
        public Transacao()
        {
            Indice++;
            Fechada = false;
            PodeSerExcluida = true;
        }

        public static int Indice = 0;

        public bool Fechada { get; set; }

        public bool PodeSerExcluida { get; set; }
    }
}

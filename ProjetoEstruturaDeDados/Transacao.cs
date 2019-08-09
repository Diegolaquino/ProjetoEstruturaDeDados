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

        public int PegaIndice {
            get
            {
                return Indice;
            } 
        }
        private static int Indice = 0;

        public bool Fechada { get; set; }

        public bool PodeSerExcluida { get; set; }
    }
}

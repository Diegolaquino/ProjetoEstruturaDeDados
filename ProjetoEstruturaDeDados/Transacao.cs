using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEstruturaDeDados
{
    public class Transacao
    {
        public Transacao(int indice)
        {
            PegaIndice = indice;
            Fechada = false;
            //PodeSerExcluida = true;
        }

        public int PegaIndice { get; }

        public bool Fechada { get; set; }

        //public bool PodeSerExcluida { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEstruturaDeDados
{
    public class Transacao
    {
        public Transacao(int indice)
        {
            Indice = indice;
            Fechada = false;
            PodeSerExcluida = true;
        }

        public int PegaIndice {
            get
            {
                return Indice;
            } 
        }
        private int Indice;

        public bool Fechada { get; set; }

        public bool PodeSerExcluida { get; set; }
    }
}

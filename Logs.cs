using System;
using System.Collections.Generic;

namespace logistica {
  public class LogisticaException : Exception {

    public LogisticaException(string EId) {
      switch(EId) {
        case "teste": Console.WriteLine("Erro identificado!!!"); break;
      }

    }

  }
}
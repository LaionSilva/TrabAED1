
using System;
using System.Collections.Generic;

namespace logistica {
  class MainClass {
    public static void Main (string[] args) {
      Console.WriteLine ("\nParabéns! O código esta copilando");

      Distribuidora mercado = new Distribuidora();

      mercado.NovoCliente("Laion");
      mercado.Ofertar();
      mercado.Vender();

    }
  }
}
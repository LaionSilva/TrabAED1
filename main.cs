
using System;
using System.Collections.Generic;

namespace logistica {
  class MainClass {
    public static void Main (string[] args) {
      Console.WriteLine ("\nParabéns! O código esta copilando");

      Distribuidora mercado = new Distribuidora();

      //mercado.NovoCliente("Laion1");
      //mercado.NovoCliente("Laion2");
      //mercado.NovoCliente("Laion3");      

      mercado.NovoCaminhao();
      mercado.Ofertar();
      mercado.Vender();

      //mercado.Salvar();

    }
  }
}
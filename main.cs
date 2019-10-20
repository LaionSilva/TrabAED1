
using System;
using System.Collections.Generic;

namespace logistica {
  class MainClass {
    public static void Main (string[] args) {
      Console.WriteLine ("\nParabéns! O código esta copilando");

      Distribuidora mercado = new Distribuidora();
/*
      int[,] result = mercado.rota.Combinar(4);
      for(int z = 0; z < 24; z++) {
        for(int k = 0; k < 4; k++) 
          { Console.Write("{0} ", result[k, z]); }
        Console.WriteLine();
      } 
*/

      //mercado.NovoCliente("Laion1");
      //mercado.NovoCliente("Laion2");
      //mercado.NovoCliente("Laion3");      

      //mercado.NovoCaminhao();
      mercado.Ofertar();
      mercado.Vender();

      //mercado.Salvar();

    }
  }
}
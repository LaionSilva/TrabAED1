
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
      Console.WriteLine ("\nfim");

      mercado.NovoCliente("Laion1");
      mercado.NovoCliente("Laion2");
      mercado.NovoCliente("Laion3"); 
      mercado.NovoCliente("Laion4");
      mercado.NovoCliente("Laion5");
      mercado.NovoCliente("Laion6");
      mercado.NovoCliente("Laion7");
      mercado.NovoCliente("Laion8");
      mercado.NovoCliente("Laion9");     
*/
      mercado.Ofertar();
      mercado.Vender();
      //mercado.OrganizarEncomendas();
      mercado.ComoViajar();

      //mercado.Salvar();

    }
  }
}
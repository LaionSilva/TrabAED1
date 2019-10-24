
using System;
using System.Collections.Generic;

namespace logistica {
  class MainClass {
    private static Distribuidora mercado = new Distribuidora();
    private static string titulo =  "\n== 0 == 1 == 0 ==  A N Ô N I M U S   H . L  == 1 == 0 == 1 ==" +
                                    "\n== 1 == 0 == 1 =  D I S T R I B U I D O R A  = 0 == 1 == 0 ==";

    public static void Main () {
      Distribuidora mercado = new Distribuidora();

      Console.Clear();
        Console.WriteLine (
          "_________________________________________________________________________________________\n" +
          "___######__###____##__######__###____##_##____##_####____####__######__##____##_#######__\n" + 
          "__##____##_##_##__##_##____##_##_##__##__##__##__##_##__##_##_##____##_##____##_##_______\n" + 
          "__########_##__##_##_##____##_##__##_##___####___##__####__##_##____##_##____##_#######__\n" + 
          "__##____##_##___####_##____##_##___####____##____##___##___##_##____##_##____##______##__\n" + 
          "__##____##_##____###__######__##____###____##____##________##__######___######__#######__\n" + 
          "_________________________________________________________________________________________" 
        );
        System.Threading.Thread.Sleep(1000);
      Console.Clear();

      Menu(); 
      Console.ReadKey(); 
    }

    public static void Menu() {
      bool loop = true;

      while(loop) {
        switch (Cabecario()) { 
          case "CC": CadastarCliente(); break;
          case "OF": OfertarProdutos(); break;
          case "VD": VenderProdutos(); break;
          case "CR": CalcularViagen(); break;
          //case "EOV1": <comando> ;break;
          case "ER": GerarRelatório(); break;
          case "ANONYMUS@RESET": mercado.ResetarFiles(); break;
          case "EXIT": loop = false; break;
          default: Console.WriteLine("Escolha uma opção válida!\n"); break;
        }
        Console.ReadKey(); 
      }
    }

    public static string Cabecario() {
      string asw = "";
      Console.Clear();
      Console.WriteLine (titulo);
      
      Console.WriteLine("\nEscolha a operação desejada ou EXIT para sair: (Digite o código)");
      Console.WriteLine("CC - Cadastrar Cliente");
      Console.WriteLine("OF - Ofertar Produtos");
      Console.WriteLine("VD - Vender Produtos");
      //Console.WriteLine("CC01 - Vender ");
      Console.WriteLine("CR - Calcular Rota");
      //Console.WriteLine("EOV1 - Emitir Ordem de viagem");
      Console.WriteLine("ER - Emitir Relatório de viagem"); 
      Console.Write("\nCódigo: ");      
      asw = Console.ReadLine();//Console.ReadLine(); 
      asw = asw.ToUpper(); 
      Console.Clear();
      Console.WriteLine (titulo);
      return asw;
    }

    public static void CadastarCliente(){
      while(true) {
        Console.WriteLine("\nInforme o nome do cliente");
        string nome = Console.ReadLine();
        if(mercado.getClientes(nome) == -1) {
          mercado.NovoCliente(nome);
          break;
        }
        Console.WriteLine("Cliente já posssui cadastro!");
        Console.WriteLine("Deseja Cadastrar outro cliente?(y/n)"); 
        string asw = Console.ReadLine().ToUpper();
        if(asw == "N") { break; }
      }
    }

    public static void OfertarProdutos(){
      Console.WriteLine("\nOfertando Produtos...");
      mercado.Ofertar();
    }

    public static void VenderProdutos(){
      Console.WriteLine("\nVendendo Produtos...");
      mercado.Vender(); 
    }

    public static void CalcularViagen(){
      mercado.ComoViajar();
    }

    public static void GerarRelatório(){
      Console.WriteLine("Gerando relatório...");
      mercado.ImprimirRelatório();
    }

  }
}


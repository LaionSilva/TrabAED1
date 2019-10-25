
using System;
using System.Collections.Generic;

namespace logistica {
  class MainClass {
    private static Distribuidora mercado = new Distribuidora();
    private static string titulo =  "\n== 0 == 1 == 0 ==  A N Ô N I M U S   H . L  == 1 == 0 == 1 ==" +
                                    "\n== 1 == 0 == 1 =  D I S T R I B U I D O R A  = 0 == 1 == 0 ==";
    private static string fimProcesso =  "\n== 0 == 1 == 0 ==  A N Ô N I M U S   H . L  == 1 == 0 == 1 ==" +
                                    "\n== 1 == 0 == 1 ==  D E S C O N E C T A D O  == 0 == 1 == 0 ==";
    private static string fachada = 
      "_________________________________________________________________________________________\n" +
      "___######__###____##__######__###____##_##____##_####____####__######__##____##_#######__\n" + 
      "__##____##_##_##__##_##____##_##_##__##__##__##__##_##__##_##_##____##_##____##_##_______\n" + 
      "__########_##__##_##_##____##_##__##_##___####___##__####__##_##____##_##____##_#######__\n" + 
      "__##____##_##___####_##____##_##___####____##____##___##___##_##____##_##____##______##__\n" + 
      "__##____##_##____###__######__##____###____##____##________##__######___######__#######__\n" + 
      "_________________________________________________________________________________________";


    public static void Main () {
      Distribuidora mercado = new Distribuidora();

      Console.Clear();
        Console.WriteLine (fachada);
        System.Threading.Thread.Sleep(3000);
      Console.Clear();
      Console.WriteLine (titulo);

      bool loop = true;
      while(loop) {
        Console.WriteLine("\nO - Operacional | A - Administrativo | EXIT - Desconectar");
        Console.Write("\nEscolha o setor desejado... \nSetor: ");
        switch (Console.ReadLine().ToUpper()) { 
          case "O": MenuOperacional(); break;
          case "A": MenuAdministrativo(); break;
          case "ANONYMUS@RESET": mercado.ResetarFiles(); break;
          case "EXIT": loop = false; break;
          default: Console.WriteLine("\nComando inválido!\n"); break;
        }
      }

      Console.Clear();
      Console.WriteLine (fimProcesso);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  OPERACIONAL
    public static string CabecarioOperacional() {
      string asw = "";
      Console.Clear();
      Console.WriteLine (titulo);      
      Console.WriteLine("\nEscolha a operação desejada ou EXIT para sair: (Digite o código)");
      Console.WriteLine("OF - Ofertar Produtos");
      Console.WriteLine("VD - Vender Produtos");
      //Console.WriteLine("CC01 - Vender ");
      Console.WriteLine("CR - Calcular Rota");
      //Console.WriteLine("EOV1 - Emitir Ordem de viagem");
      Console.WriteLine("ER - Emitir Relatório de viagem"); 
      Console.WriteLine("EXIT - Voltar");
      Console.Write("\nCódigo: ");      
      asw = Console.ReadLine().ToUpper();
      Console.Clear();
      Console.WriteLine (titulo);
      return asw;
    }

    public static void MenuOperacional() {
      bool loop = true;

      while(loop) {
        string comando = CabecarioOperacional();
        switch (comando) {
          case "OF": OfertarProdutos(); break;
          case "VD": VenderProdutos(); break;
          case "CR": CalcularViagen(); break;
          //case "EOV1": <comando> ;break;
          case "ER": GerarRelatório(); break;
          case "EXIT": loop = false; break;
          default: Console.WriteLine("\nComando inválido!\n"); break;
        }
        if(comando != "EXIT"){
          Console.Write("Digite qualquer coisa para continuar... ");
          Console.ReadKey(); 
        }        
      }
      Console.Clear();
    }
    //  OPERACIONAL
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  ADMINISTRATIVO
    public static string CabecarioAdministrativo() {
      string asw = "";
      Console.Clear();
      Console.WriteLine (titulo);
      Console.WriteLine("\nEscolha a operação desejada ou EXIT para sair: (Digite o código)");
      Console.WriteLine("CC - Cadastrar Cliente");
      Console.WriteLine("CP - Cadastrar Produto");
      Console.WriteLine("BC - Banco de Dados Clientes");
      Console.WriteLine("EXIT - Voltar");
      Console.Write("\nCódigo: ");      
      asw = Console.ReadLine();
      asw = asw.ToUpper(); 
      Console.Clear();
      Console.WriteLine (titulo);
      return asw;
    }

    public static void MenuAdministrativo() {
      bool loop = true;

      while(loop) {
        string comando = CabecarioAdministrativo();
        switch (comando) { 
          case "CC": CadastarCliente(); break;
          case "CP": CadastarProduto(); break;
          case "BC": BancoClientes(); break;
          case "EXIT": loop = false; break;
          default: Console.WriteLine("\nEscolha uma opção válida!"); break;
        }
        if(comando != "EXIT"){
          Console.Write("Digite qualquer coisa para continuar... ");
          Console.ReadKey(); 
        } 
      }
      
    }
    // ADMINISTRATIVO
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  MÉTODOS OPERACIONAIS
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
    //  MÉTODOS OPERACIONAIS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  MÉTODOS ADMINISTRATIVOS
    public static void CadastarCliente(){
      while(true) {
        Console.Write("\nInforme o nome do cliente\nNome: ");
        string nome = Console.ReadLine();
        if(mercado.getClientes(nome) == -1) {
          mercado.NovoCliente(nome);
          Console.WriteLine("Cliente Cadastrado com Sucesso!\n");
          break;
        }
        Console.WriteLine("Cliente já posssui cadastro!");
        Console.Write("Deseja Cadastrar outro cliente? (y/n): "); 
        if(Console.ReadLine().ToUpper() == "N") { break; }
      }
    }

    public static void CadastarProduto(){
      while(true) {
        Console.WriteLine("\nInforme os seguintes dados do novo produto:");
        Console.Write("\nNome: ");
        string nome = Console.ReadLine();
        Console.Write("Preço de custo R$: ");
        double custo = double.Parse(Console.ReadLine());
        Console.Write("Peso em Kg: ");
        double peso = double.Parse(Console.ReadLine());
        Console.Write("Volume em metro cúico: ");
        double volume = double.Parse(Console.ReadLine());
        if(mercado.NovoProduto(nome, custo, peso, volume) == true) {
          Console.WriteLine("\nProduto Cadastrado com Sucesso!\n");
          break;
        }
        Console.WriteLine("\nProduto já posssui cadastro!");
        Console.Write("Deseja Cadastrar outro Produto?(y/n) "); 
        if(Console.ReadLine().ToUpper() == "N") { break; }
      }
    }

    public static void BancoClientes(){
      Console.WriteLine("\nClientes cadastrados:");
      mercado.ListarClientes();
      Console.WriteLine();
    }
    //  MÉTODOS ADMINISTRATIVOS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    
  }
}


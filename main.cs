using System;
using System.Collections.Generic;

namespace logistica {
  class MainClass {
    private static Distribuidora mercado = new Distribuidora();

    private static string titulo =  
    "\n=== 0 == 1 == 0 ====  A N Ô N I M O U S   H . L  ==== 1 == 0 == 1 ===" +
    "\n=== 1 == 0 == 1 ===   D I S T R I B U I D O R A   === 0 == 1 == 0 ===" +
    "\nCode by: Higor Parnoff | Laion Fernandes - Engenharia de Computação";

    private static string fimProcesso = 
    "\n=== 0 == 1 == 0 ====  A N Ô N I M O U S   H . L  ==== 1 == 0 == 1 ===" +
    "\n=== 1 == 0 == 1 ====   D E S C O N E C T A D O   ==== 0 == 1 == 0 ===";

    private static string fachada = 
      "                    #                                                        \n" +
      "                  ## ##                                                      \n" +
      "  #####  ##   ##  #####  ##   ## ##    ## ###     ###  #####  ##   ## ###### \n" + 
      " ##   ## ###  ## ##   ## ###  ##  ##  ##  ## ## ## ## ##   ## ##   ## ##     \n" + 
      " ####### ## # ## ##   ## ## # ##   ####   ##  ###  ## ##   ## ##   ## ###### \n" + 
      " ##   ## ##  ### ##   ## ##  ###    ##    ##   #   ## ##   ## ##   ##     ## \n" + 
      " ##   ## ##   ##  #####  ##   ##    ##    ##       ##  #####   #####  ###### \n";
      

    public static void Main () {
      //Console.ReadKey(); 
      Distribuidora mercado = new Distribuidora();

      Console.Clear();
        Console.WriteLine (fachada);
        System.Threading.Thread.Sleep(3000);
      Console.Clear();
      Console.WriteLine (titulo);

      bool loop = true;
      while(loop) {
        Console.WriteLine("\nO - Setor operacional | A - Setor administrativo | SAIR - Desconectar");
        Console.Write("\nEscolha o setor desejado... \nSetor: ");
        switch (Console.ReadLine().ToUpper()) { 
          case "O": MenuOperacional(); break;
          case "A": MenuAdministrativo(); break;
          case "ANONYMOUS@RESET": mercado.ResetarFiles(); break;
          case "SAIR": loop = false; break;
          default: 
            Console.Clear();
            Console.WriteLine (titulo);
            Console.WriteLine("\nComando inválido!\n"); 
            Console.ReadKey(); 
            Console.Clear();
            Console.WriteLine (titulo);
            break;
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
      Console.WriteLine("\nEscolha a operação desejada: (Digite o código)");
      Console.WriteLine("OF - Ofertar Produtos");
      Console.WriteLine("VD - Vender Produtos");
      //Console.WriteLine("CC01 - Vender ");
      Console.WriteLine("CR - Calcular Rota");
      //Console.WriteLine("EOV1 - Emitir Ordem de viagem");
      //Console.WriteLine("ER - Emitir Relatório de viagem"); 
      Console.WriteLine("VOLTAR - Menu inicial");
      Console.WriteLine("SAIR - Desconectar");
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
          //case "ER": GerarRelatório(); break;
          case "VOLTAR": loop = false; break;
          case "SAIR" : 
            Console.Clear();
            Console.WriteLine(fimProcesso);
            Environment.Exit(0); 
            break;
          default: 
            Console.Clear();
            Console.WriteLine (titulo);
            Console.WriteLine("\nComando inválido!"); 
            break;
        }
        if(comando != "VOLTAR"){
          Console.Write("\nDigite qualquer coisa para continuar... ");
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
      Console.WriteLine("\nEscolha a operação desejada: (Digite o código)");
      Console.WriteLine("CC - Cadastrar Cliente");
      Console.WriteLine("CP - Cadastrar Produto");
      Console.WriteLine("LC - Listar Clientes");
      Console.WriteLine("LP - Listar Produtos");
      Console.WriteLine("CPR - Comprar Produto");
      Console.WriteLine("VOLTAR - Menu inicial");
      Console.WriteLine("SAIR - Desconectar");
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
          case "LC": BancoClientes(); break;
          case "LP": BancoProdutos(); break;
          case "CPR": ComprarProdutos(); break;
          case "VOLTAR": loop = false; break;
          case "SAIR" : 
            Console.Clear();
            Console.WriteLine(fimProcesso);
            Environment.Exit(0); 
            break;
          default: 
            Console.Clear();
            Console.WriteLine (titulo);
            Console.WriteLine("\nComando inválido!"); 
            break;
        }
        if(comando != "VOLTAR"){
          Console.Write("\nDigite qualquer coisa para continuar... ");
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

    public static void CalcularViagen() {
      while(true) {
        Console.Write("\nQuantos containers deseja acoplar ao caminhão?\n>> ");
        try{
          int containers = int.Parse(Console.ReadLine());
          if(containers > 3) {
            throw new LogisticaException("LE_Main_LimiteContainers");
          }
          else{
            mercado.ComoViajar(containers);
            break;
          }          
        }
        catch(LogisticaException) {}
        catch(FormatException) { 
          Console.WriteLine("\nValor invalido"); 
        }
        catch(Exception e) { 
          LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Main", "CalcularViagen"); 
        }

        Console.Write("\nDeseja tentar denovo?(y/n)\n>>  ");         
        if(Console.ReadLine().ToUpper() == "Y") { 
          Console.Clear();
          Console.WriteLine (titulo);
          CalcularViagen(); 
        } else { Console.WriteLine("Fim da operação\n");  }
        break;
      }
    }
    //  MÉTODOS OPERACIONAIS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  MÉTODOS ADMINISTRATIVOS
    public static void CadastarCliente(){
      while(true) {
        try {
          Console.Write("\nInforme o nome do cliente\nNome: ");
          string nome = Console.ReadLine();

          if(mercado.getClientes(nome) == -1) {
            if(nome.Length < 5) {
              throw new LogisticaException("LE_Main_NovoClienteNomeVazio");
            } 
            else { 
              mercado.NovoCliente(nome); 
              Console.WriteLine("Cliente Cadastrado com Sucesso!\n");
              break;
            }
          }
          else { throw new LogisticaException("LE_Main_NovoClienteNomeDuplicado"); }
        } 
        catch(LogisticaException) {
          Console.Write("\nDeseja Cadastrar outro cliente? (y/n):\n>> "); 
          if(Console.ReadLine().ToUpper() == "Y") {
            Console.Clear();
            Console.WriteLine (titulo);
            CadastarCliente(); 
          } else { Console.WriteLine("Fim da operação\n");  }
          break; 
        }
        catch(Exception e) { 
          LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Main", "CadastarCliente", true); 
        }
      }
    }

    public static void CadastarProduto(){
      while(true) {
        Console.WriteLine("\nInforme os seguintes dados do novo produto:");
        Console.Write("\nNome: ");
        
        try{
          string nome = Console.ReadLine();
          if(nome.Length == 0) {
            throw new LogisticaException("LE_Main_NovoProdutoNomeVazio");
          }
          Console.Write("Preço de custo R$: ");
          double custo = double.Parse(Console.ReadLine());
          Console.Write("Peso em Kg: ");
          double peso = double.Parse(Console.ReadLine());
          Console.Write("Volume em metro cúbico: ");
          double volume = double.Parse(Console.ReadLine());

          if(mercado.NovoProduto(nome, custo, peso, volume) == true) {
            Console.WriteLine("\nProduto Cadastrado com Sucesso!\n");
            break;
          } 
          else { throw new LogisticaException("LE_Main_NovoProdutoNomeDuplicado"); }
        }
        catch(FormatException) { 
          Console.WriteLine("\nValor invalido detectado"); 
        }
        catch(LogisticaException) {}
        catch(Exception e) { 
          LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Main", "CadastarProduto", true); 
        }

        Console.Write("\nDeseja Cadastrar outro Produto?(y/n)\n>>  ");         
          if(Console.ReadLine().ToUpper() == "Y") { 
            Console.Clear();
            Console.WriteLine (titulo);
            CadastarProduto(); 
          } else { Console.WriteLine("Fim da operação\n");  }
          break;
      }
    }

    public static void BancoClientes(){
      Console.WriteLine("\nClientes cadastrados:");
      mercado.ListarClientes();
      Console.WriteLine();
    }

    public static void BancoProdutos(){
      Console.WriteLine("\nProdutos cadastrados:");
      mercado.ListarProdutos();
      Console.WriteLine();
    }

    public static void ComprarProdutos(){
      while(true) {
        string nome = "";

        try{
          Console.WriteLine("\nInforme os seguintes dados:");
          Console.Write("\nNome: ");
          nome = Console.ReadLine();
          Console.Write("Quantidade: ");
          
          int quant = int.Parse(Console.ReadLine()); 
          if(mercado.ComprarProduto(nome, quant)) {}
        }
        catch(FormatException) { 
          Console.WriteLine("\nQuantidade não aceita\n"); 
        }
        catch(Exception e) { 
          LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Main", "ComprarProdutos"); 
        }

        Console.Write("\nDeseja continuar comprando?(y/n)\n>>  ");         
        if(Console.ReadLine().ToUpper() == "Y") { 
          Console.Clear();
          Console.WriteLine (titulo);
          ComprarProdutos(); 
        } else { Console.WriteLine("Fim da operação\n");  }
        break; 
      }
    }
    //  MÉTODOS ADMINISTRATIVOS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    
  }
}


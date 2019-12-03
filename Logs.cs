using System;
using System.Collections.Generic;

namespace logistica {
  public class LogisticaException : Exception { 
    private static Save file = new Save();
    private static string mensFinal = "\nFim da operação";

    public LogisticaException(string tipoException, Exception mensagem = null, string classe = "", string metodo = "", bool infNota = false) {
      string infNoteAdm = "";

      switch(tipoException) { //  GERAÇÃO DE LOGS E MENSAGENS DE ERRO
        //  Main: Exceptions manipulaveis
        case "LE_Main_NovoClienteNomeVazio": 
          Console.WriteLine("Operação mal sucedida\nNome muito pequeno. Informe um nome com no mínimo 5 caracteres");
          break;
          
        case "LE_Main_NovoClienteNomeDuplicado": 
          Console.WriteLine("Operação mal sucedida\nCliente já cadastrado"); 
          break;

        case "LE_Main_NovoProdutoNomeVazio": 
          Console.WriteLine("Operação mal sucedida\nNome do produto vazio"); 
          break;

        case "LE_Main_NovoProdutoNomeDuplicado":
          Console.WriteLine("Operação mal sucedida\nNome do produto já cadastrado!"); 
          break;


        //  Distribuidora: Exceptions manipulaveis
        case "LE_Distribuidora_NovoRelatorioEntrega_DiarioEntrega":
          Console.WriteLine("Erro grave\n" + 
                            "O novo relatório não pode ser arquivado por falta de informações\n" + 
                            "Por prevenção o administrador será avisado\n");
          RegistrarLogException(tipoException, mensagem, classe, metodo, infNota); 
          break;

        case "LE_Distribuidora_GerarRelatório_indexClientes":
          Console.WriteLine("Erro grave\n" +
                            "Cliente não encontrado" +
                            "Por prevenção o administrador será avisado\n"); 
          infNoteAdm = "Nota do desenvolvedor: Indice da lista clientes igual a -1 ou fora do range.";
          RegistrarLogException(tipoException, mensagem = null, "Distribuidora", "GerarRelatório", infNota); 
          break;
      }
    }

    public static void ExceptionGrave(string tipoException, Exception mensagem = null, string classe = "", string metodo = "", bool infNota = false) {
      string infNoteAdm = "";

      switch(tipoException) { //  GERAÇÃO DE LOGS E MENSAGENS DE ERRO
        //  Save: Exceptions manipulaveis
        case "LE_Save":
          Console.WriteLine("Erro grave\n" + 
                            "Foi detectado uma falha grave ao salvar, as últimas ações poderão ser perdidas ou fragmentadas\n" + 
                            "O administrador deverá ser avisado\n");
          RegistrarLogException(tipoException, mensagem, classe, metodo, true); 
          break;


        //  Main: Exceptions graves - gerar de logs de erro 
        case "LE_ExceptionNaoTratada": 
          Console.Write("\n========== ERRO INESPERADO ==========\n" +
                        "O administrador deverá ser avisado\n"); 
          RegistrarLogException(tipoException, mensagem, classe, metodo, infNota);
          break;

        case "LE_IndexOutOfRangeException": 
          Console.Write("\n========== ERRO IDENTIFICADO ==========\n" +
                        "Não foi possivel executar {0}:{1}().\n" +
                        "Erro: IndexOutOfRangeException: Índice inválido ao acessar um membro de um array, uma coleção, ou para ler ou gravar de um local específico em um buffer.\n" +
                        "O administrador deverá ser avisado\n", 
                        classe, metodo); 
          infNoteAdm = "Nota do desenvolvedor: Índice inválido ao acessar um membro de um array, uma coleção, ou para ler ou gravar de um local específico em um buffer.";
          RegistrarLogException(tipoException, mensagem, classe, metodo, infNota, infNoteAdm);
          break;

        case "LE_DivideByZeroException": 
          Console.Write("\n========== ERRO IDENTIFICADO ==========\n" +
                        "Não foi possivel executar {0}:{1}().\n" +
                        "Erro: DivideByZeroException: Foi identificada uma divisão por zero.\n" +
                        "O administrador deverá ser avisado\n", 
                        classe, metodo); 
          infNoteAdm = "Nota do desenvolvedor: Foi identificada uma divisão por zero.";
          RegistrarLogException(tipoException, mensagem, classe, metodo, infNota, infNoteAdm);
          break;


        //  Save: Erro em gravar logs de erro
        case "LE_Save_Exception": 
          Console.Write("\n========== ERRO GRAVE INESPERADO ==========\n" +
                        "Não foi possivel executar {0}:{1}().\n" +
                        "Erro: LE_Save_Exception: Erro grave ao gerar logs de erro. Sistema de manutenção contra erros comprometido\n" +
                        "O administrador deverá ser avisado\n", 
                        classe, metodo); 
          break;
      }
    }

    private static void RegistrarLogException(string te, Exception ms = null, string cl = "", string me = "", bool infNota = false, string infNoteAdm = "") {
      DadosLogException newException = new DadosLogException();

      newException.tipo = te;
      newException.classe = cl;
      newException.metodo = me;
      newException.data = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
      newException.mensagem = ms;
      newException.notaAdm = infNoteAdm;
      if(infNota) {
        Console.Write("\nNos informe um pouco sobre o ocorrido para analizarmos uma solução mais eficiente:\n>> ");
        newException.nota = Console.ReadLine();
      }

      file.setLogException(newException);
    }

  }
}
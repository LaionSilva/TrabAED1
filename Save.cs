using System;
using System.Collections.Generic;
using System.IO;

namespace logistica {
  public class Save {

    private List<Produto> produtos = new List<Produto>(); 
    private List<Encomenda> encomendas = new List<Encomenda>();
    private List<Produto> pedidos = new List<Produto>();
    private List<Relatorio> diarioEmpresarial = new List<Relatorio>(); 
    private List<Caminhao> frota = new List<Caminhao>(); 
	  private List<Cliente> clientes = new List<Cliente>(); 
    private string file = "dados.txt";

    public Save() {
      CheckArquivo();
    }

    public void setProdutos(List<Produto> p){
      produtos = p;
      Guardar();
    }
    public List<Produto> getProdutos(){
      Carregar();
      return produtos;
    }

    public void setClientes(List<Cliente> c){
      clientes.AddRange(c);
      Guardar();
    }
    public List<Cliente> getClientes(){
      Carregar();
      return clientes;
    }

    private void Carregar(){ //  Get do dados.txt para todas as listas
      string lendo = "vazio", aux;
      string key = "";
      using(Stream FileIn = File.Open(file, FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          while (lendo != null) { lendo = Carregar.ReadLine();

            if(lendo == "#produtos") { key = lendo; }
            else if(lendo == "produtos#") { key = ""; }

            switch(key){ 
              case "#produtos": do{ aux = Carregar.ReadLine();
                                  if(aux == "--") {
                                    produtos.Add( new Produto( Carregar.ReadLine(), int.Parse(Carregar.ReadLine()), double.Parse(Carregar.ReadLine()) ) );
                                    Console.WriteLine(produtos.Count); 
                                  } 
                                } while (aux == "--"); 
                                lendo = aux; break;
            }

          }
        }
      }
    }

    public void Guardar(){ //  Set no dados.txt os dados de todas as listas 
      System.IO.File.Delete(file);
      using (StreamWriter Salvar = File.AppendText(file)) { 
        Salvar.WriteLine("#produtos"); 
        foreach(Produto p in produtos){
          Salvar.WriteLine("--"); 
          Salvar.WriteLine(p.getTipo()); 
          Salvar.WriteLine(p.getQuantidade()); 
          Salvar.WriteLine(p.getCusto()); 
        }
        Salvar.WriteLine("produtos#"); 
      }
    }

    private void CheckArquivo(){ //  Verifica a existencia de um arquivo.txt, caso não exista é criado um
      if (!System.IO.File.Exists(file)){
        using (StreamWriter Salvar = File.AppendText(file)) {}
      }
    }

  }
}
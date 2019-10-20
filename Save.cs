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
	  private Cliente clientes = new Cliente(); 
    private List<Cliente> Clientes = new List<Cliente>(); 

    //Endereço dos arquivos salvos
    private string fileProdutos = "file_produtos.txt";
    private string fileClientes = "file_clientes.txt";
    private string fileEncomendas = "file_encomendas.txt";

    public Save() { CheckArquivo(); }

    //GETs das Listas
    public List<Produto> getProdutos() {
      Carregar("produtos");
      return produtos;
    }

    public List<Cliente> getClientes(){
      Carregar("clientes");
      return Clientes;
    }

    public List<Encomenda> getEncomendas(){
      Carregar("encomendas");
      return encomendas;
    }
    

    //SETs das Listas
    public void setProdutos(List<Produto> p) {
      produtos = p;
      Guardar("produtos");
    }

    public void setClientes(List<Cliente> c) {
      double[] coord = new double[2];
      if(c.Count > 0){
        System.IO.File.Delete(fileClientes);
        CheckArquivo();
        foreach(Cliente cl in c) {
          coord = cl.getCoord();
          clientes = new Cliente(cl.getId(), cl.getNome(), coord[0], coord[1], cl.getTendencia());
          pedidos.Clear();
          pedidos.AddRange(cl.getPedidos());
          Guardar("clientes");
        }  
      }        
    }    

    public void setEncomendas(List<Encomenda> e) {
      encomendas = e;
      Guardar("encomendas");
    }
   

    private void Carregar(string dado){ //  Get do dados.txt para todas as listas
      switch(dado){        
        case "produtos": CarregarProdutos(); break;
        case "clientes": CarregarClientes(); break;
        case "encomendas": CarregarEncomendas(); break;
      }
    }
    
    public void Guardar(string dado){ //  Set no dados.txt os dados de todas as listas 
      switch(dado){
        case "produtos": GuardarProdutos(); break;
        case "clientes": GuardarClientes(); break;
        case "encomendas": GuardarEncomendas(); break;
      }
    }

    private void CheckArquivo(){ //  Verifica a existencia de um arquivo.txt, caso não exista é criado um
      if (!System.IO.File.Exists(fileProdutos))
        { using (StreamWriter Salvar = File.AppendText(fileProdutos)) {} }
      if (!System.IO.File.Exists(fileClientes))
        { using (StreamWriter Salvar = File.AppendText(fileClientes)) {} }
      if (!System.IO.File.Exists(fileEncomendas))
        { using (StreamWriter Salvar = File.AppendText(fileEncomendas)) {} }
    }


    //Métodos responsaveis por realizar os GETs
    private void CarregarProdutos(){
      string lendo = "";
      using(Stream FileIn = File.Open(fileProdutos, FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          do{ lendo = Carregar.ReadLine();
            if(lendo == "--") {
              produtos.Add( new Produto( 
                Carregar.ReadLine(), 
                int.Parse(Carregar.ReadLine()), 
                double.Parse(Carregar.ReadLine()), 
                double.Parse(Carregar.ReadLine()), 
                double.Parse(Carregar.ReadLine()) 
              ) );
              //Console.WriteLine(produtos.Count + lendo); //  TESTE
            } 
          } while (lendo != null); 
        }
      }
    }

    private void CarregarClientes(){
      string lendo = "";
      using(Stream FileIn = File.Open(fileClientes, FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          double[] coord = new double[2];
          int id = 0, tendencia = 0;
          string nome = "";
          bool gravar = false;

          lendo = Carregar.ReadLine();
          do{
            if(lendo == "--") {           
              id = int.Parse(Carregar.ReadLine());
              nome = Carregar.ReadLine();
              coord[0] = int.Parse(Carregar.ReadLine());
              coord[1] = int.Parse(Carregar.ReadLine());
              tendencia = int.Parse(Carregar.ReadLine());
              clientes = new Cliente(id, nome, coord[0], coord[1], tendencia);

              lendo = Carregar.ReadLine();
              gravar = true;
            } 
            if((lendo == "-#") && (gravar)) {
              while (lendo == "-#"){
                pedidos.Add( new Produto( 
                  Carregar.ReadLine(), 
                  int.Parse(Carregar.ReadLine()), 
                  double.Parse(Carregar.ReadLine()),
                  double.Parse(Carregar.ReadLine()),
                  double.Parse(Carregar.ReadLine()) 
                ) );
                lendo = Carregar.ReadLine();
              }    
              if(pedidos.Count > 0) 
                { clientes.setPedidos(pedidos); }
              Clientes.Add( clientes );    
            }
            else if(gravar) 
              { Clientes.Add( clientes ); }

            gravar = false; 
            //Console.WriteLine(Clientes.Count + lendo); //  TESTE   
          } while (lendo != null); 

        }
      }
    }

    private void CarregarEncomendas(){
      string lendo = "", cliente = "", data = "";
      int id = 0, prazo = 0;
      double preco = 0, frete = 0;  
      bool status = false;  
      using(Stream FileIn = File.Open(fileEncomendas, FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          lendo = Carregar.ReadLine(); 
          do{
            List<Produto> produtosEn = new List<Produto>();
            if(lendo == "--") {
              id = int.Parse(Carregar.ReadLine());
              preco = double.Parse(Carregar.ReadLine());
              cliente = Carregar.ReadLine();
              frete = double.Parse(Carregar.ReadLine());
              prazo = int.Parse(Carregar.ReadLine());
              data = Carregar.ReadLine();
              if(Carregar.ReadLine() == "true") { status = true; }
              else { status = false; }
              lendo = Carregar.ReadLine();
                     
              while (lendo == "-#"){
                produtosEn.Add( 
                  new Produto(Carregar.ReadLine(), 
                  int.Parse(Carregar.ReadLine()), 
                  double.Parse(Carregar.ReadLine()),
                  double.Parse(Carregar.ReadLine()),
                  double.Parse(Carregar.ReadLine())
                ) );  
                lendo = Carregar.ReadLine();
              }  
              encomendas.Add( new Encomenda(id, produtosEn, cliente, 0, frete, prazo, data) );
            } 
            
          } while (lendo != null); 
        }
      }
    }


    //Métodos responsaveis por realizar os SETs
    private void GuardarProdutos(){
      System.IO.File.Delete(fileProdutos);
      using (StreamWriter Salvar = File.AppendText(fileProdutos)) { 
        foreach(Produto p in produtos){
          Salvar.WriteLine("--"); 
          Salvar.WriteLine(p.getTipo()); 
          Salvar.WriteLine(p.getQuantidade()); 
          Salvar.WriteLine(p.getCusto()); 
          Salvar.WriteLine(p.getPeso()); 
          Salvar.WriteLine(p.getVolume()); 
        }
      }
    }

    private void GuardarClientes(){
      using (StreamWriter Salvar = File.AppendText(fileClientes)) { 
        double[] coord = new double[2];        
        coord = clientes.getCoord();
        Salvar.WriteLine("--"); 
        Salvar.WriteLine(clientes.getId()); 
        Salvar.WriteLine(clientes.getNome()); 
        Salvar.WriteLine(coord[0]); 
        Salvar.WriteLine(coord[1]); 
        Salvar.WriteLine(clientes.getTendencia()); 
        foreach(Produto p in pedidos){
          Salvar.WriteLine("-#"); 
          Salvar.WriteLine(p.getTipo()); 
          Salvar.WriteLine(p.getQuantidade()); 
          Salvar.WriteLine(p.getCusto()); 
          Salvar.WriteLine(p.getPeso()); 
          Salvar.WriteLine(p.getVolume()); 
        }      
      }
    }

    private void GuardarEncomendas(){
      System.IO.File.Delete(fileEncomendas);
      using (StreamWriter Salvar = File.AppendText(fileEncomendas)) { 
        foreach(Encomenda e in encomendas){
          Salvar.WriteLine("--"); 
          Salvar.WriteLine(e.getId());  
          Salvar.WriteLine(e.getPreco());
          Salvar.WriteLine(e.getCliente());
          Salvar.WriteLine(e.getFrete());
          Salvar.WriteLine(e.getPrazo());
          Salvar.WriteLine(e.getDataCompra());
          if(e.getStatusEntrega())
            { Salvar.WriteLine("true"); }
          else { Salvar.WriteLine("false"); }
          foreach(Produto p in e.getPacote()){
            Salvar.WriteLine("-#"); 
            Salvar.WriteLine(p.getTipo()); 
            Salvar.WriteLine(p.getQuantidade()); 
            Salvar.WriteLine(p.getCusto()); 
            Salvar.WriteLine(p.getPeso()); 
            Salvar.WriteLine(p.getVolume()); 
          }
        }
      }
    }

  }
}
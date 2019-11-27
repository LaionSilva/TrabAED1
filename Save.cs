
using System;
using System.Collections.Generic;
using System.IO;

namespace logistica {
  public class Save : Estoque {
    //private List<Caminhao> frota = new List<Caminhao>(); 
	  private Cliente cli = new Cliente(); 

    public Save() { CheckArquivo(); }

    //GETs das Listas
    public List<Produto> getProdutos() {
      Carregar("produtos");
      return produtos;
    }

    public List<Cliente> getClientes(){
      Carregar("clientes");
      return clientes;
    }

    public List<Encomenda> getEncomendas(){
      Carregar("encomendas");
      return encomendas;
    }

    public List<Encomenda> getEntregas(){
      Carregar("entregas");
      return entregas;
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
          cli = new Cliente(cl.getId(), cl.getNome(), coord[0], coord[1], cl.getTendencia());
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

    public void setEntregas(List<Encomenda> e) {
      entregas = e;
      Guardar("entregas");
    }

    public void setRelatorio(DadosLog dados) {
      System.IO.File.Delete(fileRelatorio);
      GuardarRelatorio(dados);
    }
   

    private void Carregar(string dado){ //  Get do dados.txt para todas as listas
    System.Threading.Thread.Sleep(50);
      switch(dado){        
        case "produtos": CarregarProdutos(); break;
        case "clientes": CarregarClientes(); break;
        case "encomendas": CarregarEncomendas(); break;
        case "entregas": CarregarEntregas(); break;
      }
      System.Threading.Thread.Sleep(50);
    }
    
    public void Guardar(string dado){ //  Set no dados.txt os dados de todas as listas 
      System.Threading.Thread.Sleep(50);
      switch(dado){
        case "produtos": GuardarProdutos(); break;
        case "clientes": GuardarClientes(); break;
        case "encomendas": GuardarEncomendas(); break;
        case "entregas": GuardarEntregas(); break;
      }
      System.Threading.Thread.Sleep(50);
    }

    public void Reset() { //  Apagar todos os dados de todos os arquivos de armazenamento
      try{
        System.Threading.Thread.Sleep(50);
        System.IO.File.Delete(fileProdutos);
        System.IO.File.Delete(fileEncomendas);
        System.IO.File.Delete(fileEntregas);
        System.IO.File.Delete(fileClientes);
        CheckArquivo();
        Console.WriteLine("Dados de armazenamento apagados");
      } catch {  Console.WriteLine("Erro: Reset - sav:res");}
    }

    private void CheckArquivo(){ //  Verifica a existencia de um arquivo.txt, caso não exista é criado um
      System.Threading.Thread.Sleep(50);
      if (!System.IO.File.Exists(fileProdutos))
        { using (StreamWriter Salvar = File.AppendText(fileProdutos)) {} }
      if (!System.IO.File.Exists(fileClientes))
        { using (StreamWriter Salvar = File.AppendText(fileClientes)) {} }
      if (!System.IO.File.Exists(fileEncomendas))
        { using (StreamWriter Salvar = File.AppendText(fileEncomendas)) {} }
      if (!System.IO.File.Exists(fileEntregas))
        { using (StreamWriter Salvar = File.AppendText(fileEntregas)) {} }
      if (!System.IO.File.Exists(fileRelatorio))
        { using (StreamWriter Salvar = File.AppendText(fileRelatorio)) {} }
      System.Threading.Thread.Sleep(50);
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
              cli = new Cliente(id, nome, coord[0], coord[1], tendencia);

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
                { cli.setPedidos(pedidos); }
              clientes.Add( cli );    
            }
            else if(gravar) 
              { clientes.Add( cli ); }

            gravar = false; 
            //Console.WriteLine(Clientes.Count + lendo); //  TESTE   
          } while (lendo != null); 

        }
      }
    }

    private void CarregarEncomendas(){
      string lendo = "", data = "";
      int id = 0, prazo = 0, cliente = 0;
      double frete = 0;  
      using(Stream FileIn = File.Open(fileEncomendas, FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          lendo = Carregar.ReadLine(); 
          do{
            List<Produto> produtosEn = new List<Produto>();
            if(lendo == "--") {
              id = int.Parse(Carregar.ReadLine());
              cliente = int.Parse(Carregar.ReadLine());
              frete = double.Parse(Carregar.ReadLine());
              prazo = int.Parse(Carregar.ReadLine());
              data = Carregar.ReadLine();
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

    private void CarregarEntregas(){
      string lendo = "", data = "";
      int id = 0, prazo = 0, cliente = 0;
      double frete = 0;   
      using(Stream FileIn = File.Open(fileEntregas, FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          lendo = Carregar.ReadLine(); 
          do{
            List<Produto> produtosEn = new List<Produto>();
            if(lendo == "--") {
              id = int.Parse(Carregar.ReadLine());
              cliente = int.Parse(Carregar.ReadLine());
              frete = double.Parse(Carregar.ReadLine());
              prazo = int.Parse(Carregar.ReadLine());
              data = Carregar.ReadLine();
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
              entregas.Add( new Encomenda(id, produtosEn, cliente, 0, frete, prazo, data) );
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
        coord = cli.getCoord();
        Salvar.WriteLine("--"); 
        Salvar.WriteLine(cli.getId()); 
        Salvar.WriteLine(cli.getNome()); 
        Salvar.WriteLine(coord[0]); 
        Salvar.WriteLine(coord[1]); 
        Salvar.WriteLine(cli.getTendencia()); 
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
          Salvar.WriteLine(e.getCliente());
          Salvar.WriteLine(e.getFrete());
          Salvar.WriteLine(e.getPrazo());
          Salvar.WriteLine(e.getDataCompra());
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

    private void GuardarEntregas(){
      using (StreamWriter Salvar = File.AppendText(fileEntregas)) { 
        foreach(Encomenda e in entregas){
          Salvar.WriteLine("--"); 
          //Salvar.WriteLine(e.getId());  
          Salvar.WriteLine(e.getCliente());
          Salvar.WriteLine(e.getFrete());
          Salvar.WriteLine(e.getPrazo());
          Salvar.WriteLine(e.getDataCompra());
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

    private void GuardarRelatorio(DadosLog dados){
      using (StreamWriter Salvar = File.AppendText(fileRelatorio)) {
        foreach(string r in dados.relatorio) {
          Salvar.Write(r); 
        }
        foreach(string m in dados.mapa) {
          Salvar.Write(m); 
        }
      }  
    }

  }
}
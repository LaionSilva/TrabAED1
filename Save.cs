
using System;
using System.Collections.Generic;
using System.IO;

namespace logistica {
  public class Save : Estoque {
    //private List<Caminhao> frota = new List<Caminhao>(); 
	  private Cliente cli = new Cliente(); 

    public Save() { CheckArquivo(); }

    //GETs do Estoque
    public List<Produto> getProdutos() {
      System.Threading.Thread.Sleep(5);
      CarregarProdutos();
      return produtos;
    }

    public List<Cliente> getClientes() {
      System.Threading.Thread.Sleep(5);
      CarregarClientes();
      return clientes;
    }

    public List<Encomenda> getEncomendas() {
      System.Threading.Thread.Sleep(5);
      CarregarEncomendas();
      return encomendas;
    }

    public List<Encomenda> getEntregas() {
      System.Threading.Thread.Sleep(5);
      CarregarEntregas();
      return entregas;
    }

     public string getSenhas() {
      System.Threading.Thread.Sleep(5);
      return CarregarSenhas();
    }
    

    //SETs do Estoque
    public void setProdutos(List<Produto> p) {
      System.IO.File.Delete(fileProdutos);
      produtos = p;
      System.Threading.Thread.Sleep(5);
      GuardarProdutos();
    }

    public void setClientes(List<Cliente> c) {
      double[] coord = new double[2];
      if(c.Count > 0){
        System.IO.File.Delete(fileClientes);
        CheckArquivo();
        System.Threading.Thread.Sleep(5);
        try {
          foreach(Cliente cl in c) {
            coord = cl.getCoord();
            cli = new Cliente(cl.getId(), cl.getNome(), coord[0], coord[1], cl.getTendencia());
            pedidos.Clear();
            pedidos.AddRange(cl.getPedidos());
            System.Threading.Thread.Sleep(1);
            GuardarClientes();
          }  
        } 
        catch (Exception e) {
          LogisticaException.ExceptionGrave("LE_Save", e, "Save", "setClientes");
        }
      }        
    }    

    public void setEncomendas(List<Encomenda> e) {
      System.IO.File.Delete(fileEncomendas);
      encomendas = e;
      System.Threading.Thread.Sleep(5);
      GuardarEncomendas();
    }

    public void setEntregas(List<Encomenda> e) {
      System.IO.File.Delete(fileEntregas);
      entregas = e;
      System.Threading.Thread.Sleep(5);
      GuardarEntregas();
    }

    public void setRelatorio(DadosLog dados) {
      System.IO.File.Delete(fileRelatorio);
      System.Threading.Thread.Sleep(5);
      GuardarRelatorio(dados);
    }

    public void setLogException(DadosLogException e) {
      System.Threading.Thread.Sleep(5);
      GuardarLogException(e);
    }

    public void Reset() { //  Apagar todos os dados de todos os arquivos de armazenamento
      try {
        System.Threading.Thread.Sleep(5);
        System.IO.File.Delete(fileProdutos);
        System.IO.File.Delete(fileEncomendas);
        System.IO.File.Delete(fileEntregas);
        System.IO.File.Delete(fileClientes);
        System.IO.File.Delete(fileRelatorio);
        CheckArquivo();
        Console.WriteLine("\nDados salvos apagados\n");
      } 
      catch(FileNotFoundException) {} //  Nada a ser feito, o fluxo de dados se corrigirá sozinho 
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "Reset");
      }
      finally {
        CheckArquivo();
        System.Threading.Thread.Sleep(5);
        System.IO.File.Delete(fileProdutos);
        System.IO.File.Delete(fileEncomendas);
        System.IO.File.Delete(fileEntregas);
        System.IO.File.Delete(fileClientes);
        System.IO.File.Delete(fileRelatorio);
        CheckArquivo();
        Console.WriteLine("\nDados salvos apagados\n");
      }
    }

    private void CheckArquivo() { //  Verifica a existencia de um arquivo.txt, caso não exista é criado um
      int posicao = 1;
      System.Threading.Thread.Sleep(5);
      if (!System.IO.File.Exists(fileProdutos))
        { using (StreamWriter Salvar = File.AppendText(fileProdutos)) {} posicao++; }
      if (!System.IO.File.Exists(fileClientes))
        { using (StreamWriter Salvar = File.AppendText(fileClientes)) {} posicao++; }
      if (!System.IO.File.Exists(fileEncomendas))
        { using (StreamWriter Salvar = File.AppendText(fileEncomendas)) {} posicao++; }
      if (!System.IO.File.Exists(fileEntregas))
        { using (StreamWriter Salvar = File.AppendText(fileEntregas)) {} posicao++; }
      if (!System.IO.File.Exists(fileRelatorio))
        { using (StreamWriter Salvar = File.AppendText(fileRelatorio)) {} posicao++; }
      if (!System.IO.File.Exists(fileSenhas))
        { using (StreamWriter Salvar = File.AppendText(fileSenhas)) {} posicao++; }
      System.Threading.Thread.Sleep(5);
    }


    //Métodos responsaveis por realizar os GETs
    private void CarregarProdutos(){
      string lendo = "";
      try {
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
      catch(FileNotFoundException) {} //  Nada a ser feito, o fluxo de dados se corrigirá sozinho 
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "CarregarProdutos");
      }
    }

    private void CarregarClientes(){
      string lendo = "";
      double[] coord = new double[2];
      int id = 0, tendencia = 0;
      string nome = "";
      bool gravar = false;

       try {
        using(Stream FileIn = File.Open(fileClientes, FileMode.Open)) {
          using(StreamReader Carregar = new StreamReader(FileIn)) {
            lendo = Carregar.ReadLine();
            do {
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
      catch(FileNotFoundException) {} //  Nada a ser feito, o fluxo de dados se corrigirá sozinho 
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "CarregarClientes");
      }
    }

    private void CarregarEncomendas(){
      string lendo = "", data = "";
      int id = 0, prazo = 0, cliente = 0;
      double frete = 0;  

      try {
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
      catch(FileNotFoundException) {} //  Nada a ser feito, o fluxo de dados se corrigirá sozinho 
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "CarregarEncomendas");
      }
    }

    private void CarregarEntregas(){
      string lendo = "", data = "";
      int id = 0, prazo = 0, cliente = 0;
      double frete = 0; 

      try {   
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
      catch(FileNotFoundException) {} //  Nada a ser feito, o fluxo de dados se corrigirá sozinho 
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "CarregarEntregas");
      }
    }

    private string CarregarSenhas(){
      string senha = "";
      try {
        using(Stream FileIn = File.Open(fileSenhas, FileMode.Open)){
          using(StreamReader Carregar = new StreamReader(FileIn)){
            senha = Carregar.ReadLine();
          }
        }
      }
      catch(NullReferenceException) { LogisticaException.ExceptionGrave("LE_Save_SenhaNaoEncontrada"); }
      catch(LogisticaException) {} 
      catch(FileNotFoundException) {} //  Nada a ser feito, o fluxo de dados se corrigirá sozinho 
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "CarregarProdutos");
      }

      return senha;
    }


    //Métodos responsaveis por realizar os SETs
    private void GuardarProdutos() {
      try {
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
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "GuardarProdutos");
      }
    }

    private void GuardarClientes() {
      try {
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
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "GuardarClientes");
      }
    }

    private void GuardarEncomendas(){
      try {
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
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "GuardarEncomendas");
      }
    }

    private void GuardarEntregas(){
      try {
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
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "GuardarEntregas");
      }
    }

    private void GuardarRelatorio(DadosLog dados){
      try {
        using (StreamWriter Salvar = File.AppendText(fileRelatorio)) {
          foreach(string r in dados.relatorio) {
            Salvar.Write(r); 
          }
          foreach(string m in dados.mapa) {
            Salvar.Write(m); 
          }
        }
      }
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save", e, "Save", "GuardarRelatorio");
      }  
    }

    private void GuardarLogException(DadosLogException newException) {
      try {
        using (StreamWriter Salvar = File.AppendText(fileLogException)) {
          Salvar.WriteLine("Exceptions inesperada: "); 
          Salvar.Write("Classe: ");
          Salvar.WriteLine(newException.classe); 
          Salvar.Write("Método: "); 
          Salvar.WriteLine(newException.metodo); 
          Salvar.Write("Instante: "); 
          Salvar.WriteLine(newException.data); 
          Salvar.Write("Nota do usuário: "); 
          Salvar.WriteLine(newException.nota); 
          if(newException.notaAdm.Length > 0) { Salvar.WriteLine(newException.notaAdm); }
          Salvar.Write("Mensagem:"); 
          Salvar.WriteLine(newException.mensagem); 
          Salvar.Write("\n\n"); 
        } 
      }
      catch (Exception e) {
        LogisticaException.ExceptionGrave("LE_Save_Exception", e, "Save", "GuardarLogException");
      } //IO Exception
    }

  }
}
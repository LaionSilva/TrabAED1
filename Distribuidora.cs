using System;
using System.Collections.Generic;
using System.Linq; //Para calcular o menor ou maior valor de uma lista

namespace logistica {
  public class Distribuidora { 
    private double[] coord = new double[2]; //  lat, long
    private double coefLucro;
    private double carteira; //  total de verbas
    private bool CarteiraInf = true;
    private int capacidade;
    private List<Produto> produtos = new List<Produto>(); 
    private List<Encomenda> encomendas = new List<Encomenda>();
    private List<Encomenda> entregas = new List<Encomenda>();
    private List<Relatorio> diarioEntregas = new List<Relatorio>(); 
	  private List<Cliente> clientes = new List<Cliente>(); 
    public Logistica rota = new Logistica();
    private Caminhao veiculo = new Caminhao(); 
    public Save file = new Save();
    
    public Distribuidora(double lat = 0, double lon = 0, int i = 10000, double c = 0, double l = 0.4) {
      coord[0] = lat;
      coord[1] = lon;
      capacidade = i;
      carteira = c;
      coefLucro = l;

      Carregar();      
      Salvar();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  ARMAZENAMENTO DE DADOS
    public void Salvar() { //  Guardar listas em dados.txt
      file.setProdutos(produtos); 
      file.setClientes(clientes);
      file.setEncomendas(encomendas);  
    }

    public void Carregar() { //  Carregar listas de dados.txt
      produtos = file.getProdutos(); 
      clientes = file.getClientes(); 
      encomendas = file.getEncomendas();      
    }
    //  ARMAZENAMENTO DE DADOS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  PRODUTOS 
    public void NovoProduto(string tipo, double custo, double peso, double volume) { 
      //  Produto(tipo, quantidade, preço por unidade)  
      try{
        produtos.Add(new Produto(tipo, 0, custo, peso, volume));
        Salvar();
      } catch { Console.WriteLine ("Erro: Novo Produto - dis:NPr"); }         
    }

    public bool ComprarProduto(string tipo, int n) {
      try{
        bool prod = false, renda = false;
        foreach(Produto p in produtos){
          if(tipo == p.getTipo()) {
            if((carteira >= n * p.getCusto()) || CarteiraInf){
              p.upQuant(n);
              if(!CarteiraInf) { carteira -= n * p.getCusto(); }
              renda = true;
            } else { Console.WriteLine("Não há renda suficiente: dis-ComPro"); }       
            prod = true;
            break;             
          }
        }
        if(renda && prod) { 
          Console.WriteLine("Estoque reabastecido"); 
          Salvar(); return true; 
        }
        else if (!prod) { Console.WriteLine("Produto não encontrado"); return false; }
        else { return false; }
      } catch { Console.WriteLine ("Erro: Comprar Produto - dis:CPr"); return false;}      
    }

    private bool DownEstoque(List<Produto> pacote) { // abater da Lista<produtos> o que foi vendido
      try{
        for(int i = 0; i < pacote.Count; i++) {
          foreach(Produto e in produtos) {
            if(pacote[i].getTipo() == e.getTipo()) 
              { e.downQuant( pacote[i].getQuantidade() ); }
          }
        }
        Salvar();
      } catch { return false; }
      return true;
    }
    //  PRODUTOS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  CLIENTES
    public void NovoCliente(string nome) { //  Cadastrar novo cliente para a distribuidora - localização aleatória
      try{
        List<int> ids = new List<int>();
        ids.Add(0);
        foreach(Cliente c in clientes) {
          ids.Add(c.getId());
        }
        Random rand = new Random();
        clientes.Add( new Cliente(ids.Max() + 1, nome, rand.Next(-4000,4000) / 100, rand.Next(-8000,8000) / 100) );
        coord = clientes[getClientes(nome)].getCoord();
        Console.WriteLine("Novo cliente: {0} - lat: {1}, lon: {2}", nome, coord[0], coord[1]);
        Salvar();
      } catch { Console.WriteLine ("Erro: Novo Cliente - dis:NCl"); }       
    }

    public void Ofertar() { //  Oferecer ao cliente os produtos
      try{
        bool compra = false;
        foreach(Cliente c in clientes) {
          int oferta = c.Ofertar(produtos);
          if(oferta == 0) 
            { Console.WriteLine("Erro ao ofertar produtos: dis-ofe"); }
          else if (oferta == 2) 
            { compra = true; }
        }
        if(compra) { 
          Console.WriteLine("Pedido realizado"); 
          Salvar();
        }
      } catch { Console.WriteLine ("Erro: Ofertar - dis:ofe"); }     
    }

    public void Vender() { //  Aceitar pedidos realizados pelos clientes
      try{
        List<Produto> pacote = new List<Produto>();
        List<int> ids = new List<int>();
        bool venda = false;
        foreach(Cliente c in clientes) {
          pacote.Clear();
          pacote.AddRange(c.Vender(produtos));
          if(pacote.Count > 0) {
            double[] coord = new double[2];
            coord = c.getCoord();
            double frete = calcularDistancia(0, coord[0], 0, coord[1]) * 0.60;
            int dist = (int)(calcularDistancia(0, coord[0], 0, coord[1]) * 0.60);
            ids.Clear();
            ids.Add(0);
            foreach(Encomenda e in encomendas) {
              ids.Add(e.getId());
            }
            encomendas.Add( new Encomenda(ids.Max() + 1, pacote, getClientes(c.getNome()), coefLucro, frete, dist) );  
            DownEstoque(pacote); // abater da Lista<produtos> o que foi vendido
            venda = true;
          }
        }
        if(venda) { 
          Console.WriteLine("Encomenda preparada"); 
          OrganizarEncomendas();
          Salvar();
        }
      } catch { Console.WriteLine ("Erro: Vender - dis:ven"); }      
    }

    public int getClientes(string nome){ //  Encontrar o id de algum cliente pelo nome - retorna -1 caso não encontre
      for(int i = 0; i < clientes.Count; i++){
        if(nome == clientes[i].getNome()) { return i; }
      } return -1;
    }
    //  CLIENTES
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  ENCOMENDAS
    private void OrganizarEncomendas(){
      try{
        int[] auxIndex = new int[encomendas.Count];
        List<double> coefMon = new List<double>(); 
        List<Encomenda> auxEnc = new List<Encomenda>();
        for(int i = 0; i < encomendas.Count; i++)
          { coefMon.Add( ((encomendas[i].getPreco() / encomendas[i].getPeso()) / (encomendas[i].getVolume())) * 1000 ); }
        for(int i = 0; i < encomendas.Count; i++) {
          auxIndex[i] = coefMon.IndexOf(coefMon.Max());
          coefMon[ coefMon.IndexOf(coefMon.Max()) ] = -1;
        }
        for(int i = 0; i < encomendas.Count; i++)
          { auxEnc.Add( encomendas[ auxIndex[i] ] ); }
        encomendas.Clear();
        encomendas.AddRange(auxEnc);  
      } catch { Console.WriteLine ("Erro: Organizar Encomendas - dis:OEn"); }
    }
    //  ENCOMENDAS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  LOGÍSTICA
    public void ComoViajar(){
      Console.WriteLine ("\nVIAGEM");
      List<string> nomeClientes = new List<string>();//  Nome dos clientes atendidos
      List<Cliente> osClientes = new List<Cliente>(); //  Lista com os Clientes atendidos
      List<double> precoPorCliente = new List<double>(); //  Somatorio dos preços das encomendas por cliente 
      List<int> idCli = new List<int>();

      double[] coordCliente = new double[2];
      double lat1, lon1, lat2, lon2; //  auxiliar
      double precoTotal = 0; //  Valor bruto que vamos receber
      
      DadosLog dados = new DadosLog();    

      for(int i = 0; i < encomendas.Count; i++) {
        if(idCli.IndexOf( encomendas[i].getCliente() ) == -1) {
          idCli.Add( encomendas[i].getCliente() );
          foreach(Cliente c in clientes) {
            if(c.getId() == encomendas[i].getCliente()) {
              nomeClientes.Add(c.getNome());
              osClientes.Add(c);
            }
          }          
        }
        if(idCli.Count >= 8) { break; }
      }

      for(int i = 0; i < precoPorCliente.Count; i++) { 
        precoTotal += precoPorCliente[i];
      } 

      //  função Laion(nomeClientes) ==> int[nome.Cont] = rota.MelhorRota(nomes (List<string>), clientes, nome.Count);
      Console.WriteLine("Dentro de disribuidora, antes de passar para logistica {0}", encomendas.Count); //  TESTE
      Console.WriteLine("Dentro de disribuidora, antes de passar para logistica {0}", nomeClientes.Count); //  TESTE
      dados = rota.MelhorRota(nomeClientes, osClientes, nomeClientes.Count);

      dados.custo = (dados.distancia / veiculo.getEficiencia())*3.686 + veiculo.calcularDiariaMotorista(dados.distancia); //  preço do diesel 3,686
      dados.lucro = (precoTotal/1.4) - dados.custo;
      Console.WriteLine ("\ndistancia: {0}km\ncusto: R$:{1}\nlucro: R$:{2}", dados.distancia, dados.distancia, dados.lucro);
      NovoRelatorioEntrega(dados, osClientes);
    }

    private void NovoRelatorioEntrega(DadosLog dados, List<Cliente> CliEntrega){
      try{
        List<int> ids = new List<int>();
        List<int> idClientes = new List<int>();
        List<int> pacote = new List<int>();
        List<Encomenda> auxEnc = new List<Encomenda>();
        int[] indices = new int[dados.rota.Length];

        ids.Add(0);
        foreach(Relatorio r in diarioEntregas) {
          ids.Add(r.getId());
        }
        for(int i = 0; i < dados.rota.Length; i++) {
          idClientes.Add( CliEntrega[ dados.rota[i] ].getId() );
        }
        foreach(Encomenda e in encomendas) {          
          if(idClientes.IndexOf(e.getCliente()) != -1 ){
            pacote.Add(e.getId());
            auxEnc.Add(e); Console.WriteLine ("relatorio ok");
          }
        }
        foreach(Encomenda e in auxEnc) {
          encomendas.Remove(e);
        }
        Salvar();
        
        diarioEntregas.Add( new Relatorio(ids.Max() + 1, idClientes, pacote, dados.distancia, dados.custo, dados.lucro) );
        Console.WriteLine ("Novo relatório de entrega gerado. id: {0}", ids.Max() + 1);
      } catch { Console.WriteLine ("Erro: Novo Relatório de Entrega - dis:NRE"); }        
    }
    
    private double calcularDistancia(double lat1, double lon1, double lat2, double lon2){
      double x1 = lat1*111.12;
      double x2 = lat2*111.12;
      double y1 = lon1*111.12 * ( (50 - Math.Sqrt( Math.Pow(lat1, 2) )) / 50 );
      double y2 = lon2*111.12 * ( (50 - Math.Sqrt( Math.Pow(lat2, 2) )) / 50 );

      return Math.Sqrt( Math.Pow(x1 - y1, 2) + Math.Pow(x2 - y2, 2));
    }
    // LOGÍSTICA
    ////////////////////////////////////////////////////////////////////////////////////////////////////////    
  }
}
using System;
using System.Collections.Generic;
using System.Linq; //Para calcular o menor ou maior valor de uma lista

namespace logistica {
  public class Distribuidora { 
    private double[] coord = new double[2]; //  lat, long
    private int capacidade;
    private double coefLucro;
    private double carteira; //  total de verbas
    private bool CarteiraInf = true;
    private double combustivel;
    private List<Produto> produtos = new List<Produto>(); 
    private List<Encomenda> encomendas = new List<Encomenda>();
    private List<Relatorio> diarioEmpresarial = new List<Relatorio>(); 
    private List<Caminhao> frota = new List<Caminhao>(); 
	  private List<Cliente> clientes = new List<Cliente>(); 
    public Save file = new Save();
    public Logistica rota = new Logistica();
    
    public Distribuidora(double lat = 0, double lon = 0, int i = 10000, double c = 0, double l = 0.4) {
      coord[0] = lat;
      coord[1] = lon;
      capacidade = i;
      carteira = c;
      coefLucro = l;
      combustivel = 0;

      Carregar();    
      //InicializarProdutos();  
      Salvar();
    }

    public void Salvar() { //  Guardar listas em dados.txt
      try{
        file.setProdutos(produtos); 
        file.setClientes(clientes);
        file.setEncomendas(encomendas);
      } catch { Console.WriteLine ("Erro: Salvar - dist:sal"); }      
    }

    public void Carregar() { //  Carregar listas de dados.txt
      try{
        produtos = file.getProdutos(); 
        clientes = file.getClientes(); 
        encomendas = file.getEncomendas(); 
      } catch { Console.WriteLine ("Erro: Carregar - dis:car"); }      
    }

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
    
    public void NovoCliente(string nome) { //  Cadastrar novo cliente para a distribuidora - localização aleatória
      try{
        Random rand = new Random();
        clientes.Add( new Cliente(1, nome, rand.Next(-4000,4000) / 100, rand.Next(-8000,8000) / 100) );
        coord = clientes[getClientes(nome)].getCoord();
        Console.WriteLine("Novo cliente: {0} - lat: {1}, lon: {2}", nome, coord[0], coord[1]);
        Salvar();
      } catch { Console.WriteLine ("Erro: Novo Cliente - dis:NCl"); }       
    }

    public void NovoCaminhao(double mc = 150, double kg = 3000, double ef = 10) { //  Comprar novo caminão
      try{
        int id = 1;
        frota.Add( new Caminhao(id, ef, mc, kg) );
        Console.WriteLine("Novo caminão: id: {0} - espaço: {1}m3 - carga máx: {2}kg - eficiência: {3}km/L", id, mc, kg, ef);
      } catch { Console.WriteLine ("Erro: Novo Caminhão - dis:NCa"); }      
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
        bool venda = false;
        foreach(Cliente c in clientes) {
          pacote = c.Vender(produtos);
          if(pacote.Count > 0) {
            double[] coord = new double[2];
            coord = c.getCoord();
            double frete = calcularDistancia(0, coord[0], 0, coord[1]) * 0.60;
            int dist = (int)(calcularDistancia(0, coord[0], 0, coord[1]) * 0.60);
            encomendas.Add( new Encomenda(1, pacote, c.getNome(), coefLucro, frete, dist) );  
            DownEstoque(pacote); // abater da Lista<produtos> o que foi vendido
            venda = true;
          }
        }
        if(venda) { 
          Console.WriteLine("Encomenda preparada"); 
          Salvar();
        }
      } catch { Console.WriteLine ("Erro: Vender - dis:ven"); }      
    }

    private void OrganizarEncomendas(){
      //try{
        List<Encomenda> aux = encomendas;
        List<int> coefMon = new List<int>(); 
        foreach(Encomenda e in aux)
          { coefMon.Add( (int)((e.getPreco() / e.getPeso()) / (e.getVolume() * 1000000)) ); }
        aux.Clear();
        int range =coefMon.Count;
        for(int i = 0; i < range; i++){
          aux.Add(encomendas[ coefMon.IndexOf(coefMon.Max()) ]);
          coefMon.RemoveAt( coefMon.IndexOf(coefMon.Max()) );
        }
        encomendas.Clear();
        encomendas = aux;
        Salvar();
        
      //} catch { Console.WriteLine ("Erro: Organizar Encomendas - dis:OEn"); }
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

    public int getClientes(string nome){ //  Encontrar o id de algum cliente pelo nome - retorna -1 caso não encontre
      for(int i = 0; i < clientes.Count; i++){
        if(nome == clientes[i].getNome()) { return i; }
      }
      return -1;
    }

    private void comoViajar(){
      List<string> nomeClientes = new List<string>();// Nome dos clientes atendidos
      List<Cliente> osClientes = new List<Cliente>(); // Lista com os Clientes atendidos
      List<double> precoPorCliente = new List<double>(); // Somatorio dos preços das encomendas por cliente 

      double[] coordCliente = new double[2];
      double lat1, lon1, lat2, lon2; // auxiliar
      double distanciaTotal = 0;
      double lucro = 0;
      double precoTotal = 0; // Valor bruto que vamos receber
      double custoTotal;  
      string nome; // auxiliar
      int index = 0; // auxiliar

      for(int i=0; i<encomendas.Count; i++) { // rodar até a quantidade de encomendas
        nome = encomendas[i].getCliente(); // pega o nome do cliente
        index = nomeClientes.IndexOf(nome);
        if(index == -1) {// IndexOf(elemento) retorna -1 se o elemento não exite na lista
          nomeClientes.Add(nome);
          List<string> idNome = new List<string>();
          foreach(Cliente c in clientes)
            { idNome.Add(c.getNome()); }

          osClientes.Add(clientes[idNome.IndexOf(nome)]);
          precoPorCliente.Add(encomendas[i].getPreco());
          //encomendas.Remove(encomendas[i]);
        } else {
          precoPorCliente[index]+= encomendas[i].getPreco();
          //encomendas.Remove(encomendas[i]);
        }
        if(nomeClientes.Count >= 8) {
          break;
        }
      }
      
      for(int i=0; i<precoPorCliente.Count; i++){ 
        precoTotal += precoPorCliente[i];
      } 

      int[] indices = new int[nomeClientes.Count];
      // função Laion(nomeClientes) ==> int[nome.Cont] = rota.MelhorRota(nomes (List<string>), clientes, nome.Count);
      indices = rota.MelhorRota(nomeClientes, osClientes, nomeClientes.Count);

      int h=0; // auxiliar
      while(h<osClientes.Count){ // Calcular a distancia total
        coordCliente = osClientes[indices[h]].getCoord();
        lat1 = coordCliente[0];
        lon1 = coordCliente[1];

        if(0 == h){ // Calcular do armazem até o primeiro cliente
          lat2 = coord[0];
          lon2 = coord[1];
          distanciaTotal += calcularDistancia(lat1, lon1, lat2, lon2);
        }

        h++;
        coord = osClientes[indices[h]].getCoord();
        lat2 = coordCliente[0];
        lon2 = coordCliente[1];
        distanciaTotal += calcularDistancia(lat1, lon1, lat2, lon2);
        if(osClientes.Count == h){ // Calcular do ultimo cliente até o armazem
          lat1 = coord[0];
          lon1 = coord[1];
          distanciaTotal += calcularDistancia(lat1, lon1, lat2, lon2);
        }
      }
      // preço do diesel 3,686
      custoTotal = (distanciaTotal / frota[0].getEficiencia())*3.686 + frota[0].calcularDiariaMotorista(distanciaTotal);
      lucro = precoTotal - custoTotal;
    } 
    
    private double calcularDistancia(double lat1,double lon1, double lat2, double lon2){
      double x1 = lat1*111.12;
      double x2 = lat2*111.12;
      double y1 = lon1*111.12 * ( (50 - Math.Sqrt( Math.Pow(lat1, 2) )) / 50 );
      double y2 = lon2*111.12 * ( (50 - Math.Sqrt( Math.Pow(lat2, 2) )) / 50 );

      return Math.Sqrt( Math.Pow(x1 - y1, 2) + Math.Pow(x2 - y2, 2));
    }
    
  }
}
using System;
using System.Collections.Generic;
using System.Linq; //Para calcular o menor ou maior valor de uma lista

namespace logistica {
  public class Distribuidora { 
    private double[] coord = new double[2]; //  lat, long
    private int capacidade;
    private double coefLucro;
    private double carteira; //  total de verbas
    private double combustivel;
    private List<Produto> produtos = new List<Produto>(); 
    private List<Encomenda> encomendas = new List<Encomenda>();
    private List<Relatorio> diarioEmpresarial = new List<Relatorio>(); 
    private List<Caminhao> frota = new List<Caminhao>(); 
	  private List<Cliente> clientes = new List<Cliente>(); 
    public Save file = new Save();
    
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
      file.setProdutos(produtos); 
      file.setClientes(clientes);
    }
    public void Carregar() { //  Carregar listas de dados.txt
      produtos.AddRange(file.getProdutos()); 
      clientes.AddRange(file.getClientes()); 
    }

    protected void InicializarProdutos() { 
      //  Produto(tipo, quantidade, preço por unidade)
      produtos.Add(new Produto("frutas", 100, 300.55));

      produtos.Add(new Produto("eletrônicos", 100, 646.32));

      produtos.Add(new Produto("katanas", 100, 14555.47));
    }
    
    public void NovoCliente(string nome) { //  Cadastrar novo cliente para a distribuidora - localização aleatória
       Random rand = new Random();
       clientes.Add( new Cliente(1, nome, rand.Next(-4000,4000) / 100, rand.Next(-8000,8000) / 100) );
       coord = clientes[getClientes(nome)].getCoord();
       Console.WriteLine("Novo cliente: {0} - lat: {1}, lon: {2}", nome, coord[0], coord[1]);
    }

    public void NovoCaminhao(double mc = 150, double kg = 3000, double ef = 10) { //  Comprar novo caminão
      int id = 1;
      frota.Add( new Caminhao(id, ef, mc, kg) );
      Console.WriteLine("Novo caminão: id: {0} - espaço: {1}m3 - carga máx: {2}kg - eficiência: {3}km/L", id, mc, kg, ef);
    }

    private void Abastecer() {} //  TODO

    public void Ofertar() { //  Oferecer ao cliente os produtos
      bool compra = false;
      foreach(Cliente c in clientes) {
        int oferta = c.Ofertar(produtos);
        if(oferta == 0) 
          { Console.WriteLine("Erro ao ofertar produtos: dis-ofe"); }
        else if (oferta == 2) 
          { compra = true; }
      }
      if(compra) { Console.WriteLine("Pedido realizado"); }
    }

    public void Vender() { //  Aceitar pedidos realizados pelos clientes
      List<Produto> pacote = new List<Produto>();
      bool venda = false;
      foreach(Cliente c in clientes) {
        pacote = c.Vender(produtos);
        if(pacote.Count > 0) {
          double[] coord = new double[2];
          coord = c.getCoord();
          Destino destino = new Destino(c.getNome(), coord[0], coord[1], 0);
          encomendas.Add( new Encomenda(1, pacote, destino, 0) );
          DownEstoque(pacote); // abater da Lista<produtos> o que foi vendido
          venda = true;
        }
      }
      if(venda) { Console.WriteLine("Encomenda preparada"); }
    }

    private bool DownEstoque(List<Produto> pacote) { // abater da Lista<produtos> o que foi vendido
      try{
        for(int i = 0; i < pacote.Count; i++) {
          foreach(Produto e in produtos) {
            if(pacote[i].getTipo() == e.getTipo()) 
              { e.downQuant( pacote[i].getQuantidade() ); }
          }
        }
      } catch { return false; }
      return true;
    }

    public int getClientes(string nome){ //  Encontrar o id de algum cliente pelo nome - retorna -1 caso não encontre
      for(int i = 0; i < clientes.Count; i++){
        if(nome == clientes[i].getNome()) { return i; }
      }
      return -1;
    }


    private void comoViajar(){ //favor dar um nome melhor pro metodo
      List<int> rota = new List<int>();
      if(true){
        rota = menorRota();
      }



    }

    private List<int> menorPrazo() { // retorna uma lista crescente de prazos com os index da encomenda
      List<int> prazos = new List<int>();
      List<int> auxPrazos = new List<int>();
      List<int> rota = new List<int>();
      int aux;

      for(int i=0; i<encomendas.Count; i++){ // Pegar o prazo de cada encomenda
        prazos.Add(encomendas[i].getPrazo());
      }
      auxPrazos = prazos;

      rota.Add(auxPrazos.IndexOf(prazos.Min())); // Adiciona o index Da menor prazo em rota
      auxPrazos.Remove(prazos.Min());
      while(rota.Count<encomendas.Count){ // enquanto a rota for menor que o numero de entregas
        aux = auxPrazos[0];
        for(int i=1; i<auxPrazos.Count; i++){
          if(auxPrazos[i]<aux){ // se o prazo for menor guarda o valor
            aux = auxPrazos[i]; 
          }
        }
        rota.Add(prazos.IndexOf(aux)); // Adiciona o index do menor elemento de auxPrazos
        auxPrazos.Remove(aux); // remove o elemento adicionado na rota
      }
      return rota;
    }

    private List<double> menorRota() { 
      List<double> distancia = new List<double>();
      List<double> auxdistancia = new List<double>(); // aux para não alterar a lista distancia
      List<int> rota = new List<int>(); // Guarda o index das distancias. Obs: o index está ligado ao cliente.
      double aux;
      for(int i=0; i<clientes.Count; i++){ // Calcular a distancia do armazem até os clientes//Encomendas
        aux = Math.Sqrt( Math.Pow(coord[0] - clientes[i].getLat(), 2) + Math.Pow(coord[1] - clientes[i].getLon(), 2));
        clientes[i].setDistancia(aux);
        distancia.Add(aux);
      }
      auxdistancia = distancia; // Para não alterar a distancia

      rota.Add(auxdistancia.IndexOf(distancia.Min())); // Adiciona o index Da menor distancia em rota
      auxdistancia.Remove(distancia.Min()); 
      while(rota.Count<clientes.Count){ // enquanto a rota for menor que o numero de entregas
        aux = auxdistancia[0];
        for(int i=1; i<auxdistancia.Count; i++){
          if(auxdistancia[i]<aux){ // se distancia for menor guarda o valor
            aux = auxdistancia[i]; 
          }
        }
        rota.Add(distancia.IndexOf(aux)); // Adiciona o index do menor elemento de auxdistancia
        auxdistancia.Remove(aux); // remove o elemento adicionado na rota
      }

      return rota;
    } 
     
    private void calcularDistancia(Cliente c){
      
    }

  }
}
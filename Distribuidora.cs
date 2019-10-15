using System;
using System.Collections.Generic;

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

    public Distribuidora(double lat = 0, double lon = 0, int i = 10000, double c = 0, double l = 0.4) {
      coord[0] = lat;
      coord[1] = lon;
      capacidade = i;
      carteira = c;
      coefLucro = l;
      combustivel = 0;

      InicializarProdutos();
    }

    protected void InicializarProdutos() { 
      //  Produto(tipo, quantidade, preço por unidade)
      produtos.Add(new Produto("frutas", 100, 300.55));

      produtos.Add(new Produto("eletrônicos", 100, 646.32));

      produtos.Add(new Produto("katanas", 100, 14555.47));
    }
    
    public void NovoCliente(string nome) {
       Random rand = new Random();
       clientes.Add( new Cliente(1, nome, rand.Next(-4000,4000) / 100, rand.Next(-8000,8000) / 100) );
       coord = clientes[getClientes(nome)].getCoord();
       Console.WriteLine("Novo cliente: {0} - lat: {1}, lon: {2}", nome, coord[0], coord[1]);
    }

    private void Abastecer() {} //  TODO

    public void Ofertar() {
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

    public void Vender() {
      List<Produto> pacote = new List<Produto>();
      bool venda = false;
      foreach(Cliente c in clientes) {
        pacote = c.Vender(produtos);
        if(pacote.Count > 0) {
          double[] coord = new double[2];
          coord = c.getCoord();
          Destino destino = new Destino(c.getNome(), coord[0], coord[1], 0);
          encomendas.Add( new Encomenda(1, pacote, destino, 0) );
          DownEstoque(pacote);
          venda = true;
        }
      }
      if(venda) { Console.WriteLine("Produto vendido"); }
    }

    private bool DownEstoque(List<Produto> pacote) {
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

    public int getClientes(string nome){
      for(int i = 0; i < clientes.Count; i++){
        if(nome == clientes[i].getNome()) { return i; }
      }
      return -1;
    }    

  }
}
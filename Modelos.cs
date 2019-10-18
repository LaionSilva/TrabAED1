using System;
using System.Collections.Generic;

namespace logistica {
  public class Produto{ //  FIXO
    private string tipo;
    private int quantidade;
    private double custo; //  Valor que a distribuidora gastou para comprar

    public Produto(string t, int q, double c) {
      tipo = t;
      quantidade = q;
      custo = c;
    }

    public string getTipo() { return tipo; }
    public int getQuantidade() { return quantidade; }
    public double getCusto() { return custo; }

    public void downQuant(int q) { quantidade -= q; }
    public void upQuant(int q) { quantidade += q; }
  }


  public class Encomenda { //  FIXO
    private int id;
    private List<Produto> pacote = new List<Produto>();
    private double preco;
    public Destino cliente; //  id do cliente
    private double frete;
    private int prazo; //  dias
    private Data dataCompra;
    private Data dataEntrega;
    private bool statusPedido;
    private bool statuaEntrega;

    public Encomenda(int i, List<Produto> p, Destino c, double f = 0) { //  TODO: Controle de datas
      id = i;
      pacote = p;
      cliente = c;
      frete = f;
    }
    // GETS
    public int getId() { return id; }
    public List<Produto> getPacote() { return pacote; }
    public double getPreco() { return preco; }
    public double getFrete() { return frete; }
    public int getPrazo() { return prazo; }
    public Data getDataCompra() { return dataCompra; }
    public Data getDataEntrega() { return dataEntrega; }
    public bool getStatusPedido() { return statusPedido; }
    public bool getStatusEntrega() { return statuaEntrega; }

    //SETS
  }


  public class Destino { 
    private string nome;
    private double lat;
    private double lon;
    private double distancia;

    public Destino(string n, double la, double lo, double d){
      nome = n;
      lat = la;
      lon = lo;
      distancia = d;
    }    

    public string getNome() { return nome; }
    public double getLat() { return lat; }
    public double getLon() { return lon; }
    public double getDistancia() { return distancia; }
    public void setDistancia(double d) { distancia = d; }
  }


  public class Relatorio {
    private string id;
    private List<Encomenda> encomendas = new List<Encomenda>();
    private List<Produto> produtos = new List<Produto>();
    private Data dataCompra;
    private int prazo; //  dias
    private Data dataEntrega;
    private Destino cliente;
    private int status; //  0 a 3 (cancelado, atrazado, entregue)
  }


  public class Data {
    private int tempo;
    private int dia;
    private int mes;
    private int ano;

    public Data(int d = 1, int m = 1, int a = 2019) {
      dia = d;
      mes = m;
      ano = a;
    }

    public void setTempo(int d = 0) { tempo += d;}
    
    public int[] getData() { 
      int[] dataAt = new int[3] {dia, mes, ano}; 
      return dataAt;
    }
    
  }

}
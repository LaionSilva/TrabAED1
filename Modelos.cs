using System;
using System.Collections.Generic;

namespace logistica {
  public class Produto{
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
  }

  public class Encomenda {
    private int id;
    private List<Produto> pacote = new List<Produto>();
    private double preco;
    private Destino cliente; //  id do cliente
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
  }

  public class Relatorio {
    private string id;
    private List<Encomenda> encomenda = new List<Encomenda>();
    private Data dataCompra;
    private int prazo; //  dias
    private Data dataEntrega;
    private string nomeCliente;
    private double[] destino = new double[2];
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
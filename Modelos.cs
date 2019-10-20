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
    public string cliente; //  id do cliente
    private double frete;
    private int prazo; //  dias
    private string dataCompra;
    private bool statuaEntrega;

    public Encomenda(int i, List<Produto> p, string c, double cl, double f = 0, string dc = "@", double pr = 0, bool s = false) { //  TODO: Controle de datas
      id = i;
      pacote = p;
      cliente = c;
      frete = f;
      double valor = 0;
      foreach(Produto pac in p){
        valor += pac.getCusto() * pac.getQuantidade();
      }
      if(pr == 0) { preco = valor * (1 + cl); } 
      else { preco = pr; }
      if(dc == "@") { dataCompra = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"); }
      else { dataCompra = dc; }
      statuaEntrega = s;
    }
    // GETS
    public int getId() { return id; }
    public List<Produto> getPacote() { return pacote; }
    public double getPreco() { return preco; }
    public string getCliente() { return cliente; }
    public double getFrete() { return frete; }
    public int getPrazo() { return prazo; }
    public string getDataCompra() { return dataCompra; }
    public bool getStatusEntrega() { return statuaEntrega; }

    //SETS
    public void setStatusEntrega(bool s) { statuaEntrega = s; }
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
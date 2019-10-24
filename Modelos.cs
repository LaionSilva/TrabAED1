using System;
using System.Collections.Generic;

namespace logistica {

  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class Produto {
    private string tipo;
    private int quantidade;
    private double custo; //  custo de compra
    private double peso;
    private double volume;

    public Produto(string t, int q, double c, double p, double v) {
      tipo = t;
      quantidade = q;
      custo = c;
      peso = p;
      volume = v;
    }

    public string getTipo() { return tipo; }
    public int getQuantidade() { return quantidade; }
    public double getCusto() { return custo; }
    public double getPeso() { return peso; }
    public double getVolume() { return volume; }

    public void downQuant(int q) { quantidade -= q; }
    public void upQuant(int q) { quantidade += q; }
  }

  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class Encomenda { //  FIXO
    private int id;
    private List<Produto> pacote = new List<Produto>();
    private double preco;
    private int cliente; //  id do cliente
    private double frete;
    private int prazo; //  dias
    private string dataCompra;
    private bool statuaEntrega;

    public Encomenda(int i, List<Produto> p, int c, double cl, double f = 0, int pr = 0, string dc = "@", bool s = false) {
      id = i;
      pacote = p;
      cliente = c;
      frete = f;
      prazo = pr; 
      statuaEntrega = s;
      double valor = 0;

      foreach(Produto pac in p) {
        valor += pac.getCusto() * pac.getQuantidade() * (1 + cl);
      }
      preco = valor;
      if(dc == "@") { 
        dataCompra = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"); 
      } else { dataCompra = dc; }     
    }

    public double getPeso() { 
      double peso = 0;
      foreach(Produto pac in pacote) { 
        peso += pac.getPeso() * pac.getQuantidade(); 
      }
      return peso + frete; 
    }

    public double getVolume() { 
      double volume = 0;
      foreach(Produto pac in pacote) { 
        volume += pac.getVolume() * pac.getQuantidade(); 
      }
      return volume; 
    }
   
    public List<Produto> getPacote() { return pacote; }
    public string getDataCompra() { return dataCompra; }
    public double getPreco() { return preco; }
    public double getFrete() { return frete; }
    public int getId() { return id; }
    public int getCliente() { return cliente; }
    public int getPrazo() { return prazo; }
    public bool getStatusEntrega() { return statuaEntrega; }

    public void setStatusEntrega(bool s) { statuaEntrega = s; }
  }

  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class Relatorio {
    private int id;
    private List<int> clientes = new List<int>();
    private List<int> entregas = new List<int>();
    private double distancia;
    private double custo;
    private double lucro;

    public Relatorio(int i, List<int> c, List<int> e, double d = 0, double v = 0, double l = 0){
      id = i;
      clientes = c;
      entregas = e;
      distancia = d;
      custo = v;
      lucro = l;
    }

    public int getId() { return id; }
    public List<int> getClientes() { return clientes; }
    public List<int> getEntregas() { return entregas; }
    public double getDistancia() { return distancia; }
    public double getCusto() { return custo; }
    public double getLucro() { return lucro; }
  }

  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class DadosLog {
    public int[] rota;
    public double distancia;
    public double custo;
    public double lucro;
  }

}
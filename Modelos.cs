using System;
using System.Collections.Generic;

namespace logistica {
  public class Produto{
    private string tipo;
    private int quantidade;
    private double custo; //  Valor que a distribuidora gastou para comprar
  }

  public class Encomenda{
    private string id;
    private List<Produto> pacote;
    private double preco;
    private string cliente; //  id do cliente
    private double frete;
    private int prazo; //  dias
    private Data dataCompra;
    private Data dataEntrega;
    private bool statusPedido;
    private bool statuaEntrega;
  }

  public class Relatorio{
    private string id;
    private List<Encomenda> encomenda;
    private Data dataCompra;
    private int prazo; //  dias
    private Data dataEntrega;
    private string nomeCliente;
    private double[] destino = new double[2];
    private int status; //  0 a 3 (cancelado, atrazado, entregue)
  }

  public class Data{
    private int tempo;
    private int dia;
    private int mes;
    private int ano;

    public Data (int d = 1, int m = 1, int a = 2019) {
      dia = d;
      mes = m;
      ano = a;
    }

    public void setTempo (int d = 0) { tempo += d;}
    
    public int[] getData () { 
      int[] dataAt = new int[3] {dia, mes, ano}; 
      return dataAt;
    }
  }
}
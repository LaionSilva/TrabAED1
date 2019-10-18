using System;
using System.Collections.Generic;

namespace logistica {
  public class Cliente {
    private int id;
    private string nome;
    private double[] coord = new double[2];
    private List<Produto> pedidos = new List<Produto>();
    private int tendencia; //  chance de comprar 1 a 100

    public Cliente(){}
    public Cliente(int i, string n, double lat, double lon, int t = 50) {
      id = i;
      nome = n;
      coord[0] = lat;
      coord[1] = lon;
      tendencia = t;
    }

    public int Ofertar(List<Produto> estoque) { //  Oferecer produtos ao cliente, podendo ele comprar ou não. Add in pedidos
      try{
        Random rand = new Random();
        if ((rand.Next(1,100) <= tendencia) && (estoque.Count > 0)) {
          int index = rand.Next(0, estoque.Count);
          int quant = rand.Next(1, 50);
          pedidos.Add(new Produto(estoque[index].getTipo(), quant, estoque[index].getCusto()));
          return 2;
        }
      } catch { return 0; }
      return 1;
    }

    public List<Produto> Vender(List<Produto> estoque) { //  Retorna o Encomenda.pacote
      List<Produto> pacote = new List<Produto>();
      if(checkPedidos()) {
        bool venda = false;
        foreach(Produto e in estoque) { //  Validar os pedidos pelo estoque da Distribuidora
          bool run;
          do{
            run = false;
            for(int i = 0; i < pedidos.Count; i++) { //  Registrar pedidos
              if((pedidos[i].getTipo() == e.getTipo()) && (pedidos[i].getQuantidade() <= e.getQuantidade())) {
                pacote.Add( new Produto(pedidos[i].getTipo(), pedidos[i].getQuantidade(), pedidos[i].getCusto()) );
                e.downQuant( pedidos[i].getQuantidade() );
                pedidos.RemoveAt(i);
                i--;
                run = true;
                venda = true;
              } 
              else if(pedidos[i].getTipo() == e.getTipo()) 
                { Console.WriteLine("Produto não encontrado ou em falta ao vender: {0} - {1} - cli-ven", e.getTipo(), e.getQuantidade()); }
            }
          } while(run);
        }
        if(venda) { Console.WriteLine("Pedido aceito. Cliente: {0}", nome); }
      }
      return pacote;
    }

    public bool checkPedidos(){ //  Verificar se existe pedidos pendentes
      if(pedidos.Count > 0) { return true; }
      return false;
    }

    public int getId() { return id; }
    public string getNome() { return nome; }
    public double[] getCoord() { return coord; }
    public int getTendencia() { return tendencia; }
    public List<Produto> getPedidos() { return pedidos; }

    public void setPedidos(List<Produto> p) { pedidos.AddRange(p); }

  }
}
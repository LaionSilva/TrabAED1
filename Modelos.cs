using System;
using System.Collections.Generic;

namespace logistica {

  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class Estoque { //  Pai de Classes Principais
    protected List<Produto> produtos = new List<Produto>(); 
    protected List<Produto> pedidos = new List<Produto>();
    protected List<Encomenda> encomendas = new List<Encomenda>();
    protected List<Encomenda> entregas = new List<Encomenda>();
	  protected List<Cliente> clientes = new List<Cliente>(); 

    //  Endereços dos locais de estocagem - Usados principalmente na classe Save
    protected string fileProdutos = "file_produtos.txt";
    protected string fileClientes = "file_clientes.txt";
    protected string fileEncomendas = "file_encomendas.txt";
    protected string fileEntregas = "file_entregas.txt";
    protected string fileRelatorio = "Relatorio.txt";
    protected string fileSenhas = "file_senhas.txt";
    protected string fileLogException = "file_logException";
  }

  public class Etiqueta { //  Pai de Classes Modelo - usado nos objetos que manipulam mercadorias do estoque até a entrega
    protected int id;
    protected double preco;
    protected double custo;
    protected double lucro;
    protected double peso;
    protected double volume;

    public int getId() { return id; }
    public double getPreco() { return preco; }
    public double getCusto() { return custo; }
    public double getLucro() { return lucro; }
    public double getPeso() { return peso; }
    public double getVolume() { return volume; }
  }

  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class Produto : Etiqueta { //  Mercadoria que ainda não foi vendida
    private string tipo;
    private int quantidade;

    public Produto(string t, int q, double c, double p, double v) {
      tipo = t;
      quantidade = q;
      custo = c;
      peso = p;
      volume = v;
    }

    public string getTipo() { return tipo; }
    public int getQuantidade() { return quantidade; }

    public void downQuant(int q) { quantidade -= q; }
    public void upQuant(int q) { quantidade += q; }
  }

  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class Encomenda : Etiqueta { //  Conjunto de mercadorias que já foram vendidas a um único cliente e que ainda não foi despachada para a entrega
    private List<Produto> pacote;
    private int cliente; //  id do cliente
    private double frete;
    private int prazo; //  dias - ##fora de uso no momento##
    private string dataCompra; //  ##fora de uso no momento##
    private bool statuaEntrega;

    public Encomenda(int i, List<Produto> p, int c, double cl, double f = 0, int pr = 0, string dc = "@", bool s = false) {
      pacote = new List<Produto>();

      id = i;
      pacote = p;
      cliente = c;
      frete = f;
      prazo = pr; 
      statuaEntrega = s;

      double valor = 0; //  Calcular somatório dos preços de todos os produtos
        foreach(Produto pac in p) {
          valor += pac.getCusto() * pac.getQuantidade() * (1 + cl);
        }
      preco = valor;
      
      if(dc == "@") { 
        dataCompra = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"); 
      } else { dataCompra = dc; }     
    }

    public double getPesoEnc() { //  Calcular somatório dos pesos de todos os produtos
      double pesoEnc = 0;
        foreach(Produto pac in pacote) { 
          pesoEnc += pac.getPeso() * pac.getQuantidade(); 
        }
      return pesoEnc + frete; 
    }

    public double getVolumeEnc() { //  Calcular somatório dos volumes de todos os produtos
      double volumeEnc = 0;
        foreach(Produto pac in pacote) { 
          volumeEnc += pac.getVolume() * pac.getQuantidade(); 
        }
      return volumeEnc; 
    }
   
    public List<Produto> getPacote() { return pacote; }
    public string getDataCompra() { return dataCompra; }
    public double getFrete() { return frete; }
    public int getCliente() { return cliente; }
    public int getPrazo() { return prazo; }
    public bool getStatusEntrega() { return statuaEntrega; }

    public void setStatusEntrega(bool s) { statuaEntrega = s; }
  }

  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class Relatorio : Etiqueta { //  Guardar na memória todos os dados de todas as entrega para, posteriormente, gerar um relatório final de entrega
    private List<int> clientes = new List<int>();
    private List<int> entregas = new List<int>();
    private double distancia;

    public Relatorio(List<int> c, List<int> e, double d = 0, double v = 0, double l = 0){
      clientes = c;
      entregas = e;
      distancia = d;
      custo = v;
      lucro = l;
    }

    public List<int> getClientes() { return clientes; }
    public List<int> getEntregas() { return entregas; }
    public double getDistancia() { return distancia; }
  }

  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class DadosLog { //  Dados de mapeamento usados na composição do relatório - Principal uso: classe Mapeamento ==> classe Distribuidora
    public int[] rota;
    public double distancia;
    public double custo;
    public double lucro;
    public double peso;
    public double volume;
    public int containers;
    public List<Cliente> cliOrdem;
    public List<string> relatorio;
    public List<string> relatorioWeb;
    public List<string> mapa;
  }

  public class DadosLogException { //  Dados para geração de logs de erro
    public string tipo;
    public string classe;
    public string metodo;
    public string data;
    public Exception mensagem;
    public string nota = "";
    public string notaAdm = "";
  }

  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class Ponto {
    protected int idOr;
    protected int idDe;
    protected double lat;
    protected double lon;

    public Ponto(){}
    public Ponto(double la, double lo, int o, int d) { 
      lat = la;
      lon = lo;
      idOr = o;
      idDe = d;
    }

    public double getLat() { return lat; }
    public double getLon() { return lon; }
    public int getIdOr() { return idOr; }
    public int getIdDe() { return idDe; }

    public void setLat(double l) { lat = l; }
    public void setLon(double l) { lon = l; }
    public void setIdOr(int i) { idOr = i; }
    public void setIdDe(int i) { idDe = i; }
  }
  
  //////////////////////////////////////////////////////////////////////////////////////////////////////// 
  public class Posicao : Ponto {
    private int id;
    private bool config;

    public Posicao(int i, double c1, double c2, bool c = false) {
      id = i;
      lat = c1;
      lon = c2;
      config = c;
    }

    public int getId() { return id; }
    public double getDist(double la,double lo) { return Caminhao.CalcularDistancia(lat, lon, la, lo); }
    public bool getConfig() { return config; }

    public void setId(int i) { id = i; }
    public void setConfig(bool c) { config = c; }
  }

}
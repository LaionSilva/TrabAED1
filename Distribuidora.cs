using System;
using System.Collections.Generic;
using System.Linq; //Para calcular o menor ou maior valor de uma lista

namespace logistica {
  public class Distribuidora : Estoque { 
    private Save file;
    private Relatorio diarioEntregas;
    private double[] coord; //  lat, long
    private double coefLucro;
    private double carteira; //  total de verbas
    private bool carteiraInf;
    public static bool statusMapeamento = true;
    
    public Distribuidora(double lat = 0, double lon = 0, /*int i = 10000,*/ double c = 0, double l = 0.4) {
      file = new Save();

      coord = new double[2];
      coord[0] = lat;
      coord[1] = lon;
      carteiraInf = true;
      carteira = c;
      coefLucro = l;

      Carregar();    
      Salvar();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  ARMAZENAMENTO DE DADOS
    protected void Salvar() { //  Guardar listas em dados.txt
      file.setProdutos(produtos); 
      file.setClientes(clientes);
      file.setEncomendas(encomendas); 
      file.setEntregas(entregas);
    }

    protected void Carregar() { //  Carregar listas de dados.txt
      produtos.Clear(); 
      clientes.Clear();
      encomendas.Clear();
      produtos = file.getProdutos(); 
      clientes = file.getClientes(); 
      encomendas = file.getEncomendas(); 
    }

    public void ResetarFiles() {
      file.Reset();
    }
    //  ARMAZENAMENTO DE DADOS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  PRODUTOS 
    public bool NovoProduto(string tipo, double custo, double peso, double volume) { //  Produtos oferecidos pela distribuidora
      try {
        foreach(Produto p in produtos) {
          if(p.getTipo() == tipo) { return false; }
        }
        produtos.Add(new Produto(tipo, 5000, custo, double.Parse(Console.ReadLine()), volume)); 
        Salvar();
        return true;    
      } 
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "NovoProduto"); 
        return false;
      }
    }

    public bool ComprarProduto(string tipo, int n) { //  Abastecer o estoque dos produtos cadastrados
      bool prod = false, renda = false;

      try{
        foreach(Produto p in produtos){ //  Buscar pelo produto solicitado
          if(tipo.ToUpper() == p.getTipo().ToUpper()) {
            if((carteira >= n * p.getCusto()) || carteiraInf) {
              p.upQuant(n); //  Atualizar estoque referente a compra dos produtos
              if(!carteiraInf) { carteira -= n * p.getCusto(); }
              renda = true;
            } else { Console.WriteLine("\nNão há renda suficiente: dis-ComPro"); }       
            prod = true;
            break;             
          } 
        }

        if(renda && prod) { //  Relatórios de ação para o usuário
          Console.WriteLine("\nEstoque reabastecido"); 
          Salvar(); 
          return true; 
        }
        else if (!prod) { 
          Console.WriteLine("\nProduto não encontrado"); 
          return false; 
        }
        else { 
          Console.WriteLine("\nVerba insuficiente"); 
          return false; 
        }
      } 
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "ComprarProduto"); 
        return false;
      }
    }

    private bool DownEstoque(List<Produto> pacote) { // abater do estoque o que foi vendido
      try{
        foreach(Produto pac in pacote) {
          foreach(Produto pro in produtos) {
            if(pac.getTipo() == pro.getTipo()) { 
              pro.downQuant( pac.getQuantidade() ); 
            }
          }
        }
        Salvar();
        return true;
      } 
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "DownEstoque"); 
        return false;
      }
    }

    public void ListarProdutos() { //  Mostrar ao usuário uma lista dos produtos cadastradod
      if(produtos.Count == 0) { Console.WriteLine("Nenhum produto cadastrado\n"); }
      else {
        try {
          foreach (Produto p in produtos){
            Console.WriteLine("{0}  |  {1}un  |  {2}kg |  {3}m^3", p.getTipo(), p.getQuantidade(), p.getPeso(),p.getVolume());
          }
        } 
        catch(Exception e) { 
          LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "ListarProdutos"); 
        }
      }
    }
    //  PRODUTOS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  CLIENTES
    public void NovoCliente(string nome) { //  Cadastrar novo cliente para a distribuidora - localização aleatória
      Random rand = new Random();

      List<int> ids = new List<int>();
      ids.Add(0);
      foreach(Cliente c in clientes) {
        ids.Add(c.getId());
      }

      try{
        clientes.Add( new Cliente(ids.Max() + 1, nome, rand.Next(-4000,4000) / 100, rand.Next(-8000,8000) / 100) ); //  Novo cliente
        coord = clientes[clientes.Count - 1].getCoord();
        Console.WriteLine("Novo cliente: {0} - lat: {1}, lon: {2}", nome, coord[0], coord[1]);
        Salvar();
      } 
      catch(IndexOutOfRangeException e) { 
        LogisticaException.ExceptionGrave("LE_IndexOutOfRangeException", e, "Distribuidora", "NovoCliente"); 
      } 
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "NovoCliente"); 
      }     
    }

    public void Ofertar() { //  Oferecer ao cliente os produtos, há operações randomicas envolvidas
      bool compra = false;
      int oferta = 0;

      try{
        foreach(Cliente c in clientes) {
          oferta = c.Ofertar(produtos);
          if(oferta == 0) { 
            Console.WriteLine("Erro ao ofertar produtos: dis-ofe"); 
          }
          else if (oferta == 2) { 
            compra = true; 
          }
          if(compra) { 
            Console.WriteLine("Cliente {0} comprou", c.getNome()); 
          }
        }
      } 
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "Ofertar"); 
      }  

      if(compra) { 
        Salvar();
        Console.WriteLine("Pedidos anotados!\n");
      } else { Console.WriteLine("Não houve novos pedidos\n"); }
    }

    public void Vender() { //  Aceitar pedidos realizados pelos clientes
      List<Produto> pacote = new List<Produto>();
      List<int> ids = new List<int>();
      bool venda = false;

      try{
        foreach(Cliente c in clientes) { //  O processo é realizado pra cada cliente individualmente
          pacote.Clear();
          pacote.AddRange(c.Vender(produtos));

          if(pacote.Count > 0) {
            double[] coord = new double[2];
            coord = c.getCoord();
            double frete = Caminhao.CalcularDistancia(0, coord[0], 0, coord[1]) * 0.60;
            int dist = (int)(Caminhao.CalcularDistancia(0, coord[0], 0, coord[1]) * 0.60);
            ids.Clear();
            ids.Add(0);
            foreach(Encomenda e in encomendas) {
              ids.Add(e.getId());
            }
            encomendas.Add( new Encomenda(ids.Max() + 1, pacote, getClientes(c.getNome()), coefLucro, frete, dist) ); 
            DownEstoque(pacote); // abater do estoque o que foi vendido
            venda = true;
          }
        }
        if(venda) { 
          OrganizarEncomendas(); //  Otimizar encomendas, organizando em ordem decrescente por lucratividade
          Salvar();
          Console.WriteLine("Pedidos aceitos!\nEncomendas geradas!\n");
        } else { Console.WriteLine("Não há pedidos pendentes\n"); }
      } 
      catch(IndexOutOfRangeException e) { 
        LogisticaException.ExceptionGrave("LE_IndexOutOfRangeException", e, "Distribuidora", "Vender"); 
      } 
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "Vender"); 
      } 
    }

    public int getClientes(string nome){ //  Encontrar o id de algum cliente pelo nome - retorna -1 caso não encontre
      for(int i = 0; i < clientes.Count; i++) {
        if(nome == clientes[i].getNome()) { return clientes[i].getId(); }
      } return -1;
    }

    public void ListarClientes() { //  Mostrar ao usuário uma lista dos clientes cadastradod
      if(clientes.Count == 0) { Console.WriteLine("Nenhum cliente cadastrado\n"); }
      else {
        foreach (Cliente c in clientes) {
          Console.WriteLine("Nome: {0}  |  Id: {1}  |  Lat: {2} |  Lon: {3}", c.getNome(), c.getId(), c.getLat(), c.getLon());
        }
      }
    }
    //  CLIENTES
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  ENCOMENDAS
    private void OrganizarEncomendas() { //  Ordena a lista de encomendas em ordem decrescente por lucratividade, considerando preço, volume e peso
      int[] auxIndex = new int[encomendas.Count];
        List<double> coefMon = new List<double>(); 
        List<Encomenda> auxEnc = new List<Encomenda>();

      try{
        foreach(Encomenda e in encomendas)
          { coefMon.Add( (( (e.getPreco() - e.getFrete()) / e.getPesoEnc() / 0) / (e.getVolumeEnc())) * 1000 ); }
        for(int i = 0; i < encomendas.Count; i++) {
          auxIndex[i] = coefMon.IndexOf(coefMon.Max());
          coefMon[ coefMon.IndexOf(coefMon.Max()) ] = -1;
        }
        for(int i = 0; i < encomendas.Count; i++)
          { auxEnc.Add( encomendas[ auxIndex[i] ] ); }
        encomendas.Clear();
        encomendas.AddRange(auxEnc);  
      } 
      catch(DivideByZeroException e) { 
        LogisticaException.ExceptionGrave("LE_DivideByZeroException", e, "Distribuidora", "OrganizarEncomendas"); 
      } 
      catch(IndexOutOfRangeException e) { 
        LogisticaException.ExceptionGrave("LE_IndexOutOfRangeException", e, "Distribuidora", "OrganizarEncomendas"); 
      } 
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "OrganizarEncomendas"); 
      } 
    }
    //  ENCOMENDAS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  LOGÍSTICA
    public void ComoViajar(int nCaminhoes = 1) {
      DadosLog dados = new DadosLog(); 
      Mapeamento mapa = new Mapeamento();
      List<Cliente> osClientes = new List<Cliente>(); //  Lista com os Clientes atendidos
      List<string> nomeClientes = new List<string>();//  Nome dos clientes atendidos
      List<double> precoPorCliente = new List<double>(); //  Somatorio dos preços das encomendas por cliente 
      double precoTotal = 0; //  Valor bruto que vamos receber
      double volumeTot = 0, pesoTot = 0; //Peso e  Volume Tot das encomendas que irão na viagem
      bool erro = false;

      Console.WriteLine ("\nGERAR VIAGEM");

      Salvar();
      Carregar();
      try {
        if(!erro){
          foreach(Encomenda e in encomendas) {
            if((e.getVolumeEnc() < (Caminhao.volumeBau * nCaminhoes) - volumeTot) && (e.getPesoEnc() < (Caminhao.pesoBau * nCaminhoes) - pesoTot)){
              volumeTot += e.getVolumeEnc();
              pesoTot += e.getPesoEnc();
              foreach(Cliente c in clientes) {
                if(c.getId() == e.getCliente()) {
                  nomeClientes.Add(c.getNome());
                  osClientes.Add(c);
                  precoPorCliente.Add(e.getPreco());
                }
              }          
            }
            if(osClientes.Count >= 30) { break; }
          }
        }
      } 
      catch(IndexOutOfRangeException e) {
        LogisticaException.ExceptionGrave("LE_IndexOutOfRangeException", e, "Distribuidora", "ComoViajar/foreach(encomendas)", true);
        erro = true;
      }
      catch(ArgumentOutOfRangeException e) {
        LogisticaException.ExceptionGrave("LE_ArgumentOutOfRangeException", e, "Distribuidora", "ComoViajar/foreach(encomendas)", true); 
        erro = true;
      }
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "ComoViajar/foreach(encomendas)", true); 
        erro = true;
      } 

      try{
        if(!erro){
          foreach(double ppc in precoPorCliente) {
            precoTotal += ppc;
          }
        }
      } 
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "ComoViajar/foreach(precoPorCliente)", true); 
        erro = true;
      } 

      try {
        if(!erro){
          double[,] cliMapa = new double[2, osClientes.Count];
          for(int i = 0; i < osClientes.Count; i++) {
            cliMapa[0, i] = osClientes[i].getLat();
            cliMapa[1, i] = osClientes[i].getLon();
          }
          if(osClientes.Count > 0) {
            statusMapeamento = true;
              dados = mapa.Iniciar(cliMapa, osClientes.Count); 
            if(!statusMapeamento) {
              throw new LogisticaException("LE_Main_ErroMapeamento");
            }
            dados.custo = (dados.distancia / (Caminhao.eficMotor * (1 - nCaminhoes / 10))) * 3.686 + Caminhao.CalcularDiariaMotorista(dados.distancia); //  preço do diesel 3,686
            dados.lucro = precoTotal - dados.custo;//dados.custo - (dados.custo/1.4);
            Salvar();
            
            NovoRelatorioEntrega(dados, osClientes);
            GerarRelatório(dados);
            EnviarRelatorioEmail(dados.relatorioWeb);
          } else { 
            if(encomendas.Count == 0) { Console.WriteLine("Não há encomendas pendentes no momento"); }
            else { Console.WriteLine("Não é possivel fechar nenhuma encomenda no momento\nFavor verificar as condições de transporte do veiculo"); }
          }
        }
      }
      catch(LogisticaException) {}
      catch(IndexOutOfRangeException e) {
        LogisticaException.ExceptionGrave("LE_IndexOutOfRangeException", e, "Distribuidora", "ComoViajar/dados", true);
      }
      catch(ArgumentOutOfRangeException e) {
        LogisticaException.ExceptionGrave("LE_ArgumentOutOfRangeException", e, "Distribuidora", "ComoViajar/dados", true);
      }
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "ComoViajar/dados", true); 
      }
      
    }

    private void NovoRelatorioEntrega(DadosLog dados, List<Cliente> CliEntrega) { //  Cria um pacote de dados para gerar o relatório de entregas  
      List<int> idClientes = new List<int>();
      List<int> pacote = new List<int>();
      List<Encomenda> auxEnc = new List<Encomenda>();
      List<Cliente> cliAux = new List<Cliente>();
      string posicao = "";
      
      try {
        posicao = "for(CliEntrega)";
        for(int i = 0; i < CliEntrega.Count; i++) {
          idClientes.Add( CliEntrega[ dados.rota[i] ].getId() );
        }

        posicao = "foreach(encomendas)";
        foreach(Encomenda e in encomendas) {          
          if(idClientes.IndexOf(e.getCliente()) != -1 ){
            pacote.Add(e.getId());
            auxEnc.Add(e);
          }
        }

        posicao = "foreach(auxEnc)";
        foreach(Encomenda e in auxEnc) {
          encomendas.Remove(e);
          e.setStatusEntrega(true);
        }
        entregas.AddRange(auxEnc);
  
        posicao = "foreach(dados.rota)";
        foreach(int r in dados.rota) {
          cliAux.Add( CliEntrega[ r ] );
        }
        dados.cliOrdem = cliAux;

        posicao = "salvar";
        Salvar();

        posicao = "diarioEntregas";
        diarioEntregas = new Relatorio(idClientes, pacote, dados.distancia, dados.custo, dados.lucro); 
        Console.WriteLine ("Novo relatório de entrega gerado.\n");

      }
      catch(IndexOutOfRangeException e) { 
        LogisticaException.ExceptionGrave("LE_IndexOutOfRangeException", e, "Distribuidora", "NovoRelatorioEntrega/" + posicao); 
      } 
      catch(ArgumentNullException e) { 
        LogisticaException.ExceptionGrave("LE_Distribuidora_NovoRelatorioEntrega_DiarioEntrega", e, "Distribuidora", "NovoRelatorioEntrega/" + posicao, true); 
      }
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "NovoRelatorioEntrega/" + posicao); 
      }
    }

    public void GerarRelatório(DadosLog dados) {
      List<string> impressao = new List<string>();
      List<string> impressaoWeb = new List<string>();
      List<int> idEnt = new List<int>();
      List<int> idCli = new List<int>();
      int[] memoria = new int[2]{-1, -1}; 

      try {
        if(diarioEntregas.getClientes().Count > 0) {
          foreach(Encomenda e in entregas){
            idEnt.Add(e.getId()); 
          }
          foreach(Cliente c in clientes){
            idCli.Add(c.getId());
          }

          impressao.Add( "\n==========  DIÁRIO  DE  ENTREGA  ==========" );
          impressaoWeb.Add( "<br/>==========&nbsp;&nbsp;DIÁRIO&nbsp;&nbsp;DE&nbsp;&nbsp;ENTREGA&nbsp;&nbsp;==========" );

          System.Threading.Thread.Sleep(20);
          foreach(int c in diarioEntregas.getClientes()) {
            if(memoria[1] != c) {
              if((idCli.IndexOf(c) < 0) || (idCli.IndexOf(c) >= clientes.Count)){
                throw new LogisticaException("LE_Distribuidora_GerarRelatório_indexClientes"); 
              }
              double[] auxCoord = clientes[idCli.IndexOf(c)].getCoord(); 
              impressao.Add("\n\nCliente: ");
              impressaoWeb.Add("<br/><br/>Cliente: ");
              impressao.Add( String.Format("{0}", clientes[idCli.IndexOf(c)].getNome()) );
              impressaoWeb.Add( String.Format("{0}", clientes[idCli.IndexOf(c)].getNome()) );
              impressao.Add( " - ID: " );
              impressaoWeb.Add( " - ID: " );
              impressao.Add( String.Format("{0}", clientes[idCli.IndexOf(c)].getId()) );
              impressaoWeb.Add( String.Format("{0}", clientes[idCli.IndexOf(c)].getId()) );
              impressao.Add( " | lat " );
              impressaoWeb.Add( " | lat " );
              impressao.Add( String.Format("{0}", auxCoord[0]) );
              impressaoWeb.Add( String.Format("{0}", auxCoord[0]) );
              impressao.Add( " - lon " );
              impressaoWeb.Add( " - lon " );
              impressao.Add( String.Format("{0}", auxCoord[1]) );
              impressaoWeb.Add( String.Format("{0}", auxCoord[1]) );

              foreach(int e in diarioEntregas.getEntregas()) {
                System.Threading.Thread.Sleep(20);            
                if((idEnt.IndexOf(e) >= 0) && (entregas[idEnt.IndexOf(e)].getCliente() == c)) {
                  impressao.Add( "\nLote ID: " );
                  impressaoWeb.Add( "<br/>Lote ID: " );
                  impressao.Add( String.Format("{0}", e));
                  impressaoWeb.Add( String.Format("{0}", e));

                  foreach(Produto p in entregas[idEnt.IndexOf(e)].getPacote()) {
                    System.Threading.Thread.Sleep(20);
                    impressao.Add( "\nProduto: " );
                    impressaoWeb.Add( "<br/>Produto: " );
                    impressao.Add( String.Format("{0}", p.getTipo()) );
                    impressaoWeb.Add( String.Format("{0}", p.getTipo()) );
                    impressao.Add( " | quant: " );
                    impressaoWeb.Add( " | quant: " );
                    impressao.Add( String.Format("{0}", p.getQuantidade()) );
                    impressaoWeb.Add( String.Format("{0}", p.getQuantidade()) );
                    impressao.Add( " - preço: " );
                    impressaoWeb.Add( " - preço: " );
                    impressao.Add( String.Format("{0}", p.getCusto() * p.getQuantidade()) );
                    impressaoWeb.Add( String.Format("{0}", p.getCusto() * p.getQuantidade()) );
                    impressao.Add( " - peso: " );
                    impressaoWeb.Add( " - peso: " );
                    impressao.Add( String.Format("{0}", p.getPeso() * p.getQuantidade()) );
                    impressaoWeb.Add( String.Format("{0}", p.getPeso() * p.getQuantidade()) );
                    impressao.Add( " - vol: " );
                    impressaoWeb.Add( " - vol: " );
                    impressao.Add( String.Format("{0}", p.getVolume() * p.getQuantidade()) );
                    impressaoWeb.Add( String.Format("{0}", p.getVolume() * p.getQuantidade()) );
                  }
                }
              }

            }
            memoria[1] = c;
          }
          impressao.Add( "\n\nDistância: " );
          impressaoWeb.Add( "<br/><br/>Distância: " );
          impressao.Add( String.Format("{0:0.00}", diarioEntregas.getDistancia()) );
          impressaoWeb.Add( String.Format("{0:0.00}", diarioEntregas.getDistancia()) );
          impressao.Add( "km\nCusto total da viagem: R$:" );
          impressaoWeb.Add( "km<br/>Custo total da viagem: R$:" );
          impressao.Add( String.Format("{0:0.00}", diarioEntregas.getCusto()) );
          impressaoWeb.Add( String.Format("{0:0.00}", diarioEntregas.getCusto()) );
          impressao.Add( "\nLucro total liquido: R$:" );
          impressaoWeb.Add( "<br/>Lucro total liquido: R$:" );
          impressao.Add( String.Format("{0:0.00}", diarioEntregas.getLucro()) );
          impressaoWeb.Add( String.Format("{0:0.00}", diarioEntregas.getLucro()) );
          impressao.Add( "\n" );
          impressaoWeb.Add( "<br/>" );

        } else { Console.WriteLine ("Não há relatório a relatar"); }

        dados.relatorio = impressao;
        dados.relatorioWeb = impressaoWeb;
        file.setRelatorio(dados);
        Console.WriteLine("Relatório Impresso!\n");
      } 
      catch (LogisticaException) {}
      catch(Exception e) { 
        LogisticaException.ExceptionGrave("LE_ExceptionNaoTratada", e, "Distribuidora", "GerarRelatório"); 
      }
    }

    private void EnviarRelatorioEmail(List<string> relatorio) {
      string email = "";
      bool validade = false, loop = true;
      int cont = 0;

      try {
        Console.Write ("Deseja receber uma cópia por Email? (y/n)\n>> ");
        while(loop) {
          if(Console.ReadLine().ToUpper() == "Y") {
            Console.Write ("\nInforme um email válido:\n>> ");
            email = Console.ReadLine();

            foreach(char c in email) {
              if((c == '@') && !validade) { validade = true; }
              if(validade) { 
                if((c == '.') || (c == 'c') || (c == 'o') || (c == 'm')) { cont++; } 
                else if(cont >= 4) { break; } 
                else { cont = 0; }
              }
            }

            if(cont >= 4) {
              Console.Write ("\nEnviando email...");
                EMail.EnviarRelatorio(relatorio, email);
              loop = false;
            }
            else {
              Console.Write ("\nEmail inválido, deseja tentar denovo? (y,n):\n>> ");
            }

          } else { 
            Console.WriteLine(); 
            break; 
          }
        }
      } catch {}

    }
    // LOGÍSTICA
    ////////////////////////////////////////////////////////////////////////////////////////////////////////    
  }
}



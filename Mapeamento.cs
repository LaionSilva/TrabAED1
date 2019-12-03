using System;
using System.Collections.Generic;
using System.IO;

namespace logistica {
  public class Mapeamento {      
    DadosLog dados = new DadosLog();

    private List<Posicao> clientes = new List<Posicao>();
    private List<Posicao> cliCheck = new List<Posicao>();
    private List<Posicao> cliPen = new List<Posicao>();
    private List<Ponto> pontos = new List<Ponto>();
    private List<string> memoria = new List<string>();
    private List<double> dist = new List<double>();
    private List<int> cliId = new List<int>();   

    private int[,] mapa;
    private int[,] mapa2;
    private int[] coordIn;

    private int range = 0;
    private int indexOrigem = 0;
    private int maxLat = 0;
    private int maxLon = 0;


    public Mapeamento(int mla = 40, int mlo = 80, double latIn = 0, double lonIn = 0) {
      mapa = new int[mla * 2, mlo * 2];
      mapa2 = new int[mla * 2, mlo * 2];
      coordIn = new int[2]{(int)latIn, (int)lonIn};
      maxLat = mla;
      maxLon = mlo;
    }

    public DadosLog Iniciar(double[,] cli, int n) {
      int[] coord = new int[2]{0,0};
      int[] ii = new int[2];
      range = n;
      indexOrigem = range + 1;

      clientes.Add( new Posicao(indexOrigem, coordIn[0], coordIn[1]) );
        mapa[(int)coordIn[0] + maxLat, (int)coordIn[1] + maxLon] = indexOrigem;
      for(int i = 1; i <= range; i++) {
        coord[0] = (int)cli[0, i - 1];
        coord[1] = (int)cli[1, i - 1];
        clientes.Add( new Posicao(i, coord[0], coord[1]) );
          mapa[(int)coord[0] + maxLat, (int)coord[1] + maxLon] = i;
      }

      AtualizarMapa(true);
      Combinar(coordIn);
      Relacionar();
      
      do { 
        ii = Mapa();
        if((ii[0] != -1) && (ii[1] != -1)) { 
          if( !CheckPontos(ii[0], ii[1]) )  { break; } 
          Ajustar();
          CorrigirSentido(); 
        }        
      } while((ii[0] != -1) && (ii[1] != -1));
      GerarMapa();
      //Relatorio();

      dados.distancia = CalcularDistanciaRota();
      dados.rota = getRota();

      return dados;
    }


    private void AtualizarMapa(bool sentido = false) {
      if(sentido)
        for(int i = 0; i < maxLat * 2; i++) {
          for(int j = 0; j < maxLon * 2; j++) {
            mapa2[i, j] = mapa[i, j];
          }
        }
      else 
        for(int i = 0; i < maxLat * 2; i++) {
          for(int j = 0; j < maxLon * 2; j++) {
            mapa[i, j] = mapa2[i, j];
          }
        }
    }


    private double CalcularDistanciaRota() {
      double dist = 0;
      int auxIndex = 0;

      AtualizarCliId();
      foreach(Posicao c in cliCheck) {
        auxIndex = cliId.IndexOf(c.getIdDe());
        dist += c.getDist( cliCheck[auxIndex].getLat(), cliCheck[auxIndex].getLon() );
      }

      return dist;
    }

    
    private int[] getRota() {
      int origem = cliId.IndexOf(indexOrigem);
      int alvoAt = origem, i = 0;
      int[] rota = new int[range];

      AtualizarCliId();
      
      while(true) {
        alvoAt = cliCheck[alvoAt].getIdDe();
        if(cliId.IndexOf(alvoAt) != origem) {
          rota[i] = alvoAt - 1;
        } else { break; }        
        alvoAt = cliId.IndexOf(alvoAt);
        i++;
      }
      return rota;
    }


    private void AtualizarCliId() {
      cliId.Clear();
      foreach(Posicao c in cliCheck){
        cliId.Add(c.getId());
      } System.Threading.Thread.Sleep(10);
    }


    public void Combinar(int[] coordIn) {    
      Posicao alvo = new Posicao(0,coordIn[0], coordIn[1]);
      double d;
      int id;

      Console.WriteLine ("Combinando");
      cliPen = clientes;
      foreach(Posicao c in cliPen) {
        dist.Add( c.getDist(0, 0) );
      }
      
      d = ListMin(dist);
      id = dist.IndexOf(d);
      alvo = cliPen[id];
      dist.Remove(d);
      cliPen.RemoveAt(id);

      while(cliPen.Count > 0) {
        dist.Clear();
        foreach(Posicao c in cliPen) {
          dist.Add( c.getDist(alvo.getLat(), alvo.getLon()) );
        }
        d = ListMin(dist);
        id = dist.IndexOf(d);
        alvo.setIdDe(cliPen[id].getId());
        cliPen[id].setIdOr(alvo.getId());
        cliCheck.Add(alvo);
        alvo = cliPen[id];
        cliPen.RemoveAt(id);
        dist.Remove(d);
      }

      alvo.setIdDe(cliCheck[0].getId());
      cliCheck[0].setIdOr(alvo.getId());
      cliCheck.Add(alvo);

      AtualizarCliId();
    }

    private void Ajustar() {
      Console.WriteLine ("Realizando ajustes");
      foreach(Posicao c in cliCheck) {
        dist.Clear();
        foreach(Posicao c2 in cliCheck) {
          if(c.getId() == c2.getId()) {
            dist.Add(10000);
          } else { dist.Add(c.getDist(c2.getLat(), c2.getLon())); }
        }

        double d1 = 0, d2 = 0;
        int id1 = 0, id2 = 0;
        d1 = ListMin(dist);
        id1 = dist.IndexOf(d1);
        dist[ dist.IndexOf(d1) ] = 10000;
        d2 = ListMin(dist);
        id2 = dist.IndexOf(d2);
        
        if(cliCheck[id1].getIdOr() == cliCheck[id2].getId()) {
          cliCheck[id1].setIdOr(c.getId());
          cliCheck[id2].setIdDe(c.getId());

          cliCheck[ cliId.IndexOf(c.getIdOr()) ].setIdDe(c.getIdDe());
          cliCheck[ cliId.IndexOf(c.getIdDe()) ].setIdOr(c.getIdOr());  

          c.setIdOr(cliCheck[id2].getId());
          c.setIdDe(cliCheck[id1].getId()); 
        }
        else if(cliCheck[id1].getIdDe() == cliCheck[id2].getId()) {
          cliCheck[id1].setIdDe(c.getId());
          cliCheck[id2].setIdOr(c.getId());

          cliCheck[ cliId.IndexOf(c.getIdDe()) ].setIdOr(c.getIdOr());
          cliCheck[ cliId.IndexOf(c.getIdOr()) ].setIdDe(c.getIdDe());  

          c.setIdDe(cliCheck[id2].getId());
          c.setIdOr(cliCheck[id1].getId());
        }
      }
    }


    private void Relacionar(){
      double[] coord = new double[2]{0,0};
      double[] coordOr = new double[2]{0,0};
      Console.WriteLine ("Unindo rotas");
      AtualizarMapa();

      foreach(Posicao c in cliCheck) {
        coord[0] = c.getLat(); 
        coord[1] = c.getLon();
        int auxIdOr = c.getIdOr(); 
        
        coordOr[0] = cliCheck[ cliId.IndexOf(auxIdOr) ].getLat(); 
        coordOr[1] = cliCheck[ cliId.IndexOf(auxIdOr) ].getLon(); 
        
        Conectar(coordOr[0], coordOr[1], coord[0], coord[1], cliCheck[ cliId.IndexOf(auxIdOr) ].getId(), c.getId());
      }
    }


    private double ListMin(List<double> list){
      double min = 100000;
      foreach(double l in list){
        if(l < min) { min = l; }
      }
      return min;
    }

    
    private int[] Mapa() {
      int[] ii = new int[2]; ii[0] = -1; ii[1] = -1;

      for(int i = 0; i < maxLat * 2; i++) {
        for(int j = 0; j < maxLon * 2; j++) {
          if(mapa[i,j] <= -2) { 
            ii[0] = i; 
            ii[1] = j; 
            break; 
          }
        }
      }
      return ii;
    }


    private void GerarMapa() {
      List<string> mapaRel = new List<string>();
      mapaRel.Add("\n==========ROTA DE ENTREGA==========\n");
      for(int i = 0; i < maxLat * 2; i++) {
        for(int j = 0; j < maxLon * 2; j++) {
          if(mapa[i,j] == 0) { mapaRel.Add(" "); }
          else if(mapa[i,j] <= -1) { mapaRel.Add("."); }
          else if(mapa[i,j] == indexOrigem) { mapaRel.Add("|$|"); }
          else { mapaRel.Add(String.Format("{0:0}", mapa[i,j]) ); }
        }
        mapaRel.Add("\n");
        System.Threading.Thread.Sleep(5);
      }    
      dados.mapa = mapaRel;
    }


    private bool CheckPontos(int la, int lo){
      int or1 = 0, or2 = 0, de1 = 0, de2 = 0, cont = 0;
      AtualizarCliId();
      foreach(Ponto p in pontos){
        if((p.getLat() == la) && (p.getLon() == lo)) {
          if(memoria.IndexOf(String.Format("{0:0}", p.getLat()) + String.Format("{0:0}", p.getLon())) == -1) {
            if(cont == 0) { 
              or1 = p.getIdOr();
              de1 = p.getIdDe(); 
              //Console.WriteLine("ponto cruzado: {0} {1} {2} {3} 1", p.getLat(), p.getLon(), p.getIdOr(), p.getIdDe()); 
              cont++; 
            }
            else if(cont == 1) { 
              or2 = p.getIdOr(); 
              de2 = p.getIdDe(); 
              //Console.WriteLine("ponto cruzado: {0} {1} {2} {3} 2", p.getLat(), p.getLon(), p.getIdOr(), p.getIdDe()); 
              memoria.Add(String.Format("{0:0}", p.getLat()) + String.Format("{0:0}", p.getLon()));
              cont++; 
              break;
            }
          } else {
            mapa[la, lo] = -1; 
            return true;
          }
        }
      }

      if(cont == 2) {
        int or = 0, de = 0;

        de = cliCheck[ cliId.IndexOf( de1 ) ].getIdDe();
        or = cliCheck[ cliId.IndexOf( or2 ) ].getIdOr();

        cliCheck[ cliId.IndexOf(or1) ].setIdDe( cliCheck[ cliId.IndexOf(or2) ].getId() );
        cliCheck[ cliId.IndexOf(de2) ].setIdOr( cliCheck[ cliId.IndexOf(de1) ].getId() );

        cliCheck[ cliId.IndexOf(  cliCheck[ cliId.IndexOf( or2 ) ].getIdOr() ) ].setIdDe( cliCheck[ cliId.IndexOf(de1) ].getId() );
        cliCheck[ cliId.IndexOf(  cliCheck[ cliId.IndexOf( de1 ) ].getIdDe() ) ].setIdOr( cliCheck[ cliId.IndexOf(or2) ].getId() );

        cliCheck[ cliId.IndexOf(or2) ].setIdDe( de );
        cliCheck[ cliId.IndexOf(or2) ].setIdOr( or1 );
        cliCheck[ cliId.IndexOf(de1) ].setIdOr( or );
        cliCheck[ cliId.IndexOf(de1) ].setIdDe( de2 );
        
        pontos.Clear();
        Relacionar();
        return true;
      }
      else if (cont == 1) { 
        mapa[la, lo] = -1; 
        return true;
      }
      return false;
    }


    private void Conectar(double c, double cc, double b, double bb, int ido, int idd) {
      int or = 0, de = 0, auxOr = 0, auxDe = 0, salto = 0;
      double espaco = 0;
      bool sentido;
      int[] c1 = new int[2];
      int[] c2 = new int[2];

      c1[0] = (int)c + maxLat;
      c2[0] = (int)b + maxLat;
      c1[1] = (int)cc + maxLon;
      c2[1] = (int)bb + maxLon;

      double distanciaLat = c1[0] - c2[0];
      double distanciaLon = c1[1] - c2[1];
      if(distanciaLat < 0) { distanciaLat *= -1; }
      if(distanciaLon < 0) { distanciaLon *= -1; }

      
      if(distanciaLat > distanciaLon) {
        espaco = distanciaLat / distanciaLon;
        or = c1[0]; 
        auxOr = c1[1];
        de = c2[0];
        auxDe = c2[1];
        sentido = true;
      } else {
        espaco = distanciaLon / distanciaLat;
        or = c1[1];
        auxOr = c1[0]; 
        de = c2[1];
        auxDe = c2[0];
        sentido = false;
      }

      if(or < de) {
        for(int i = or; i < de; i++) {
          if(((i - or) % espaco < 1) && (i > 0)) { 
            if(auxOr < auxDe)  { salto++; }
            else { salto--; }
          }
          if(sentido) { 
            if(mapa[i, c1[1] + salto] <= 0) {
              if((i - or > 1) && (i < de - 1)) { 
                  mapa[i, c1[1] + salto] -= 1; 
              }
              else { 
                mapa[i, c1[1] + salto] = -1; 
                pontos.Add( new Ponto(i, c1[1] + salto, ido, idd) );
              }
            } 
          } else {
            if(mapa[c1[0] + salto, i] <= 0) { 
              if((i - or > 1) && (i < de - 1)) {
                mapa[c1[0] + salto, i] -= 1; 
              }
              else { 
                mapa[c1[0] + salto, i] = -1; 
                pontos.Add( new Ponto(c1[0] + salto, i, ido, idd) );
              }
            }
          }
        } 
      } else {
        for(int i = or; i > de; i--) { 
          if(((i - de) % espaco < 1) && (i > 0)) { 
            if(auxOr < auxDe)  { salto++; }
            else { salto--; }
          }
          if(sentido) { 
            if(mapa[i, c1[1] + salto] <= 0) { 
              if((i < or - 1) && (i > de + 1)) { 
                mapa[i, c1[1] + salto] -= 1;                  
              }
              else { 
                mapa[i, c1[1] + salto] = -1; 
                pontos.Add( new Ponto(i, c1[1] + salto, ido, idd) );
              }
            } 
          } else {
            if(mapa[c1[0] + salto, i] <= 0) { 
              if((i < or - 1) && (i > de + 1)) { 
                mapa[c1[0] + salto, i] -= 1; 
              }
              else { 
                mapa[c1[0] + salto, i] = -1; 
                pontos.Add( new Ponto(c1[0] + salto, i, ido, idd) ); 
              }
            }
          }
        } 
      }
    }

  
    private void Relatorio() {
      int o = 0, d = 0, p = 0;

      AtualizarCliId();

      for(int i = 1; i <= range; i++) {
        o = cliCheck[ cliId.IndexOf(i) ].getIdOr();
        d = cliCheck[ cliId.IndexOf(i) ].getIdDe();
        p = i;
        Console.WriteLine ("Or: {0} Id: {1} De: {2}", o, p, d);
      }
      o = cliCheck[ cliId.IndexOf(indexOrigem) ].getIdOr();
      d = cliCheck[ cliId.IndexOf(indexOrigem) ].getIdDe();
      p = indexOrigem;
      Console.WriteLine ("Or: {0} Id: {1} De: {2}", o, p, d);
    }


    private void CorrigirSentido() {
      AtualizarCliId();
      
      int id1 = 0, id2 = 0;
      bool s1 = false, s2 = false;

      foreach(Posicao c in cliCheck){
        if((c.getIdOr() == c.getId()) && id1 == 0) {
          id1 = c.getId(); s1 = false;
        }
        else if((c.getIdDe() == c.getId()) && id1 == 0) {
          id1 = c.getId(); s1 = true;
        }
        else if((c.getIdOr() == c.getId()) && id1 != 0 && id2 == 0) {
          id2 = c.getId(); s2 = false;
        }
        else if((c.getIdDe() == c.getId()) && id1 != 0 && id2 == 0) {
          id2 = c.getId(); s2 = true;
        }
      } System.Threading.Thread.Sleep(10);

      if(id1 != 0 && id2 != 0) { 
        //Console.WriteLine ("Corrigindo Sentidos");
        if(s1 && !s2) {
          cliCheck[id1].setIdDe( cliCheck[id2].getId() );
          cliCheck[id2].setIdOr( cliCheck[id1].getId() );
        } else {
          cliCheck[id1].setIdOr( cliCheck[id2].getId() );
          cliCheck[id2].setIdDe( cliCheck[id1].getId() );
        } 
      }
    }

  }
  
}
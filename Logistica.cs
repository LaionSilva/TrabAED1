using System;
using System.Collections.Generic;

namespace logistica {
  public class Logistica {
    private int range;
    private int n = 1, c = 0;
    private int[] valores;
    private int[,] result;

    public DadosLog MelhorRota(List<string> nomes, List<Cliente> clientes, int r) {
      int[] rota = new int[r];
      DadosLog dados = new DadosLog();
      try{
        Console.WriteLine ("Combinando entregas...");
        Combinar(r);
        System.Threading.Thread.Sleep(1000);
        Console.WriteLine ("\nCalculando rota...");

        double[] coordOr = new double[2] {0, 0};
        double[] coordDe = new double[2] {0, 0};
        double dist = 100 * r;

        for(int i = 0; i < n; i++) {
          double auxDist = 0;
          for(int j = 0; j < r; j++) {
            coordOr = coordDe;
            coordDe = clientes[ getClientes(nomes[ result[j, i] ], clientes) ].getCoord();
            auxDist += calcularDistancia(coordOr[0], coordOr[1], coordDe[0], coordDe[1]);  
          }
          if(auxDist < dist) { 
            dist = auxDist; 
            for(int j = 0; j < r; j++) 
              { rota[j] = result[j, i]; }
          } 
          if(i == n - 1)  
            { dist += calcularDistancia(coordDe[0], coordDe[1], 0, 0); }  
        }

        //  TESTE, APAGAR DEPOIS
        Console.WriteLine();
        for(int i= 0; i < rota.Length; i++) { Console.Write("{0} ", rota[i]); }
        Console.Write(" - Dist: {0}km ", dist * 111.12);
        Console.WriteLine();

        dados.rota = rota; dados.dist = dist * 111.12;
      } catch { Console.WriteLine ("Erro: Melhor rota - log:MRo"); }
      
      return dados;
    }

    private double calcularDistancia(double lat1,double lon1, double lat2, double lon2){
      try { return Math.Sqrt( Math.Pow(lat1 - lat2, 2) + Math.Pow(lon1 - lon2, 2)); }
      catch { Console.WriteLine ("Erro: Calcular Distancia - log:CDi"); return 100; }
    }

    public int getClientes(string nome, List<Cliente> clientes){ //  Encontrar o id de algum cliente pelo nome - retorna -1 caso não encontre
      try{
        for(int i = 0; i < clientes.Count; i++){
          if(nome == clientes[i].getNome()) { return i; }
        } return -1;
      } catch { Console.WriteLine ("Erro: Get Clientes - log:GCl"); return -1; }      
    }

    private void Combinar(int i){
      try{
        range = i;
        valores = new int[range];
        for(int j = i; j > 1; j--) 
          { n *= j;  }
        result = new int[i, n];
        nFor(0); 
      } catch {  Console.WriteLine ("Erro: Combinar - log:com");  }           
    }

    private void nFor(int a){
      if(a < range){
        for (int i = 0; i < range; i++){
          valores[a] = i;
          nFor(a + 1);                    
        } 
      } else { Testar(); }
      
    }

    private void Testar(){
      bool falha = false;
      for(int i = 0; i < range - 1; i++) {
        for(int j = i + 1; j < range; j++) {
          if (valores[i] == valores[j]) { 
            falha = true; 
            j = range; 
            i = range; 
          }
        }
      }
      if(!falha) {
        for(int i = 0; i < range; i++) 
          { result[i, c] = valores[i]; }
        c++; 
      }      
    }

  }
}

using System;
using System.Collections.Generic;

namespace logistica {
  public class Rota {
    private int range;
    private int n = 1, c = 0;
    private int[] valores;
    private int[,] result;

    public int[,] Combinar(int i){
      range = i;
      valores = new int[range];
      for(int j = i; j > 1; j--) 
        { n *= j;  }
      result = new int[i, n];
      nFor(0);   
      return result;   
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

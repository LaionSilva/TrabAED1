using System;
using System.Collections.Generic;

namespace logistica {
  public class Caminhao {
    private int id; 
    private bool viajando;
    private double[] capacidade = new double[2]; // [metros cubicos, Kg]
    private double eficMotor; //  (Km/L) / peso[Kg]  
    private List<Encomenda> carga = new List<Encomenda>();
    private List<Relatorio> diarioEntregas  = new List<Relatorio>();

    public Caminhao(int i, List<Encomenda> c, double ef = 10, double mc = 150, double kg = 3000){
      id = i;      
      carga = c;
      eficMotor = ef;
      capacidade[0] = mc;
      capacidade[1] = kg;
      viajando = false;
    }
    
  }
}
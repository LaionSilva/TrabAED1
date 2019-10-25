using System;
using System.Collections.Generic;

namespace logistica {
  public class Caminhao {
    private double eficMotor; //  (Km/L) / peso[Kg]  
    //private List<Relatorio> diarioEntregas  = new List<Relatorio>();

    public Caminhao(double ef = 10){   
      eficMotor = ef;
    }
// ESTE METODO ESTOU USANDO
    public double calcularDiariaMotorista(double distanciaTotal){
      //pagar 100 reais a cada 1000 km
      return (distanciaTotal/1000)*150;
    }
//

    //SETS
    public void setEficiencia(double ef){
      eficMotor = ef;
    }

    //Gets 
    public double getEficiencia(){
      return eficMotor;
    }
  }
}
using System;
using System.Collections.Generic;

namespace logistica {
  public class Caminhao {
    private int id; 
    private bool viajando;
    private double tanqueCombustivel;
    private double capacidadeTanque;
    private double[] capacidade = new double[2]; // [metros cubicos, Kg]
    private double eficMotor; //  (Km/L) / peso[Kg]  
    private List<Encomenda> carga = new List<Encomenda>();
    private List<Relatorio> diarioEntregas  = new List<Relatorio>();

    public Caminhao(int i, double ef = 10, double mc = 150, double kg = 3000){
      id = i;      
      eficMotor = ef;
      capacidade[0] = mc;
      capacidade[1] = kg;
      viajando = false;
      capacidadeTanque = 300.0;
      tanqueCombustivel = 0.0;
    }
// ESTE METODO ESTOU USANDO
    public double calcularDiariaMotorista(double distanciaTotal){
      //pagar 100 reais a cada 1000 km
      return (distanciaTotal/1000)*100;
    }
//
    public bool Abastecer(double q){
      if(q< capacidadeTanque - tanqueCombustivel){
        tanqueCombustivel+= q;
        return true;
      }else{
        return false;
      }
    }

    public bool carregar(double mc, double kg){
      if(!viajando){
        if( (mc <= capacidade[0]) && (kg <= capacidade[1]) ){
          return true;
        }else{
          return false;
        }
      }
      return false;
    }

    public bool Viajar(){
      viajando = true;
      return true;
    }
    //Métodos: Carregar(encomendas), Descarregar(pedido), Viajar, VerificarIntegridade(Manutenção)

    //SETS
    public void setEficiencia(double ef){
      eficMotor = ef;
    }

    //Gets 
    public double getEficiencia(){
      return eficMotor;
    }

    public bool getViajando(){
      return viajando;
    }
  }
}
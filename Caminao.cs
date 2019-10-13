using System;
using System.Collections.Generic;

namespace logistica {
  public class Caminhao {
    private int id; 
    private bool viajando;
    private double tanque; //  litros
    private double[] capacidade = new double[2]; // [metros cubicos, Kg]
    private double eficMotor; //  (Km/L) / peso[Kg]
    private List<Encomenda> carga;
    private List<Relatorio> diarioEntregas;

    //  TODO: Construtor faltando
    //  TODO: Métodos faltando
  }
}
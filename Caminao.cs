using System;
using System.Collections.Generic;

namespace logistica {
  public static class Caminhao {
    private static double precoDiesel = 3.75;
    private static double coefDiariaMotorista = 150/1000;
    public const double eficMotor = 10; //  (Km/L) / peso[Kg] 
    public const double volumeBau = 600; //  (m^3) capacidade máx
    public const double pesoBau = 10000; //  10mil kg capacidade máx

    public static double CalcularDiariaMotorista(double distanciaTotal) {
      //pagar 100 reais a cada 1000 km
      return distanciaTotal*Caminhao.coefDiariaMotorista;
    }

    public static double CalcularGastoCombustivel(double distanciaTotal) {
      return Caminhao.precoDiesel*distanciaTotal;
    }
    
    public static double CalcularDistancia(double lat1, double lon1, double lat2, double lon2) {
      double x1 = lat1*111.12;
      double x2 = lat2*111.12;
      double y1 = lon1*111.12 * ( (50 - Math.Sqrt( Math.Pow(lat1, 2) )) / 50 );
      double y2 = lon2*111.12 * ( (50 - Math.Sqrt( Math.Pow(lat2, 2) )) / 50 );

      return Math.Sqrt( Math.Pow(x1 - y1, 2) + Math.Pow(x2 - y2, 2));
    }
    public static double volumePercent(double volume, int containers) {
      return (volume * 100) / (Caminhao.volumeBau * containers);
    }
    public static double pesoPercent(double peso, int containers) {
      return (peso * 100) / (Caminhao.pesoBau * containers);
    }
  }
}
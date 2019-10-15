using System;
using System.Collections.Generic;
using System.IO;

namespace logistica {
  public class Save {

    private void Carregar(string file){
      using(Stream FileIn = File.Open(file, FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          //suporte = Carregar.ReadLine();
        }
      }
    }

    private void Guardar(string file, string dado){
      using (StreamWriter Salvar = File.AppendText(file)) {  
        //Salvar.WriteLine(dado); 
      }
    }

  }
}
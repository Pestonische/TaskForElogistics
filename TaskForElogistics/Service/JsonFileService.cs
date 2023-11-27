using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using TaskForElogistics.Model;
using TaskForElogistics.Service.Interface;

namespace TaskForElogistics.Service
{
    /*
    * Класс реализующий интерфейс IFileService
    */
    public class JsonFileService : IFileService
    {
        public T OpenFile<T>(string filename, T rates)
        {
            
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(T));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                rates = (T)jsonFormatter.ReadObject(fs);
            }

            return rates;
        }

        public void SaveFile<T>(string filename, T ratesList)
        {
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(T));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, ratesList);
            }
        }

    }
}

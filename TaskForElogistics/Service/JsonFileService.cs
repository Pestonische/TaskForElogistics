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
        public ObservableCollection<Rate> OpenFile(string filename)
        {
            ObservableCollection<Rate> rates = new ObservableCollection<Rate>();
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(ObservableCollection<Rate>));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                rates = jsonFormatter.ReadObject(fs) as ObservableCollection<Rate>;
            }

            return rates;
        }

        public void SaveFile(string filename, ObservableCollection<Rate> ratesList)
        {
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(ObservableCollection<Rate>));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, ratesList);
            }
        }

    }
}

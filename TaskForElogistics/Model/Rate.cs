using System.ComponentModel;

namespace TaskForElogistics.Model
{
    /*
     * Класс для хранения курсов валют в формате для записей в Json файл
    */
    public class Rate : INotifyPropertyChanged
    {
        // дата, на которую запрашивается курс
        public System.DateTime Date { get; set; }

        // буквенный код
        public string Cur_Abbreviation { get; set; }

        // наименование валюты на русском языке
        public string Cur_Name { get; set; }

        // курс
        private decimal? _cur_OfficialRate; 

        /*
        * Поле реализует интерфейс INotifyPropertyChanged,
        * для динамического изменения данных на графике
        */
        public decimal? Cur_OfficialRate {
            get
            {
                if (_cur_OfficialRate == null)
                {
                    return null;
                }

                return _cur_OfficialRate;
            }
            set
            {
                _cur_OfficialRate = value;
                NotifyPropertyChanged("CurrenciesName");
            }
        }

        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

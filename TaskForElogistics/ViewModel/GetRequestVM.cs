using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using TaskForElogistics.Model;
using TaskForElogistics.Service.Interface;

namespace TaskForElogistics.ViewModel
{
    public class GetRequestVM : INotifyPropertyChanged
    {

        #region Допалнительные статические переменные 

        // дата начала периода
        private static DateTime _startDate; 

        // дата конца периода
        private static DateTime _endData; 

        // все валюты
        private static ObservableCollection<Currency> _currencies; 

        // валюты за период
        private static ObservableCollection<Currency> _currenciesForPeriod; 

        // результат запроса курсов валют
        private static ObservableCollection<RateShort> _ratesShort; 

        // элемент управления сохранением и удалением файлов
        IFileService fileService;   

        // элемент управления диалоговыми окнами
        IDialogService dialogService; 

        // имя файла с валютами
        private const string NameCurrencyFile = "Currency.json"; 

        // максимальный период для запроса
        private static int maxTimePeriod = 365;

        #endregion

        #region Связанные данные 

        //Список возможных валют за период
        private static List<String> _currenciesName;

        public List<String> CurrenciesName
        {
            get
            {
                if (_currenciesName == null)
                {
                    return null;
                }

                return _currenciesName;
            }
            private set
            {
                _currenciesName = value;
                NotifyPropertyChanged("CurrenciesName");
            }
        }

        //Курсы валют
        private static ObservableCollection<Rate> _rates;

        public ObservableCollection<Rate> Rates
        {
            get
            {
                if (_rates == null)
                {
                    return null;
                }

                return _rates;
            }
            private set
            {
                _rates = value;
                NotifyPropertyChanged("Rates");
            }
        }

        #endregion

        #region Вспомогательные функции

        // Заполняет CurrenciesName именами валют за период
        private void GetCurrenciesName()
        {
            List<String> result = new List<string>();
            ObservableCollection<Currency> currenciesForPeriod = new ObservableCollection<Currency>();

            foreach (Currency currency in _currencies)
            {
                if((currency.Cur_DateStart <= _startDate && _startDate <= currency.Cur_DateEnd) ||
                    (currency.Cur_DateStart <= _endData && _endData <= currency.Cur_DateEnd))
                {
                    result.Add($"{currency.Cur_Name} ({currency.Cur_Abbreviation})");
                    currenciesForPeriod.Add(currency);
                }
            }

            _currenciesForPeriod = currenciesForPeriod;
            CurrenciesName = result.Union(result).OrderBy(a=>a).ToList();            
        }

        // Заполняет Rates курсами валют
        private void GetRatesList()
        {
            ObservableCollection<Rate> rates = new ObservableCollection<Rate>();

            foreach (RateShort rateShort in _ratesShort)
            {
                rates.Add(new Rate()
                {
                    Date = rateShort.Date,
                    Cur_Abbreviation = _currencies.Where(a => a.Cur_ID == rateShort.Cur_ID)
                                            .Select(x => x.Cur_Abbreviation).FirstOrDefault(),
                    Cur_Name = _currencies.Where(a => a.Cur_ID == rateShort.Cur_ID).Select(x => x.Cur_Name).FirstOrDefault(),
                    Cur_OfficialRate = rateShort.Cur_OfficialRate
                });
            }

            Rates = rates;
        }

        // Возвращает id валюты
        private List<int> GetRateId(ObservableCollection<Currency> currencies, string rateName)
        {
            List<int> rateId = currencies.Where(a => a.Cur_Abbreviation == rateName).Select(x => x.Cur_ID).ToList();

            return rateId;
        }

        // Возвращает имя валюты
        private string GetRateName(string rateName)
        {
            List<string> partsOfRateName = rateName.Split('(', ')').ToList();
            rateName = partsOfRateName[partsOfRateName.Count - 2];

            return rateName;
        }

        // Возврат stream
        private Stream GetStream(string requestString)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                return response.GetResponseStream();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
                return null;
            }
        }

        #endregion

        #region Реализация Commands
        // Выполняет запрос на получение валют
        private void GetAllСurrencies(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endData = endDate;

            try
            {
                if (_currencies == null)
                {                   
                    if (File.Exists(NameCurrencyFile))
                    {
                        (DateTime s, ObservableCollection<Currency> cur) file = (DateTime.Now, new ObservableCollection<Currency>());
                        file = fileService.OpenFile(NameCurrencyFile, file);

                        if (DateTime.Compare(DateTime.Today, file.s) == 0)
                        {
                            _currencies = file.cur;
                            GetCurrenciesName();
                            return;
                        }
                    }

                    var stream = GetStream("https://api.nbrb.by/exrates/currencies") ;

                    if (stream != null) _currencies = JsonConvert.DeserializeObject<ObservableCollection<Currency>>(
                                                        new StreamReader(stream).ReadToEnd().ToString());

                    fileService.SaveFile(NameCurrencyFile, (DateTime.Today, _currencies));
                }
                else GetCurrenciesName();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }
        
        // Выполняет запрос на получение курсов валют 
        private void GetRateRequest(string rateName)
        {
            rateName = GetRateName(rateName);
            List<int> rateId = GetRateId(_currenciesForPeriod, rateName);
            List<RateShort> ratesShort = new List<RateShort>();

            try
            {   
                foreach (var rate in rateId)
                {                    
                    var stream = GetStream($"https://api.nbrb.by/ExRates/Rates/Dynamics/{rate}?startDate={_startDate.ToString("yyyy-MM-dd")}&endDate={_endData.ToString("yyyy-MM-dd")}") ;
                    
                    if (stream != null) ratesShort.AddRange(
                        JsonConvert.DeserializeObject<List<RateShort>>(new StreamReader(stream).ReadToEnd().ToString()));                    
                }

                _ratesShort = new ObservableCollection<RateShort>(ratesShort);
                GetRatesList();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        // Сохраняет данные в файл
        private void Save()
        {
            try
            {
                if (dialogService.SaveFileDialog() == true)
                {
                    fileService.SaveFile(dialogService.FilePath, Rates);
                    dialogService.ShowMessage("Файл сохранен");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        // Открывает данные из файла
        private void Open()
        {
            try
            {
                if (dialogService.OpenFileDialog() == true)
                {
                    Rates = fileService.OpenFile(dialogService.FilePath, Rates);
                    dialogService.ShowMessage("Файл открыт");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        #endregion

        #region Commands для кнопок

        //Запрос списка валют
        private RelayCommand _search;
        public RelayCommand Search
        {
            get
            {
                return _search ?? new RelayCommand(obj =>
                {
                    if (obj != null)
                    {
                        (DateTime Start, DateTime End) values = ((DateTime Start, DateTime End))obj;
                        if (values.Start > values.End)
                        {
                            dialogService.ShowMessage("Дата начала не может быть больше даты конца");
                        }
                        else if ((values.End - values.Start).TotalDays > maxTimePeriod)
                        {
                            dialogService.ShowMessage("Превышен диапазон периода времени");
                        }
                        else GetAllСurrencies(values.Start, values.End);
                    }
                    else dialogService.ShowMessage("Неверные данные");

                });

            }
        }

        //Запрос списка курса валют за период
        private RelayCommand _getRates;
        public RelayCommand GetRates
        {
            get
            {
                return _getRates ?? new RelayCommand(obj =>
                {
                    if (obj != null)
                    {
                        string str = obj.ToString();
                        GetRateRequest(str);
                    }
                    else dialogService.ShowMessage("Неверные данные");

                });

            }
        }

        //Сохранение изменений
        private RelayCommand _saveData;
        public RelayCommand SaveData
        {
            get
            {
                return _saveData ?? new RelayCommand(obj =>
                {
                    Save();
                });

            }
        }

        //Открыть сохраненные данные 
        private RelayCommand _openFile;
        public RelayCommand OpenFile
        {
            get
            {
                return _openFile ?? new RelayCommand(obj =>
                {                    
                    Open();
                });

            }
        }
        #endregion

        // Конструктор класса
        public GetRequestVM(IDialogService dialogService, IFileService fileService )
        {
            this.fileService = fileService;
            this.dialogService = dialogService;
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

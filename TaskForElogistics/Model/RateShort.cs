using System;

namespace TaskForElogistics.Model
{
    /*
     * Класс для хранения данных запроса курсов валют
    */
    public class RateShort
    {
        // внутренний код
        public int Cur_ID { get; set; }

        // дата, на которую запрашивается курс
        public System.DateTime Date { get; set; }

        // курс
        public decimal? Cur_OfficialRate { get; set; } 
    }
}

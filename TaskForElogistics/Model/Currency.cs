using System;

namespace TaskForElogistics.Model
{
    /*
     * Класс для хранения всех возможных валют
    */
    public class Currency
    {
        // внутренний код
        public int Cur_ID { get; set; }

        // код для связи с прошлыми значениями
        public Nullable<int> Cur_ParentID { get; set; }

        // цифровой код
        public string Cur_Code { get; set; }

        // буквенный код
        public string Cur_Abbreviation { get; set; }

        // наименование валюты на русском языке
        public string Cur_Name { get; set; }

        // наименование на белорусском языке
        public string Cur_Name_Bel { get; set; }

        // наименование на английском языке
        public string Cur_Name_Eng { get; set; }

        // наименование валюты на русском языке, содержащее количество единиц
        public string Cur_QuotName { get; set; }

        // наименование на белорусском языке, содержащее количество единиц
        public string Cur_QuotName_Bel { get; set; }

        // наименование на английском языке, содержащее количество единиц
        public string Cur_QuotName_Eng { get; set; }

        // наименование валюты на русском языке во множественном числе
        public string Cur_NameMulti { get; set; }

        // наименование валюты на белорусском языке во множественном числе
        public string Cur_Name_BelMulti { get; set; }

        // наименование на английском языке во множественном числе
        public string Cur_Name_EngMulti { get; set; }

        // количество единиц иностранной валюты
        public int Cur_Scale { get; set; }

        // периодичность установления курса (0 – ежедневно, 1 – ежемесячно)
        public int Cur_Periodicity { get; set; }

        /*
         * дата включения валюты в перечень валют, к которым
         * устанавливается официальный курс бел. рубля
        */
        public System.DateTime Cur_DateStart { get; set; }

        /*
         * дата исключения валюты из перечня валют, к которым
         * устанавливается официальный курс бел. рубля
         */
        public System.DateTime Cur_DateEnd { get; set; } 
                                                         
    }
}

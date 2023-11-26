using System;

namespace TaskForElogistics.Service.Interface
{
    /*
     * Интерфейс для DialogService
    */
    public interface IDialogService
    {
        // показ сообщения
        void ShowMessage(string message);

        // путь к выбранному файлу
        string FilePath { get; set; }

        // открытие файла
        bool OpenFileDialog();

        // сохранение файла
        bool SaveFileDialog();  
    }
}

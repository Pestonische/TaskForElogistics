using System;
using System.Collections.ObjectModel;
using TaskForElogistics.Model;

namespace TaskForElogistics.Service.Interface
{
    /*
     * Интерфейс для IFileService
    */
    public interface IFileService
    {
        // функция открытия файла
        ObservableCollection<Rate> OpenFile(string filename);

        // функция сохранения файла
        void SaveFile(string filename, ObservableCollection<Rate> rateList); 
    }
}

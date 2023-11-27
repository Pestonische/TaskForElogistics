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
        T OpenFile<T>(string filename, T rates);

        // функция сохранения файла
        void SaveFile<T>(string filename, T rateList); 
    }
}

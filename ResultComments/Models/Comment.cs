using System;
using System.Collections.Generic;
using ResultComments.Helpers;

namespace ResultComments.Models
{
    /// <summary>
    /// Этот класс является предком для класса ругательств
    /// Здесь определены все методы, а так же хэши комментариев
    /// В дочернем классе переопределены именно хэши
    /// </summary>
    public class Comment
    {

        protected List<string> _hash_100;
        protected List<string> _hash_99_90;
        protected List<string> _hash_89_75;
        protected List<string> _hash_74_60;
        protected List<string> _hash_59_50;
        protected List<string> _hash_49;

        public Comment()
        {
            InitiateHashes();
        }

        /// <summary>
        /// Функция возвращает рандомный элемент из массива комментариев
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public string Get(double res)
        {

            if (res >= 100) return GetRandomSwear(_hash_100);

            if (res >= 90 && res < 100) return GetRandomSwear(_hash_99_90);

            if (res >= 75 && res < 90) return GetRandomSwear(_hash_89_75);

            if ((res >= 60) && (res < 75)) return GetRandomSwear(_hash_74_60);

            if ((res >= 50) && (res < 60)) return GetRandomSwear(_hash_59_50);

            return GetRandomSwear(_hash_49);
        }

        protected virtual string GetRandomSwear(List<string> hash)
        {
            return hash[GetRandomNumber(hash.Count)];
        }

        protected virtual int GetRandomNumber(int count)
        {
            return new Random().Next(count);
        }

        /// <summary>
        /// Инициализация массивов, которая перезаписывается в дочерних элменетах
        /// </summary>
        protected virtual void InitiateHashes()
        {
            _hash_100 = new List<string>
            {
                "Молодец! Так держать! Эта сессия - ничто для тебя!"
            };
            //---------------------------------
            _hash_99_90 = new List<string>
            {
                "Круто! Еще немного подготовки - и стольник тебе обеспечен!"
            };
            //---------------------------------
            _hash_89_75 = new List<string>
            {
                "Я знаю, ты можешь лучше"
            };
            //---------------------------------
            _hash_74_60 = new List<string>
            {
                "Ну ничего, в другой раз повезет"
            };
            _hash_59_50 = new List<string>
            {
                "Слабовато, но стоит тебе подготовиться, и оценка будет выше!"
            };
            //--------------------------------
            _hash_49 = new List<string>
            {
                "Ну как же так? Это ведь легкий тест"
            };
        }
    }
}
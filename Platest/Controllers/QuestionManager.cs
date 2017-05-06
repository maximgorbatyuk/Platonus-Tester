using System;
using System.Collections.Generic;
using System.Linq;
using Platest.Helpers;
using Platest.Interfaces;
using Platest.Models;

namespace Platest.Controllers
{
    /// <summary>
    /// Класс для обработки массива готовых тестовых вопросов
    ///
    /// </summary>
    public class QuestionManager : IQuestionManager
    {
        private List<TestQuestion> _list;

        

        private int _firstListCount;

        private readonly IQuestionProvider _questionProvider;

        private int _currentIndex;

        private int _limit;

        public QuestionManager()
        {
            _list = new List<TestQuestion>();
            _questionProvider = new QuestionProcessor();
        }

        public void SetSourceList(SourceFile file)
        {
            _list = _questionProvider.GetQuestionList(file);
            _limit = _list.Count;
            _currentIndex = 0;
            _firstListCount = _list.Count;
            Shuffle();
        }

        /// <summary>
        /// Перемешение вопросов в массиве.
        /// </summary>
        public void Shuffle()
        {
            var count = _list.Count;
            var result = new List<TestQuestion>(0);
            for (var i = 0; i < count; i++)
            {
                var q = _list[new Random(DateTime.Now.Millisecond + i).Next(_list.Count)];
                result.Add(q);
                _list.Remove(q);
            }
            _list = result;
        }

        /// <summary>
        /// Возвращает следующий вопрос. Раньше (до перевода на WPF 04.06.2016) метод удалял
        /// возвращаемый вопрос. Сейчас принцип выдачи изменен, так как в планах
        /// сделать переход по предыдущим вопросам
        /// </summary>
        /// <returns></returns>
        public TestQuestion GetNext()
        {
            if (_currentIndex >= _list.Count || _currentIndex >= _limit)
            {
                return null;
            }
            var question = _list[_currentIndex];
            _currentIndex++;
            return question;
        }

        public int GetCount() => _list.Count;

        public int GetFirstListCount() => _firstListCount;

        public void SetQuestionLimit(int limit)
        {
            _limit = limit > _list.Count? _list.Count : limit;
        }

        public int GetCurrentPosition() => _currentIndex;

        public IEnumerable<string> GetErrors() => _questionProvider?.GetErrors();
    }
}
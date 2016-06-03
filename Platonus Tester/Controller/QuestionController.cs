using System;
using System.Collections.Generic;
using Platonus_Tester.Helper;
using Platonus_Tester.Model;

namespace Platonus_Tester.Controller
{
    public class QuestionController
    {
        private List<TestQuestion> _list;
        public List<string> Errors { get; private set; }
        private int _firstListCount;
        private readonly QuestionProcessor _questionProcessor;
        private int _currentIndex;
        private int _limit;

        public QuestionController()
        {
            _list = new List<TestQuestion>(0);
            _questionProcessor = new QuestionProcessor();
        }

        public void SetSourceList(SourceFile file)
        {
            _list = _questionProcessor.GetQuestionList(file);
            _limit = _list.Count;
            _currentIndex = 0;
            _firstListCount = _list.Count;
            Errors = _questionProcessor.GetErrors();
            Shuffle();
        }

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
    }
}
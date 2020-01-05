using Platest.Helpers;
using Platest.Models;
using Xunit;
using System.Collections.Generic;
using System.IO;
using System;

namespace PlatestTest
{
    public class QuestionProcessorTest
    {

        [Fact]
        public void GetQuestionListResultTest()
        {
            var controller = new QuestionProcessor();

            // Act
            var result = controller.GetQuestionList(SourceFile());

            // Arrange
            Assert.IsType<List<TestQuestion>>(result);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetQuestionResultTest()
        {
            var controller = new QuestionProcessor();

            // Act
            var result = controller.GetQuestion(SourceFile().SourceText);

            // Arrange
            Assert.IsType<TestQuestion
                >(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetQuestionResultMissingVariantTest()
        {
            var controller = new QuestionProcessor();
            int variantCount = 0;
            // Act
            var result = controller.GetQuestionList(SourceFile());
            foreach (var res in result)
            {
                variantCount = res.AnswerList.Count;
                // Arrange
                Assert.Equal(5, variantCount);
            }
        }


        public SourceFile SourceFile()
        {

          
            var text = File.ReadAllText(@"C:\Users\Gaukhar\source\repos\Platonus-Tester\PlatestTest\test.txt"); 

            var sf = new SourceFile(text, null, null);
            return sf;
        }
    }
}

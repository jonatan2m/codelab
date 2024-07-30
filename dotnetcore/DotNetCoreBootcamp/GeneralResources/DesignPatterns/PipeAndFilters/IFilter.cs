using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.DesignPatterns.PipeAndFilters
{
    public interface IFilter
    {
        public string Process(string input);
    }

    public class LowerCaseFilter : IFilter
    {
        public string Process(string input)
        {
            return input.ToLower();
        }
    }

    public class RemoveStopWordsFilter : IFilter
    {
        private static readonly HashSet<string> StopWords = new()
    {
        "a", "an", "and", "are", "as", "at", "be", "but", "by", "my", "not", "of", "on", "or", "the", "to"
    };

        public string Process(string input)
        {
            var words = input.Split(new[] { " ", "\t", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            var filterWords = words.Where(x => !StopWords.Contains(x));

            return string.Join(" ", filterWords);
        }
    }

    public class SentimentFilter : IFilter
    {
        public string Process(string input)
        {
            var positiveWords = new HashSet<string>
        {
            "good", "great", "awesome", "fantastic", "happy", "love", "like"
        };

            var negativeWords = new HashSet<string>
        {
            "bad", "terrible", "awful", "hate", "dislike", "sad"
        };

            var words = input.Split(new[] { " ", "\t", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            var positiveCount = words.Count(x => positiveWords.Contains(x));
            var negativeCount = words.Count(x => negativeWords.Contains(x));

            if (positiveCount > negativeCount) return "Positive";

            return negativeCount > positiveCount ? "Negative" : "Neutral";
        }
    }

    public static class SentimentAnalyzerPipe
    {
        public static string Analyze(string text)
        {
            IFilter[] sentimentPipeLine =
            {
            new LowerCaseFilter(),
            new RemoveStopWordsFilter(),
            new SentimentFilter()
        };

            return sentimentPipeLine.Aggregate(text, (current, filter) => filter.Process(current));
        }
    }


    //var positiveSentiment = SentimentAnalyzerPipe.Analyze("I am happy");
    //var negativeSentiment = SentimentAnalyzerPipe.Analyze("I am sad");
    //var neutralSentiment = SentimentAnalyzerPipe.Analyze("I am ok");
}

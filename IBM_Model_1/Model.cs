using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBM_Model_1
{
    public class Model
    {
        string[] distinctPolishWords;

        string[] distinctEnglishWords;

        string[] englishSentences;

        string[] polishSentences;

        //iterations till convergence
        int iterations;

        double initialProbability;

        double[,] t;

        public Model(string polishFilePath, string englishFilePath, int iterations)
        {
            this.iterations = iterations;
            englishSentences = ReadSentencesFromFile(englishFilePath);
            polishSentences = ReadSentencesFromFile(polishFilePath);
            distinctEnglishWords = ReadDistinctWordsFromSentences(englishSentences, false);
            distinctPolishWords = ReadDistinctWordsFromSentences(polishSentences, true);
            initialProbability = 1.0 / distinctPolishWords.Length;
            t = new double[distinctPolishWords.Length, distinctEnglishWords.Length];
            
        }

        private string[] ReadDistinctWordsFromSentences(string[] sentences, bool isPolish)
        {
            var file = string.Join(" ", sentences);
            var words = file.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var distinct = words.Distinct();
            if (isPolish)
            {
                var list = distinct.ToList();
                list.Add("NULL");
                return list.ToArray();
            }
            return distinct.ToArray();
        }

        private string[] ReadSentencesFromFile(string polishFilePath)
        {
            return File.ReadAllLines(polishFilePath);
        }

        public void Train()
        {
            for (int i = 0; i < distinctPolishWords.Length; i++)
            {
                for (int j = 0; j < distinctEnglishWords.Length; j++)
                {
                    t[i, j] = initialProbability;
                }
            }

            double[] s_total = new double [distinctPolishWords.Length];
            for (int i=0; i<iterations; i++)
            {
                double[,] count_e_f = new double[distinctPolishWords.Length, distinctEnglishWords.Length];
                double[] total_f = new double[distinctEnglishWords.Length];

                for(int j=0; j<englishSentences.Length; j++)
                {
                    string[] englishWords = englishSentences[j].Split(' ');
                    string[] polishWords = ("NULL " + polishSentences[j]).Split(' ');

                    for(int k=0; k<polishWords.Length; k++)
                    {
                        var polishIndex = Array.IndexOf(distinctPolishWords, polishWords[k]);
                        s_total[polishIndex] = 0;

                        for(int l=0; l<englishWords.Length; l++)
                        {
                            var englishIndex = Array.IndexOf(distinctEnglishWords, englishWords[l]);
                            s_total[polishIndex] += t[polishIndex, englishIndex];
                        }
                    }

                    for (int k = 0; k < polishWords.Length; k++)
                    {
                        var polishIndex = Array.IndexOf(distinctPolishWords, polishWords[k]);
                        for (int l = 0; l < englishWords.Length; l++)
                        {
                            var englishIndex = Array.IndexOf(distinctEnglishWords, englishWords[l]);
                            var value = t[polishIndex, englishIndex] / s_total[polishIndex];
                            count_e_f[polishIndex, englishIndex] += value;
                            total_f[englishIndex] += value;
                        }
                    }
                }

                for(int j = 0; j < distinctEnglishWords.Length; j++)
                {
                    for (int k = 0; k < distinctPolishWords.Length; k++)
                    {
                        t[k, j] = count_e_f[k, j] / total_f[j];
                    }
                }
            }
        }
        public string getTranslation(string source_sentence)
        {
            var words = source_sentence.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] translation = new string[words.Count()];

            for (int i = 0; i < words.Count(); i++)
            {
                int e = -1;
                for (int j = 0; j < distinctEnglishWords.Count(); j++)
                {
                    if (distinctEnglishWords[j] == words[i])
                    {
                        e = j;
                        break;
                    }
                }
                if (e == -1) continue;

                int f = 0;
                double max = 0;
                for (int j = 0; j < distinctPolishWords.Count(); j++)
                {
                    if (t[j, e] > max)
                    {
                        max = t[j, e];
                        f = j;
                    }    
                }
                translation[i] = distinctPolishWords[f];
            }

            string translationString = "";
            for (int i = 0; i < translation.Count(); i++) translationString += translation[i] + " ";
            return translationString.Substring(0, translationString.Length-1);
        }

    }
}

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
            initialProbability = 1.0 / distinctEnglishWords.Length;
            t = new double[distinctEnglishWords.Length, distinctPolishWords.Length];
            for (int i = 0; i < distinctEnglishWords.Length; i++)
            {
                for (int j = 0; j < distinctPolishWords.Length; j++)
                {
                    t[i, j] = initialProbability;
                }
            }
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
            double[] s_total = new double [distinctEnglishWords.Length];
            for (int i=0; i<iterations; i++)
            {
                double[,] count_e_f = new double[distinctEnglishWords.Length, distinctPolishWords.Length];
                double[] total_f = new double[distinctPolishWords.Length];

                for(int j=0; j<englishSentences.Length; j++)
                {
                    string[] englishWords = englishSentences[j].Split(' ');
                    string[] polishWords = ("NULL " + polishSentences[j]).Split(' ');

                    for(int k=0; k<englishWords.Length; k++)
                    {
                        var englishIndex = Array.IndexOf(distinctEnglishWords, englishWords[k]);
                        s_total[englishIndex] = 0;

                        for(int l=0; l<polishWords.Length; l++)
                        {
                            var polishIndex = Array.IndexOf(distinctPolishWords, polishWords[l]);
                            s_total[englishIndex] += t[englishIndex, polishIndex];
                        }
                    }

                    for (int k = 0; k < englishWords.Length; k++)
                    {
                        var englishIndex = Array.IndexOf(distinctEnglishWords, englishWords[k]);
                        for (int l = 0; l < polishWords.Length; l++)
                        {
                            var polishIndex = Array.IndexOf(distinctPolishWords, polishWords[l]);
                            var value = t[englishIndex, polishIndex] / s_total[englishIndex];
                            count_e_f[englishIndex, polishIndex] += value;
                            total_f[polishIndex] += value;
                        }
                    }
                }

                for(int j = 0; j < distinctEnglishWords.Length; j++)
                {
                    for (int k = 0; k < distinctPolishWords.Length; k++)
                    {
                        t[j, k] = count_e_f[j, k] / total_f[k];
                    }
                }
            }
        }
    }
}

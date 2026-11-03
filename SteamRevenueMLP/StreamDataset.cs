using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SteamRevenueMLP
{
    public class SteamDataset
    {
        class Row
        {
            public double Price, Playtime, Review, Revenue;
            public string PublisherClass;
        }

        public static readonly string[] FeatureNames =
        {
            "price", "isFree", "log(avgPlaytime)", "reviewScore", "noReview", "isAAA", "isAA"
        };

        const double TrainShare = 0.70;
        const double ValShare = 0.15;

        public double[][] TrainX, ValX, TestX;
        public double[] TrainY, ValY, TestY;
        public double RevenueThreshold;
        public int RowCount, SkippedCount, NoReviewCount, FreeCount;

        double minPrice, maxPrice, minPlay, maxPlay, minReview, maxReview, meanReview;

        public int InputCount
        {
            get { return FeatureNames.Length; }
        }

        public void Load(string path)
        {
            string[] lines = File.ReadAllLines(path);
            if (lines.Length < 2) throw new InvalidDataException("The file has no data rows.");

            List<string> header = SplitCsv(lines[0]);
            int iPrice = Column(header, "price");
            int iPlay = Column(header, "avgPlaytime");
            int iReview = Column(header, "reviewScore");
            int iRevenue = Column(header, "revenue");
            int iClass = Column(header, "publisherClass");
            int last = Math.Max(Math.Max(iPrice, iPlay), Math.Max(Math.Max(iReview, iRevenue), iClass));

            List<Row> rows = new List<Row>();
            SkippedCount = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].Trim().Length == 0) continue;
                List<string> f = SplitCsv(lines[i]);
                Row r = new Row();
                if (f.Count <= last ||
                    !Num(f[iPrice], out r.Price) || !Num(f[iPlay], out r.Playtime) ||
                    !Num(f[iReview], out r.Review) || !Num(f[iRevenue], out r.Revenue) ||
                    r.Price < 0 || r.Playtime < 0 || r.Revenue < 0)
                {
                    SkippedCount++;
                    continue;
                }
                r.PublisherClass = f[iClass].Trim();
                rows.Add(r);
            }
            if (rows.Count < 20) throw new InvalidDataException("Not enough valid rows.");

            RowCount = rows.Count;
            NoReviewCount = 0;
            FreeCount = 0;
            foreach (Row r in rows)
            {
                if (r.Review <= 0) NoReviewCount++;
                if (r.Price == 0) FreeCount++;
            }
            Prepare(rows.ToArray());
        }

        void Prepare(Row[] all)
        {
            Shuffle(all, new Random(42));
            int nTrain = (int)(all.Length * TrainShare);
            int nVal = (int)(all.Length * ValShare);
            int nTest = all.Length - nTrain - nVal;

            minPrice = minPlay = minReview = double.MaxValue;
            maxPrice = maxPlay = maxReview = double.MinValue;
            double reviewSum = 0;
            int reviewCount = 0;
            double[] revenues = new double[nTrain];
            for (int i = 0; i < nTrain; i++)
            {
                Row r = all[i];
                revenues[i] = r.Revenue;
                minPrice = Math.Min(minPrice, r.Price);
                maxPrice = Math.Max(maxPrice, r.Price);
                double play = Math.Log(1 + r.Playtime);
                minPlay = Math.Min(minPlay, play);
                maxPlay = Math.Max(maxPlay, play);
                if (r.Review > 0)
                {
                    minReview = Math.Min(minReview, r.Review);
                    maxReview = Math.Max(maxReview, r.Review);
                    reviewSum += r.Review;
                    reviewCount++;
                }
            }
            meanReview = reviewCount > 0 ? reviewSum / reviewCount : 0;
            Array.Sort(revenues);
            RevenueThreshold = revenues[nTrain / 2];

            Build(all, 0, nTrain, out TrainX, out TrainY);
            Build(all, nTrain, nVal, out ValX, out ValY);
            Build(all, nTrain + nVal, nTest, out TestX, out TestY);
        }

        void Build(Row[] all, int start, int count, out double[][] x, out double[] y)
        {
            x = new double[count][];
            y = new double[count];
            for (int i = 0; i < count; i++)
            {
                Row r = all[start + i];
                x[i] = Features(r);
                y[i] = r.Revenue >= RevenueThreshold ? 1 : 0;
            }
        }

        double[] Features(Row r)
        {
            bool noReview = r.Review <= 0;
            double review = noReview ? meanReview : r.Review;
            return new double[]
            {
                Scale(r.Price, minPrice, maxPrice),
                r.Price == 0 ? 1 : 0,
                Scale(Math.Log(1 + r.Playtime), minPlay, maxPlay),
                Scale(review, minReview, maxReview),
                noReview ? 1 : 0,
                r.PublisherClass == "AAA" ? 1 : 0,
                r.PublisherClass == "AA" ? 1 : 0
            };
        }

        public List<string> Summary()
        {
            List<string> s = new List<string>();
            s.Add($"Rows loaded: {RowCount} (skipped {SkippedCount})");
            s.Add($"Split: train {TrainX.Length}, validation {ValX.Length}, test {TestX.Length}");
            s.Add($"Target: revenue >= {RevenueThreshold:N0} (train median) is 1, otherwise 0");
            s.Add($"Games with no review score: {NoReviewCount}, free games: {FreeCount}");
            s.Add("Features: " + string.Join(", ", FeatureNames));
            s.Add("copiesSold is excluded because revenue is copies x price");
            return s;
        }

        public static void Shuffle<T>(T[] a, Random rnd)
        {
            for (int i = a.Length - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                T t = a[i];
                a[i] = a[j];
                a[j] = t;
            }
        }

        static double Scale(double v, double lo, double hi)
        {
            if (hi <= lo) return 0;
            double s = (v - lo) / (hi - lo);
            return s < 0 ? 0 : (s > 1 ? 1 : s);
        }

        static bool Num(string s, out double v)
        {
            return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v);
        }

        static int Column(List<string> header, string name)
        {
            int i = header.IndexOf(name);
            if (i < 0) throw new InvalidDataException("Missing column: " + name);
            return i;
        }

        static List<string> SplitCsv(string line)
        {
            List<string> fields = new List<string>();
            StringBuilder sb = new StringBuilder();
            bool quoted = false;
            foreach (char c in line)
            {
                if (c == '"') quoted = !quoted;
                else if (c == ',' && !quoted)
                {
                    fields.Add(sb.ToString());
                    sb.Length = 0;
                }
                else sb.Append(c);
            }
            fields.Add(sb.ToString());
            return fields;
        }
    }
}
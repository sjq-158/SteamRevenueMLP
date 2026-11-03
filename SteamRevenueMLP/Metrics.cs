using Backprop;

namespace SteamRevenueMLP
{
    public class EvalResult
    {
        public double Mse;
        public int TP, FP, TN, FN;

        public double Accuracy
        {
            get { return (double)(TP + TN) / (TP + FP + TN + FN); }
        }

        public double Precision
        {
            get { return TP + FP == 0 ? 0 : (double)TP / (TP + FP); }
        }

        public double Recall
        {
            get { return TP + FN == 0 ? 0 : (double)TP / (TP + FN); }
        }

        public double F1
        {
            get
            {
                double p = Precision, r = Recall;
                return p + r == 0 ? 0 : 2 * p * r / (p + r);
            }
        }
    }

    public static class Metrics
    {
        public static EvalResult Evaluate(NeuralNet net, double[][] x, double[] y)
        {
            EvalResult r = new EvalResult();
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                for (int k = 0; k < x[i].Length; k++) net.setInputs(k, x[i][k]);
                net.run();
                double o = net.getOuputData(0);
                double d = y[i] - o;
                sum += d * d;

                bool predicted = o >= 0.5;
                bool actual = y[i] >= 0.5;
                if (predicted && actual) r.TP++;
                else if (predicted) r.FP++;
                else if (actual) r.FN++;
                else r.TN++;
            }
            r.Mse = sum / x.Length;
            return r;
        }

        public static double MajorityBaseline(double[] y)
        {
            int positives = 0;
            foreach (double v in y)
            {
                if (v >= 0.5) positives++;
            }
            return (double)System.Math.Max(positives, y.Length - positives) / y.Length;
        }
    }
}
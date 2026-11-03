using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Backprop;

namespace SteamRevenueMLP
{
    public partial class Form1 : Form
    {
        const string DefaultCsv = "Steam_2024_bestRevenue_1500.csv";

        SteamDataset data = new SteamDataset();
        NeuralNet net;
        List<int> chartEpochs = new List<int>();
        List<double> trainLoss = new List<double>();
        List<double> valLoss = new List<double>();
        List<string> csvLines = new List<string>();
        string csvName = "training_log.csv";
        bool stop;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Path.Combine(Application.StartupPath, "Data", DefaultCsv);
            if (File.Exists(path)) LoadData(path);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV files (*.csv)|*.csv";
                if (ofd.ShowDialog() == DialogResult.OK) LoadData(ofd.FileName);
            }
        }

        void LoadData(string path)
        {
            try
            {
                data.Load(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load the CSV: " + ex.Message);
                return;
            }
            lblFile.Text = Path.GetFileName(path) + " loaded";
            txtLog.Clear();
            foreach (string line in data.Summary()) Log(line);
        }

        NeuralNet BuildNet(int hidden, double lr)
        {
            NeuralNet n = new NeuralNet(data.InputCount, hidden, 1);
            n.LRPOUT = lr;
            n.LRPIN = lr;
            return n;
        }

        void TrainEpoch(NeuralNet n, int[] order, Random rnd)
        {
            SteamDataset.Shuffle(order, rnd);
            foreach (int i in order)
            {
                double[] x = data.TrainX[i];
                for (int k = 0; k < x.Length; k++) n.setInputs(k, x[k]);
                n.setDesiredOutput(0, data.TrainY[i]);
                n.learn();
            }
        }

        int[] NewOrder()
        {
            int[] order = new int[data.TrainX.Length];
            for (int i = 0; i < order.Length; i++) order[i] = i;
            return order;
        }

        bool EnsureData()
        {
            if (data.TrainX == null)
            {
                MessageBox.Show("Load the CSV first.");
                return false;
            }
            return true;
        }

        void SetBusy(bool busy)
        {
            btnLoad.Enabled = !busy;
            btnTrain.Enabled = !busy;
            btnTune.Enabled = !busy;
            btnStop.Enabled = busy;
            btnSaveCsv.Enabled = !busy && csvLines.Count > 1;
        }

        void Pump()
        {
            Application.DoEvents();
            if (IsDisposed) stop = true;
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            if (!EnsureData()) return;

            int hidden = (int)nudHidden.Value;
            int epochs = (int)nudEpochs.Value;
            int every = (int)nudEvery.Value;
            double lr = (double)nudLr.Value;

            net = BuildNet(hidden, lr);
            chartEpochs.Clear();
            trainLoss.Clear();
            valLoss.Clear();
            csvLines.Clear();
            csvLines.Add("epoch,trainMSE,valMSE,trainAcc,valAcc");
            csvName = "training_log.csv";

            txtLog.Clear();
            foreach (string line in data.Summary()) Log(line);
            Log($"Network: {data.InputCount} inputs, {hidden} hidden, 1 output, learning rate {lr}, epochs {epochs}");
            Log("");
            Log(" Epoch   TrainMSE     ValMSE  TrainAcc  ValAcc");

            stop = false;
            SetBusy(true);
            int[] order = NewOrder();
            Random rnd = new Random();

            for (int epoch = 1; epoch <= epochs; epoch++)
            {
                TrainEpoch(net, order, rnd);
                if (epoch == 1 || epoch % every == 0 || epoch == epochs) Record(epoch);
                if (epoch % 10 == 0)
                {
                    Pump();
                    if (stop) break;
                }
            }
            if (IsDisposed) return;

            Report();
            SetBusy(false);
        }

        private void btnTune_Click(object sender, EventArgs e)
        {
            if (!EnsureData()) return;

            int[] hiddenSizes = { 4, 8, 16 };
            double[] rates = { 0.01, 0.05, 0.1, 0.3 };
            int[] checkpoints = { 100, 300, 1000 };
            const int runs = 3;
            int maxEpochs = checkpoints[checkpoints.Length - 1];

            csvLines.Clear();
            csvLines.Add("hidden,learningRate,epochs,trainAcc,valAcc,valMSE");
            csvName = "tuning_results.csv";
            txtLog.Clear();
            Log($"Tuning on the validation set, average of {runs} runs per setting");
            Log("Hidden     LR  Epochs  TrainAcc    ValAcc    ValMSE");

            int bestHidden = 0, bestEpochs = 0;
            double bestRate = 0, bestAcc = -1, bestMse = double.MaxValue;

            stop = false;
            SetBusy(true);

            foreach (int h in hiddenSizes)
            {
                foreach (double lr in rates)
                {
                    if (stop) break;
                    double[] trAcc = new double[checkpoints.Length];
                    double[] vaAcc = new double[checkpoints.Length];
                    double[] vaMse = new double[checkpoints.Length];

                    for (int run = 0; run < runs && !stop; run++)
                    {
                        NeuralNet n = BuildNet(h, lr);
                        int[] order = NewOrder();
                        Random rnd = new Random();
                        int c = 0;
                        for (int epoch = 1; epoch <= maxEpochs && !stop; epoch++)
                        {
                            TrainEpoch(n, order, rnd);
                            if (epoch == checkpoints[c])
                            {
                                EvalResult tr = Metrics.Evaluate(n, data.TrainX, data.TrainY);
                                EvalResult va = Metrics.Evaluate(n, data.ValX, data.ValY);
                                trAcc[c] += tr.Accuracy / runs;
                                vaAcc[c] += va.Accuracy / runs;
                                vaMse[c] += va.Mse / runs;
                                c++;
                            }
                            if (epoch % 20 == 0) Pump();
                        }
                    }
                    if (stop) break;

                    for (int c = 0; c < checkpoints.Length; c++)
                    {
                        Log($"{h,6} {lr,6:F3} {checkpoints[c],7} {trAcc[c],9:P1} {vaAcc[c],9:P1} {vaMse[c],9:F4}");
                        csvLines.Add($"{h},{lr},{checkpoints[c]},{trAcc[c]:F4},{vaAcc[c]:F4},{vaMse[c]:F6}");
                        bool better = vaAcc[c] > bestAcc + 1e-9 ||
                                      (Math.Abs(vaAcc[c] - bestAcc) <= 1e-9 && vaMse[c] < bestMse);
                        if (better)
                        {
                            bestAcc = vaAcc[c];
                            bestMse = vaMse[c];
                            bestHidden = h;
                            bestRate = lr;
                            bestEpochs = checkpoints[c];
                        }
                    }
                }
                if (stop) break;
            }
            if (IsDisposed) return;

            Log("");
            if (stop)
            {
                Log("Tuning stopped before the grid finished.");
            }
            else
            {
                nudHidden.Value = bestHidden;
                nudLr.Value = (decimal)bestRate;
                nudEpochs.Value = bestEpochs;
                Log($"Best on validation: hidden {bestHidden}, learning rate {bestRate}, epochs {bestEpochs}, accuracy {bestAcc:P2}");
                Log("The controls were updated. Press Train for the final model and the test set evaluation.");
            }
            SetBusy(false);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        void Record(int epoch)
        {
            EvalResult tr = Metrics.Evaluate(net, data.TrainX, data.TrainY);
            EvalResult va = Metrics.Evaluate(net, data.ValX, data.ValY);
            chartEpochs.Add(epoch);
            trainLoss.Add(tr.Mse);
            valLoss.Add(va.Mse);
            csvLines.Add($"{epoch},{tr.Mse:F6},{va.Mse:F6},{tr.Accuracy:F4},{va.Accuracy:F4}");
            Log($"{epoch,6} {tr.Mse,10:F6} {va.Mse,10:F6} {tr.Accuracy,9:P1} {va.Accuracy,7:P1}");
            pbChart.Invalidate();
        }

        void Report()
        {
            EvalResult t = Metrics.Evaluate(net, data.TestX, data.TestY);
            double baseline = Metrics.MajorityBaseline(data.TestY);

            Log("");
            Log("Test set evaluation (data not used for training or tuning)");
            Log($"Test MSE : {t.Mse:F6}");
            Log($"Accuracy : {t.Accuracy:P2} (majority-class baseline {baseline:P2})");
            Log($"Precision: {t.Precision:P2}");
            Log($"Recall   : {t.Recall:P2}");
            Log($"F1 score : {t.F1:F4}");
            Log($"Gain over baseline: {(t.Accuracy - baseline) * 100:F1} percentage points");
            Log("");
            Log("Confusion matrix (rows are actual, columns are predicted)");
            Log("              Pred high   Pred low");
            Log($"Actual high {t.TP,11} {t.FN,10}");
            Log($"Actual low  {t.FP,11} {t.TN,10}");
        }

        void Log(string s)
        {
            txtLog.AppendText(s + "\r\n");
        }

        private void btnSaveCsv_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv";
                sfd.FileName = csvName;
                if (sfd.ShowDialog() == DialogResult.OK) File.WriteAllLines(sfd.FileName, csvLines);
            }
        }

        private void pbChart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            int n = trainLoss.Count;
            if (n < 2) return;

            double max = 0;
            for (int i = 0; i < n; i++) max = Math.Max(max, Math.Max(trainLoss[i], valLoss[i]));
            if (max <= 0) return;

            RectangleF plot = new RectangleF(45, 30, pbChart.Width - 65, pbChart.Height - 60);
            g.DrawRectangle(Pens.Gray, plot.X, plot.Y, plot.Width, plot.Height);
            DrawSeries(g, trainLoss, Pens.Blue, plot, max);
            DrawSeries(g, valLoss, Pens.Red, plot, max);

            g.DrawString("Loss (MSE)", Font, Brushes.Black, 4, 4);
            g.DrawString("Train", Font, Brushes.Blue, plot.Right - 110, 4);
            g.DrawString("Validation", Font, Brushes.Red, plot.Right - 65, 4);
            g.DrawString(max.ToString("F4"), Font, Brushes.Black, 2, plot.Y - 6);
            g.DrawString("0", Font, Brushes.Black, 30, plot.Bottom - 8);
            g.DrawString("epoch " + chartEpochs[0], Font, Brushes.Black, plot.X, plot.Bottom + 4);
            g.DrawString("epoch " + chartEpochs[n - 1], Font, Brushes.Black, plot.Right - 55, plot.Bottom + 4);
        }

        void DrawSeries(Graphics g, List<double> values, Pen pen, RectangleF plot, double max)
        {
            PointF[] pts = new PointF[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                pts[i] = new PointF(
                    plot.X + plot.Width * i / (values.Count - 1),
                    plot.Bottom - plot.Height * (float)(values[i] / max));
            }
            g.DrawLines(pen, pts);
        }
    }
}
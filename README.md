# Steam Revenue Classifier: Custom Multi-Layer Perceptron in C#

A Windows Forms application that trains a 3-layer MLP (input, hidden, output) with backpropagation to classify Steam games by revenue. It uses the provided `Backprop` classes (`INeuron`, `HNeuron`, `ONeuron`, `NeuralNet`) with no change to the algorithm. The only edit is that `LRPOUT` and `LRPIN` in `NeuralNet` are public fields instead of constants, so the learning rate can be tuned from the UI.

## Problem

Binary classification: predict whether a game's revenue is at or above the median revenue of the training set (1 = high, 0 = low).

## Dataset

`Data/Steam_2024_bestRevenue_1500.csv` contains 1,500 Steam games released in 2024, ranked by revenue.

Used columns: `price`, `avgPlaytime`, `reviewScore`, `publisherClass`, and `revenue` (to build the target).

Not used:
- `copiesSold`: revenue is roughly copies sold times price, so using it would leak the answer.
- `releaseDate`: every game is from 2024 and release month showed no consistent relationship with revenue.
- `name`, `publishers`, `developers`, `steamId`: identifiers or free text.

Exploratory observations on the full dataset (share of games at or above the median revenue):

| Group | Share above median |
|---|---|
| Free games (85) | 65% |
| Price 0.01 to 5 | 24% |
| Price 10 to 15 | 45% |
| Price above 30 | 78% |
| Review score 0 (99 games) | 81% |
| Lowest playtime fifth | 23% |
| Highest playtime fifth | 76% |

Median revenue by publisher class: AAA 928,862, AA 672,187, Indie 90,795.

A review score of 0 means no score is available, and those games are mostly high earners, so the missing value carries information and is kept as a feature.

## Preprocessing

- Rows with unparseable or negative values are skipped.
- Data is shuffled with a fixed seed and split 70% train, 15% validation, 15% test.
- Min and max for scaling, the mean review score, and the revenue median are computed from the training rows only.
- Features (7): price (min-max), isFree, log(1 + avgPlaytime) (min-max), reviewScore (min-max, mean-imputed when 0), noReview flag, isAAA, isAA. Indie is the reference class.

## Model

- 7 inputs, a tunable hidden layer, 1 sigmoid output neuron.
- Online (per-sample) gradient descent with backpropagation from the provided library. Samples are reshuffled every epoch.
- The same learning rate is used for both layers.
- Loss: mean squared error of the sigmoid output against the 0/1 label.

## Evaluation

Reported on the held-out test set: accuracy, precision, recall, F1, confusion matrix, and the majority-class baseline accuracy. The loss curve (train and validation) is drawn in the app and logged to CSV.

## Hyperparameter tuning

Grid search on the validation set: hidden nodes {4, 8, 16}, learning rate {0.01, 0.05, 0.1, 0.3}, epochs {100, 300, 1000}. Each setting is averaged over 3 runs because initial weights are random. The best setting is chosen by validation accuracy, with validation MSE as the tie-breaker. Full table: `results/tuning_results.csv`.

Chosen configuration: hidden = _, learning rate = _, epochs = _ (replace with your result and explain why).

## Results

Replace with your numbers after running.

| Metric | Value |
|---|---|
| Test accuracy | |
| Majority baseline | |
| Precision | |
| Recall | |
| F1 | |

Training log: `results/training_log.csv`.

## How to run

1. Open `SteamRevenueMLP.sln` in Visual Studio.
2. Make sure `Data/Steam_2024_bestRevenue_1500.csv` has Copy to Output Directory set to Copy if newer.
3. Press F5. The CSV loads automatically.
4. Click Tune Parameters (optional), then Train. Use Save CSV to export logs.

## Project structure

- `Program.cs`: entry point
- `Form1.cs`, `Form1.Designer.cs`: UI, training loop, tuning, evaluation report
- `SteamDataset.cs`: CSV parsing, preprocessing, train/validation/test split
- `Metrics.cs`: evaluation metrics
- `InputN.cs`, `HiddenN.cs`, `OuputN.cs`, `BackNeural.cs`: provided neural network library

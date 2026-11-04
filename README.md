# Steam Revenue Classifier: Custom MLP in C#

Group Members: Christian V. Buzarang and Ericka Fatima Reign R. Ligaray
BSCS - 3 F2 | CS345 - Intelligent Systems

A Windows Forms app that trains a 3-layer MLP (input, hidden, output) with backpropagation to predict
whether a Steam game's 2024 revenue is at or above the median. It uses only the provided `Backprop`
classes (`INeuron`, `HNeuron`, `ONeuron`, `NeuralNet`). The one edit is that `LRPOUT` and `LRPIN` are
public fields so the UI can set the learning rate.

## Problem

Binary classification (1 = revenue at or above the training median, 0 = below). Raw revenue is extremely
right-skewed (skew 22.9, max 838M vs median 109K), so regression on it is unstable. A median split is not.

## Data

`Data/Steam_2024_bestRevenue_1500.csv`: 1,500 Steam games from 2024.

- Features (7): price, isFree, log(avgPlaytime), reviewScore, noReview flag, isAAA, isAA. Numeric values are
  min-max scaled to [0, 1]; scaling values, the mean review score and the median are taken from training rows only.
- Dropped: `copiesSold` (revenue = copies x price, so it leaks the label), `releaseDate`, and identifier/text columns.
- Split: 70% train / 15% validation / 15% test, shuffled with a fixed seed.

## Model

7 inputs, tunable hidden layer, 1 sigmoid output. Per-sample gradient descent, reshuffled every epoch.
Loss is MSE against the 0/1 label.

## Tuning

Grid on the validation set: hidden {4, 8, 16} x learning rate {0.01, 0.05, 0.1, 0.3} x epochs {100, 300, 1000},
averaged over 3 runs. Results: `results/tuning_results.csv`.

Chosen: hidden = 8, learning rate = 0.05, epochs = 300 (validation accuracy 75.4%, validation MSE 0.1815).
The validation set is only 225 games, so differences of 1 to 2 points between the top settings are noise.
We picked the smallest network and a moderate rate among the top group. Loss flattens by about epoch 100,
and longer runs or higher rates did not improve validation accuracy.

## Results

Fill in from your own run.

| Metric | Value |
|---|---|
| Test accuracy | |
| Majority baseline | |
| Precision | |
| Recall | |
| F1 | |

Training log: `results/training_log.csv`.

## Run

1. Open `SteamRevenueMLP.slnx` in Visual Studio.
2. Set `Data/Steam_2024_bestRevenue_1500.csv` to Copy if newer.
3. Press F5. The CSV loads automatically.
4. Optional: Tune Parameters. Then Train. Tick Show loss graph to see the loss curve. Save CSV exports the log.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace MitchBarry.Bot.GuessANumber.Dialogs
{
    public class PlayGameDialog : CancelAndHelpDialog
    {
        private const string MinimumNumberStepMsgText = "What is the lowest number in this game?";
        private const string MaximumNumberStepMsgText = "What is the highest number in this game?";
        private const string InvalidGuessStepMsgText = "That wasn't a numeric response.";
        private const string GuessStepMsgText = "Take a guess!";
        private const string GuessAgainStepMsgText = "Take another guess!";
        private string[] WrongGuessResponses = new[]
        {
            "Hmm... that's not it. Try again!",
            "So close! (or were you?) Try again!",
            "WRONG! Try again!",
            "No way! Try again!",
            "Even I could have gotten it by now! Try again!"
        };

        private string GetWrongGuessResponse() => WrongGuessResponses[new Random().Next(0, WrongGuessResponses.Length)];

        private string GetSuccessfulResponse(GameDetails gameDetails)
        {
            return $"It took you {gameDetails.Guesses.Count} guesses, but you got it!";
        }

        public PlayGameDialog()
            : base(nameof(PlayGameDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                MinimumNumberStepAsync,
                MaximumNumberStepAsync,
                GuessStepAsync,
                CheckGuessStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> MinimumNumberStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var gameDetails = (GameDetails)stepContext.Options;

            if (gameDetails.MinimumNumber == null)
            {
                var promptMessage = MessageFactory.Text(MinimumNumberStepMsgText, MinimumNumberStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(NumberPrompt<int>), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(gameDetails.MinimumNumber, cancellationToken);
        }

        private async Task<DialogTurnResult> MaximumNumberStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var gameDetails = (GameDetails)stepContext.Options;

            if (int.TryParse(stepContext.Result.ToString(), out int numberProvided))
            {
                gameDetails.MinimumNumber = numberProvided;
            }
            else
            {
                return await stepContext.ReplaceDialogAsync(this.InitialDialogId, gameDetails);
            }


            if (gameDetails.MaximumNumber == null)
            {
                var promptMessage = MessageFactory.Text(MaximumNumberStepMsgText, MaximumNumberStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(NumberPrompt<int>), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(gameDetails.MaximumNumber, cancellationToken);
        }

        private async Task<DialogTurnResult> GuessStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var gameDetails = (GameDetails)stepContext.Options;

            if (!int.TryParse(stepContext.Result.ToString(), out int numberProvided))
            {
                if (gameDetails.MaximumNumber == null)
                {
                    // ask previous again
                    return await stepContext.ReplaceDialogAsync(this.InitialDialogId, gameDetails);
                }
            }

            gameDetails.MaximumNumber ??= numberProvided;

            // choose a number!
            if (gameDetails.ChosenNumber == null)
            {
                gameDetails.ChosenNumber = new Random().Next(gameDetails.MinimumNumber.Value, gameDetails.MaximumNumber.Value + 1);
            }

            var promptMessage = MessageFactory.Text(GuessStepMsgText, GuessStepMsgText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(NumberPrompt<int>), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> CheckGuessStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var gameDetails = (GameDetails)stepContext.Options;

            if (!int.TryParse(stepContext.Result.ToString(), out int numberProvided))
            {
                var promptMessage = MessageFactory.Text(InvalidGuessStepMsgText, InvalidGuessStepMsgText, InputHints.AcceptingInput);
                await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                // ask previous again
                return await stepContext.ReplaceDialogAsync(this.InitialDialogId, gameDetails);
            }

            gameDetails.Guesses.Add(numberProvided);

            if (numberProvided != gameDetails.ChosenNumber)
            {
                var wrongMessage = GetWrongGuessResponse();
                var promptMessage = MessageFactory.Text(wrongMessage, wrongMessage);
                await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                // ask again
                return await stepContext.ReplaceDialogAsync(this.InitialDialogId, gameDetails);
            }
            else
            {
                var successMessage = GetSuccessfulResponse(gameDetails);
                var promptMessage = MessageFactory.Text(successMessage, successMessage);
                await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}

// <auto-generated>
// Code generated by luis:generate:cs
// Tool github: https://github.com/microsoft/botframework-cli
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
namespace MitchBarry.Bot.GuessANumber.CognitiveModels
{
    public partial class Game: IRecognizerConvert
    {
        [JsonProperty("text")]
        public string Text;

        [JsonProperty("alteredText")]
        public string AlteredText;

        public enum Intent {
            Cancel,
            None,
            PlayGame
        };
        [JsonProperty("intents")]
        public Dictionary<Intent, IntentScore> Intents;

        public class _Entities
        {
            // Built-in entities
            public DateTimeSpec[] datetime;

            // Lists
            public string[][] Airport;


            // Composites
            public class _InstanceNumberRange
            {
                public InstanceData[] minimumNumber;
                public InstanceData[] maximumNumber;
            }
            public class NumberRangeClass
            {
                public string[] minimumNumber;
                public string[] maximumNumber;
                [JsonProperty("$instance")]
                public _InstanceNumberRange _instance;
            }
            public NumberRangeClass[] numberRange;

            // Instance
            public class _Instance
            {
                public InstanceData[] Airport;
                public InstanceData[] datetime;
                public InstanceData[] maximumNumber;
                public InstanceData[] minimumNumber;
                public InstanceData[] numberRange;
            }
            [JsonProperty("$instance")]
            public _Instance _instance;
        }
        [JsonProperty("entities")]
        public _Entities Entities;

        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> Properties {get; set; }

        public void Convert(dynamic result)
        {
            var app = JsonConvert.DeserializeObject<Game>(
                JsonConvert.SerializeObject(
                    result,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Error = OnError }
                )
            );
            Text = app.Text;
            AlteredText = app.AlteredText;
            Intents = app.Intents;
            Entities = app.Entities;
            Properties = app.Properties;
        }

        private static void OnError(object sender, ErrorEventArgs args)
        {
            // If needed, put your custom error logic here
            Console.WriteLine(args.ErrorContext.Error.Message);
            args.ErrorContext.Handled = true;
        }

        public (Intent intent, double score) TopIntent()
        {
            Intent maxIntent = Intent.None;
            var max = 0.0;
            foreach (var entry in Intents)
            {
                if (entry.Value.Score > max)
                {
                    maxIntent = entry.Key;
                    max = entry.Value.Score.Value;
                }
            }
            return (maxIntent, max);
        }
    }
}

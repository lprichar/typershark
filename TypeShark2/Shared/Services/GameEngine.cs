using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Shared.Services
{
    public interface IGameEngine
    {
        Task ToggleGameState(GameState gameState);
        GameState CreateGame();
        Task<List<SharkDto>> OnKeyPress(GameState gameState, string key, string playerName);
    }

    /// <summary>
    /// The game engine sends notifications like here when a shark is added.  For single player mode
    /// the game engine event handler sends requests directly back to the GameComponent.  For multi-player
    /// mode the engine event handler sends notifications back to the client through SignalR.
    /// </summary>
    public interface IGameEngineEventHandler
    {
        void GameChanged(GameDto gameDto);
        void GameOver(GameDto gameDto);
        void SharkAdded(SharkChangedEventArgs sharkAddedEventArgs);
    }

    public class GameEngine : IGameEngine
    {
        private static readonly string[] Letters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        private static readonly string[] Words2 = { "AA", "AB", "AD", "AE", "AG", "AH", "AI", "AL", "AM", "AN", "AR", "AS", "AT", "AW", "AX", "AY", "BA", "BE", "BI", "BO", "BY", "CH", "DA", "DE", "DI", "DO", "EA", "ED", "EE", "EF", "EH", "EL", "EM", "EN", "ER", "ES", "ET", "EX", "FA", "FE", "FY", "GI", "GO", "GU", "HA", "HE", "HI", "HM", "HO", "ID", "IF", "IN", "IO", "IS", "IT", "JA", "JO", "KA", "KI", "KO", "KY", "LA", "LI", "LO", "MA", "ME", "MI", "MM", "MO", "MU", "MY", "NA", "NE", "NO", "NU", "NY", "OB", "OD", "OE", "OF", "OH", "OI", "OM", "ON", "OO", "OP", "OR", "OS", "OU", "OW", "OX", "OY", "PA", "PE", "PI", "PO", "QI", "RE", "SH", "SI", "SO", "ST", "TA", "TE", "TI", "TO", "UG", "UH", "UM", "UN", "UP", "UR", "US", "UT", "WE", "WO", "XI", "XU", "YA", "YE", "YO", "YU", "ZA", "ZO" };
        private static readonly string[] Words3 = { "run", "dad", "mom", "fog", "pot", "pin", "jam", "ill", "art", "dam", "nut", "yam", "sip", "cup", "eye", "use", "cow", "key", "yak", "pop", "egg", "cub", "war", "bag", "fax", "icy", "own", "fry", "man", "hum", "shy", "low", "old", "tug", "sad", "hot", "pig", "wet", "toe", "mix", "new", "sea", "fit", "bit", "lie", "saw", "tip", "ice", "box", "ray", "few", "wax", "rob", "pan", "way", "end", "dog", "bow", "zip", "tap", "sin", "ten", "end", "toy", "spy", "cat", "men", "hug" };
        private static readonly string[] Words4 = { "milk", "care", "star", "pick", "hook", "nice", "neat", "sail", "kill", "toad", "yard", "hour", "race", "meal", "rock", "lean", "fail", "kick", "tent", "mark", "tick", "cars", "type", "fact", "rain", "many", "seat", "sail", "puny", "tart", "load", "fang", "lake", "zoom", "clip", "doll", "rich", "town", "joke", "fowl", "soak", "kiss", "past", "aunt", "pale", "bike", "boil", "rate", "flat", "milk", "ants", "grey", "coat", "cats", "trip", "soap", "talk", "walk", "wipe", "vein", "spot", "word", "mine", "self", "page", "dust", "mint", "tour", "tire", "toes", "warn", "side", "weak", "drop", "rail", "land", "sick", "bury", "snow", "whip", "vest", "beam", "calm", "flag", "debt", "call", "camp", "hand", "nail", "nest", "bolt", "zany", "nest", "book", "risk", "free", "tiny", "land", "sore", "lamp", "sock", "root", "deer", "fall", "bang", "wise", "drop", "wool", "acid", "lick", "hunt", "look", "cute", "tame", "fade", "long", "jump", "sign", "hurt", "pump", "drip", "team", "peep", "post", "wind", "idea", "turn", "boat", "melt", "soda", "bump", "frog", "wide", "vase", "dime", "join", "face", "rake", "kiss", "blow", "comb", "wink", "road", "plot", "food", "pull", "pine", "song", "iron", "pipe", "unit", "deep", "slap", "cent", "foot", "care", "wail", "work", "shop", "jail", "knit", "blue", "heal", "grab", "whip", "move", "drag", "hang", "yawn", "cast", "roll", "mute", "null", "meat", "hill", "lock", "pail", "wary", "horn", "coil", "suit", "glib", "join", "talk", "mine", "spot", "thin", "bell", "tidy", "mean", "thaw", "poor", "seal", "next", "bulb", "wren", "ship", "same", "zinc", "kick", "rare", "mice", "rush", "card", "flap", "form", "fold", "back", "lame", "soup", "ring", "pass", "huge", "worm", "good", "hall", "wash", "book", "slim", "trot", "skip", "pear", "fuel", "curl", "twig", "dear", "suck", "rule", "hate", "fool", "ripe", "bear", "loaf", "icky", "dust", "wire", "park", "wall", "wave", "note", "knot", "hair", "pour", "blot", "coal", "heat", "mend", "salt", "neck", "week", "pink", "mass", "fine", "cook" };
        private static readonly string[] Words5 = { "drain", "uncle", "scold", "horse", "quilt", "honey", "place", "birds", "slave", "strap", "straw", "empty", "steep", "skate", "tight", "clean", "group", "frogs", "anger", "drink", "spoon", "cheap", "grate", "lying", "stamp", "month", "jeans", "glove", "milky", "sulky", "slope", "cross", "stove", "close", "wheel", "front", "pushy", "house", "daffy", "hands", "match", "north", "upset", "trust", "level", "snore", "alive", "glass", "dizzy", "kitty", "smell", "trick", "alarm", "grape", "brown", "dream", "legal", "mouth", "three", "salty", "shame", "money", "burst", "abaft", "annoy", "irate", "black", "meaty", "cheer", "range", "haunt", "quick", "quack", "books", "rhyme", "rigid", "relax", "spell", "elite", "pinch", "overt", "water", "bells", "curve", "trace", "spark", "table", "smoke", "train", "ducks", "decay", "puffy", "alert", "whole", "proud", "tramp", "train", "spiky", "thumb", "birth", "wacky", "awake", "woozy", "steel", "faded", "watch", "aloof", "wound", "minor", "blind", "humor", "waste", "great", "phone", "punch", "kaput", "cough", "field", "avoid", "night", "cough", "slimy", "shock", "waves", "songs", "woman", "cloth", "found", "flaky", "nerve", "goofy", "blood", "point", "brave", "smash", "cheat", "weigh", "force", "short", "cause", "queue", "waste", "young", "prick", "trail", "bless", "offer", "funny", "clean", "skirt", "aback", "robin", "basin", "brick", "giddy", "screw", "scene", "spicy", "thank", "coach", "rifle", "tense", "plant", "spill", "shave", "crack", "store", "coast", "shoes", "drain", "husky", "white", "cause", "shaky", "queen", "sense", "fuzzy", "yummy", "elfin", "burly", "godly", "crash", "carry", "shade", "shelf", "delay", "smile", "testy", "sleep", "juicy", "rings", "tacit", "flood", "crown", "share", "dapper", "string", "spotty", "follow", "loving", "invent", "unused", "preach", "modern", "basket", "gaping", "travel", "wooden", "boring", "ground", "inject", "bushes", "cloudy", "normal", "skinny", "branch", "lonely", "throat", "bottle", "action", "cooing", "employ", "wander", "plucky", "absurd", "curved", "breezy", "arrive", "throne", "lively", "clammy", "snakes", "houses", "system", "melted", "report", "voyage", "fluffy", "offend", "stitch", "drawer", "poison", "camera", "porter", "depend", "record", "feeble", "street", "remind", "prefer", "clumsy", "summer", "sedate", "classy", "wrench", "filthy", "sticks", "reject", "locket", "wiggly", "thread", "church", "ablaze", "arrest", "lumber", "divide", "silver", "bucket", "return", "parcel", "bleach", "liquid", "cactus", "wealth", "hammer", "donkey", "advise", "pizzas", "riddle", "squeal", "stroke", "cattle", "potato", "tongue", "sneeze", "search", "abject", "answer", "bloody", "zipper", "battle", "creepy", "quiver", "squeak", "flight", "square", "bouncy", "silent", "wonder", "vessel", "flower", "brainy", "extend", "trucks", "kindly", "sponge", "giants", "theory", "switch", "income", "color", "acidic", "middle", "shrill", "dinner", "hungry", "recess", "strong", "things", "engine", "living", "excuse", "bubble", "trashy", "quaint", "torpid", "plough", "permit", "icicle", "planes", "handle", "powder", "uneven", "bruise", "lively", "desert", "afford", "detect", "writer", "amused", "pocket", "paltry", "battle", "attack", "damage", "glossy", "expand", "common", "hushed", "charge", "turkey", "advice", "tumble", "number", "belief", "regret", "degree", "memory", "pencil", "finger", "greasy", "attach", "closed", "sudden", "brawny", "intend", "vanish", "public", "smooth", "sister", "reward", "flimsy" };
        private static readonly string[] Words6 = { "shallow", "include", "current", "present", "envious", "adamant", "foolish", "oatmeal", "replace", "contain", "bashful", "jobless", "abusive", "delight", "wanting", "program", "impress", "analyze", "humdrum", "offbeat", "railway", "collect", "stretch", "vacuous", "finicky", "lyrical", "direful", "unkempt", "useless", "piquant", "produce", "special", "fearful", "correct", "zealous", "attempt", "request", "eatable", "unarmed", "frantic", "observe", "callous", "natural", "pretend", "wakeful", "magenta", "scrawny", "servant", "panicky", "educate", "caption", "wealthy", "nervous", "welcome", "history", "premium", "provide", "waggish", "succeed", "command", "serious", "regular", "arrange", "spiders", "impulse", "creator", "willing", "supreme", "awesome", "consist", "thought", "payment", "wrestle", "moaning", "tearful", "perform", "labored", "fairies", "bedroom", "eminent", "feigned", "curtain", "prevent", "believe", "amazing", "purring", "trouble", "worried", "wriggle", "boorish", "popcorn", "general", "society", "perfect", "deserve", "morning", "anxious", "furtive", "deliver", "amusing", "radiate", "warlike", "dashing", "respect", "approve", "squalid", "lettuce", "longing", "deadpan", "holiday", "dislike", "erratic", "develop", "country", "fireman", "hanging", "trouble", "crooked", "languid", "learned", "kittens", "present", "violent", "crowded", "breathe", "curious", "machine", "writing", "control", "obscene", "driving", "applaud", "spurious", "exultant", "minister", "continue", "exercise", "position", "abundant", "possible", "likeable", "trousers", "previous", "tasteful", "coherent", "standing", "familiar", "fabulous", "imported", "language", "downtown", "needless", "penitent", "acoustic", "diligent", "teaching", "obedient", "stocking", "gullible", "graceful", "yielding", "addition", "vigorous", "cautious", "guttural", "flawless", "aromatic", "doubtful", "wretched", "powerful", "addicted", "enormous", "debonair", "daughter", "decisive", "hypnotic", "frighten", "animated", "division", "frequent", "disagree", "mindless", "dazzling", "romantic", "complete", "friction", "wrathful", "devilish", "truthful", "didactic", "assorted", "succinct", "deranged", "grieving", "periodic", "industry", "pleasure", "unfasten", "building", "pleasant", "delicate", "hesitant", "lopsided", "actually", "religion", "horrible", "peaceful", "announce", "chemical", "material", "stranger", "exciting", "governor", "snobbish", "freezing", "decision", "maniacal", "youthful", "tiresome", "unwieldy", "abrasive", "spiteful", "activity", "surprise", "charming", "relation", "gruesome", "exchange", "gleaming", "boundary", "internal", "splendid" };
        private static readonly string[] Words7 = { "rainstorm", "toothsome", "ill-fated", "influence", "momentous", "befitting", "insurance", "wholesale", "afternoon", "draconian", "worthless", "voracious", "aftermath", "laughable", "acoustics", "political", "apologize", "imaginary", "delicious", "abandoned", "digestion", "obnoxious", "important", "apathetic", "scarecrow", "condemned", "vegetable", "depressed", "lunchroom", "agreement", "quicksand", "carpenter", "absorbing", "difficult", "squealing", "endurable", "plausible", "grotesque", "agonizing", "thinkable", "unhealthy", "overjoyed", "pollution", "ceaseless", "garrulous", "embarrass", "miscreant", "passenger", "alcoholic", "expensive", "expansion", "nostalgic", "delirious", "guarantee", "tasteless", "hilarious", "deafening", "secretive", "quizzical", "fantastic", "discovery", "woebegone", "existence", "sweltering", "chivalrous", "outrageous", "functional", "punishment", "irritating", "calculator", "thundering", "bewildered", "thoughtful", "wilderness", "earthquake", "toothbrush", "abstracted", "well-to-do", "successful", "accidental", "comparison", "disapprove", "fascinated", "victorious", "scientific", "understood", "changeable", "incredible", "beneficial", "fallacious", "experience", "motionless", "synonymous", "whispering", "attractive", "handsomely", "enchanting", "protective", "accessible", "unsuitable", "threatening", "efficacious", "substantial", "kindhearted", "painstaking", "stereotyped", "highfalutin", "astonishing", "superficial", "outstanding", "permissible", "domineering", "calculating", "dispensable", "magnificent", "overwrought", "symptomatic", "psychedelic", "cooperative", "industrious", "instinctive", "adventurous", "interesting", "frightening", "descriptive", "extra-small", "encouraging", "parsimonious", "questionable", "enthusiastic", "entertaining", "afterthought", "rambunctious", "distribution", "advertisement", "sophisticated", "knowledgeable", "materialistic", "scintillating" };
        private static readonly List<string[]> WordSets = new List<string[]> { Letters, Words3, Words4, Words5, Words6, Words7 };

        private readonly Random _random = new Random();
        private readonly IGameEngineEventHandler _gameEngineEventHandler;

        public GameEngine(IGameEngineEventHandler gameEngineEventHandler)
        {
            _gameEngineEventHandler = gameEngineEventHandler;
        }

        private async Task Start(GameState game)
        {
            game.GameDto.IsStarted = true;
            _gameEngineEventHandler.GameChanged(game.GameDto);
            await Task.Factory.StartNew(async () =>
            {
                while (game.GameDto.IsStarted)
                {
                    AddShark(game);
                    var baseDelay = GetBaseDelay(game.Sharks.Count, game.GameDto.IsEasy);
                    await Task.Delay(baseDelay + _random.Next(0, 800));
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void Stop(GameState game)
        {
            foreach (var shark in game.Sharks.ToList())
            {
                RemoveShark(game, shark);
            }
            game.GameDto.IsStarted = false;
            _gameEngineEventHandler.GameOver(game.GameDto);
        }

        private void Clear(GameState game)
        {
            game.GameDto.Score = 0;
            foreach (var shark in game.Sharks)
            {
                RemoveShark(game, shark);
            }
        }

        public async Task<List<SharkDto>> OnKeyPress(GameState game, string key, string playerName)
        {
            if (game.GameDto.IsStarted)
            {
                return LiveGameKeyPress(game, key, playerName);
            }

            if (key == "Enter")
            {
                await ToggleGameState(game);
            }

            return new List<SharkDto>();
        }

        private static List<SharkDto> LiveGameKeyPress(GameState game, string key, string playerName)
        {
            var durationSinceLastKeypress = DateTime.UtcNow - game.LastKeypress;
            bool isDuplicateKeystroke =
                durationSinceLastKeypress != null && durationSinceLastKeypress.Value.TotalMilliseconds < 10;
            if (isDuplicateKeystroke)
            {
                return new List<SharkDto>();
            }

            game.LastKeypress = DateTime.UtcNow;

            var changedSharks = game.Sharks
                .Where(i => !i.SharkDto.IsSolved)
                .ToList()
                .Select(s => s.OnKeyPress(playerName, key))
                .Where(s => s != null)
                .ToList();

            return changedSharks;
        }

        private void AddShark(GameState game)
        {
            var height = _random.Next(0, 80);
            var wordSet = GetWordSet(game.Sharks.Count, game.GameDto.IsEasy);
            var word = GetRandomWord(wordSet);
            var baseSecondsToSolve = GetBaseSecondsToSolve(game.Sharks.Count, game.GameDto.IsEasy);
            var secondsToSolve = baseSecondsToSolve + _random.Next(3, 5);
            var shark = new SharkManager(game, word, height, secondsToSolve);
            shark.OnSolved += Shark_OnSolved;
            shark.OnFailed += Shark_OnFailed;
            game.Sharks.Add(shark);
            shark.StartTimer();
            InvokeSharkAddedEvent(game, shark);
        }

        private void InvokeSharkAddedEvent(GameState game, SharkManager shark)
        {
            var sharkAddedEventArgs = GetSharkChangedEventArgs(game, shark);
            _gameEngineEventHandler.SharkAdded(sharkAddedEventArgs);
        }

        private static SharkChangedEventArgs GetSharkChangedEventArgs(GameState game, SharkManager shark)
        {
            var sharkAddedEventArgs = new SharkChangedEventArgs
            {
                SharkDto = shark.SharkDto,
                GameId = game?.GameDto?.Id ?? 0
            };
            return sharkAddedEventArgs;
        }

        private void RemoveShark(GameState game, SharkManager shark)
        {
            shark.OnSolved -= Shark_OnSolved;
            shark.OnFailed -= Shark_OnFailed;
            shark.Dispose();
            game.Sharks.Remove(shark);
        }

        private string GetRandomWord(string[] words)
        {
            var index = _random.Next(words.Length - 1);
            return words[index];
        }

        private void Shark_OnSolved(object sender, GameState game)
        {
            game.GameDto.Score++;
            _gameEngineEventHandler.GameChanged(game.GameDto);
        }

        private void Shark_OnFailed(object sender, GameState game)
        {
            Stop(game);
            if (game.GameDto.Score > 0)
            {
                game.GameDto.Message = "Game over, congratulations you scored " + game.GameDto.Score;
            }
            else
            {
                game.GameDto.Message = "Won't you please play again?";
            }

            _gameEngineEventHandler.GameOver(game.GameDto);
        }

        private static int GetBaseDelay(int sharkCount, bool isEasy)
        {
            if (isEasy)
            {
                return 1000 + (int)((10 / Math.Log(Math.Max(2, sharkCount))) * 100);
            }
            else
            {
                return (int)((1 / Math.Log(Math.Max(2, sharkCount))) * 1000);
            }
        }

        private static int GetBaseSecondsToSolve(int sharkCount, bool isEasy)
        {
            if (isEasy)
            {
                return 10 + (int)((4 / Math.Log(Math.Max(2, sharkCount))) * 2);
            }
            else
            {
                return (int)((1 / Math.Log(Math.Max(2, sharkCount))) * 7);
            }
        }

        private static string[] GetWordSet(int sharkCount, bool isEasy)
        {
            var wordSet = isEasy ? 0 : 2;
            wordSet = wordSet + (sharkCount / 10);
            wordSet = Math.Min(wordSet, WordSets.Count - 1);
            return WordSets[wordSet];
        }

        public GameState CreateGame()
        {
            return new GameState
            {
                GameDto = new GameDto(),
                Sharks = new List<SharkManager>(),
            };
        }

        public async Task ToggleGameState(GameState gameState)
        {
            if (gameState.GameDto.IsStarted)
            {
                Stop(gameState);
            }
            else
            {
                Clear(gameState);
                gameState.GameDto.Message = "";
                await Start(gameState);
            }
        }
    }

}

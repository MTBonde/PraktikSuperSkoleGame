using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CORE;
using Letters;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace Analytics
{
    public class WeightManager : PersistentSingleton<WeightManager>, IWeightManager
    {
        private Dictionary<string, int> entityWeights = new Dictionary<string, int>();
        private readonly Dictionary<string, ILanguageUnit> _languageUnits = new Dictionary<string, ILanguageUnit>();
        private readonly Dictionary<string, float> _weights = new Dictionary<string, float>();
        private readonly ILetterRepository letterRepository;

        private ConcurrentDictionary<string, ILanguageUnit> letterWeights;


        private const float InitialWeight = 50f;
        private const float WeightIncrement = 5f;
        private const float WeightDecrement = 5f;
        private const float MaxWeight = 99f;
        private const float MinWeight = 0f;

        public WeightManager(ILetterRepository letterRepository)
        {
            this.letterRepository = letterRepository;
        }
        
        public WeightManager()
        {
            letterRepository = new LetterRepository();
        }

        /// <summary>
        /// Initializes the weights for a given set of entities, setting a default weight if not already set.
        /// </summary>
        public void InitializeWeights(IEnumerable<ILanguageUnit> languageUnits)
        {
            letterWeights = PlayerManager.Instance.PlayerData.LettersWeights;
            
            foreach (var unit in languageUnits)
            {
                if (!letterWeights.ContainsKey(unit.Identifier))
                {
                    unit.Weight = InitialWeight;
                    letterWeights.TryAdd(unit.Identifier, unit);
                }
            }
        }
        
        /// <summary>
        /// Ensures that letterWeights is initialized before use.
        /// </summary>
        private void EnsureInitialized()
        {
            if (letterWeights == null)
            {
                letterWeights = PlayerManager.Instance.PlayerData.LettersWeights;
            }
        }

        /// <summary>
        /// Initializes the weights from PlayerManager's PlayerData, setting a default weight if not already set.
        /// This method should be called after PlayerManager has been initialized.
        /// </summary>
        public void InitializeWeights()
        {
            // Link letterWeights to PlayerManager's PlayerData
            letterWeights = PlayerManager.Instance.PlayerData.LettersWeights;

            // Initialize all letters from letter repository with default weight if they are not already present
            var allLetters = letterRepository.GetAllLetters();

            foreach (var letter in allLetters)
            {
                string upperIdentifier = letter.ToString().ToUpper();
                string lowerIdentifier = letter.ToString().ToLower();

                if (!letterWeights.ContainsKey(upperIdentifier))
                {
                    var upperLetterData = new LetterData(upperIdentifier,
                        LetterCategory.All, InitialWeight);
                    letterWeights.TryAdd(upperIdentifier, upperLetterData);
                    Debug.Log(
                        $"Added uppercase letter: {upperIdentifier} with initial weight {InitialWeight}");
                }

                if (!letterWeights.ContainsKey(lowerIdentifier))
                {
                    var lowerLetterData = new LetterData(lowerIdentifier,
                        LetterCategory.All, InitialWeight);
                    letterWeights.TryAdd(lowerIdentifier, lowerLetterData);
                    Debug.Log(
                        $"Added lowercase letter: {lowerIdentifier} with initial weight {InitialWeight}");
                }

                //     // if we dont want upper and lower
                //     string identifier = letter.ToString().ToUpper();
                //
                //     if (!letterWeights.ContainsKey(identifier))
                //     {
                //         var letterData = new LetterData(identifier, LetterCategory.All, InitialWeight);
                //         letterWeights.TryAdd(identifier, letterData);
                //      }
            }
        }
        
        public void PrintAllWeights()
        {
            EnsureInitialized();
            
            foreach (var unit in letterWeights)
            {
                Debug.Log($"Identifier: {unit.Key}, Weight: {unit.Value.Weight}");
            }
        }

        /// <summary>
        /// Sets the weight of a specific entity.
        /// </summary>
        public void SetEntityWeight(ILanguageUnit entity, int weight)
        {
            EnsureInitialized();
            
            if (letterWeights.ContainsKey(entity.Identifier))
            {
                letterWeights[entity.Identifier].Weight = weight;
            }
            else
            {
                Debug.LogWarning($"Entity '{entity.Identifier}' not found in WeightManager.");
            }
        }

        /// <summary>
        /// Updates the weight of an entity based on whether the action was correct.
        /// </summary>
        public void UpdateWeight(ILanguageUnit entity, bool isCorrect)
        {
            EnsureInitialized();
            
            if (letterWeights.TryGetValue(entity.Identifier, out ILanguageUnit currentUnit))
            {
                currentUnit.Weight = isCorrect
                    ? Mathf.Max(currentUnit.Weight - WeightDecrement, MinWeight)
                    : Mathf.Min(currentUnit.Weight + WeightIncrement, MaxWeight);
            }
            else
            {
                Debug.LogWarning($"Entity '{entity.Identifier}' not found for weight update.");
            }
        }
        
        public void UpdateWeight(string identifier, bool isCorrect)
        {
            EnsureInitialized();

            if (letterWeights.TryGetValue(identifier, out ILanguageUnit currentUnit))
            {
                currentUnit.Weight = isCorrect
                    ? Mathf.Max(currentUnit.Weight - WeightDecrement, MinWeight)
                    : Mathf.Min(currentUnit.Weight + WeightIncrement, MaxWeight);
            }
            else
            {
                throw new KeyNotFoundException($"Identifier {identifier} not found in letter weights.");
            }
        }

        
        public List<ILanguageUnit> GetNextLanguageUnits(LanguageUnit type, LetterCategory category, int count)
        {
            EnsureInitialized();
    
            Debug.Log($"GetNextLanguageUnits called with type: {type}, category: {category}, count: {count}");

            // filter units by type
            var filteredUnits = letterWeights.Values
                .Where(u => u.LanguageUnitType == type);

            Debug.Log($"Filtered units count after type filter: {filteredUnits.Count()}");

            if (type == LanguageUnit.Letter)
            {
                IEnumerable<string> validIdentifiers = category switch
                {
                    LetterCategory.Vowel => letterRepository.GetVowels().Select(c => c.ToString().ToUpper()),
                    LetterCategory.Consonant => letterRepository.GetConsonants().Select(c => c.ToString().ToUpper()),
                    _ => letterWeights.Keys
                };

                filteredUnits = filteredUnits.Where(u => validIdentifiers.Contains(u.Identifier));
                Debug.Log($"Filtered units count after category filter: {filteredUnits.Count()}");
            }

            var unitList = filteredUnits.ToList();
            if (!unitList.Any())
            {
                Debug.LogWarning($"No units found for type {type} and category {category}.");
                return new List<ILanguageUnit>();
            }

            // sort units by weight, then order randomly within each weight group
            var sortedUnits = unitList
                .GroupBy(u => u.Weight)
                .OrderByDescending(g => g.Key)
                .SelectMany(g => g.OrderBy(u => UnityEngine.Random.value))
                .ToList();

            Debug.Log($"Total sorted units count: {sortedUnits.Count}");

            // return the asked for count of units
            var result = sortedUnits.Take(count).ToList();

            Debug.Log($"Returning {result.Count} units.");
            foreach (var unit in result)
            {
                Debug.Log($"Unit: {unit.Identifier}, Weight: {unit.Weight}");
            }

            return result;
        }



        
        

        /// <summary>
        /// Retrieves the current weights for all entities.
        /// </summary>
        public Dictionary<string, int> GetCurrentWeights()
        {
            return new Dictionary<string, int>(entityWeights);
        }

        /// <summary>
        /// Retrieves the weights of all vowel entities.
        /// </summary>
        public Dictionary<string, int> GetVowelWeights()
        {
            var vowels = new HashSet<string>(letterRepository.GetVowels().Select(v => v.ToString()));
            return entityWeights
                .Where(e => vowels.Contains(e.Key))
                .ToDictionary(e => e.Key, e => e.Value);
        }

        /// <summary>
        /// Retrieves the weights of all consonant entities.
        /// </summary>
        public Dictionary<string, int> GetConsonantWeights()
        {
            var consonants = new HashSet<string>(letterRepository.GetConsonants().Select(c => c.ToString()));
            return entityWeights
                .Where(e => consonants.Contains(e.Key))
                .ToDictionary(e => e.Key, e => e.Value);
        }

        
    }
}
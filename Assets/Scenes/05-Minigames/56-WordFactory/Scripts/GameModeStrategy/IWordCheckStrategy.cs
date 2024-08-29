using System.Collections.Generic;
using CORE.Scripts;
using Scenes.Minigames.WordFactory.Scripts;
using Scenes.Minigames.WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._05_Minigames._56_WordFactory.Scripts.GameModeStrategy
{
    public interface IWordCheckStrategy
    {
        bool ValidateWord(string word, List<Transform> teeth);
        void Initialize(WordValidator wordValidator, HashSet<string> createdWords, ScoreManager scoreManager, INotificationDisplay notification);
    }
}